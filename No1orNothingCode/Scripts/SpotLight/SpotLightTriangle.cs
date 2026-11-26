using System.Linq;
using UnityEngine;


public class SpotLightTriangle : MonoBehaviour
{

    //完全ChatGPTコピー
    [Header("配列に加えたオブジェクトを繋ぎ図を描く")]
    public Transform[] points;
    [Header("Visual cutoff only (does not change scoring collider)")]
    public bool clipBelowStage;
    public float stageEdgeY = -2.8f;
    private readonly System.Collections.Generic.List<Vector3> visibleVertices = new System.Collections.Generic.List<Vector3>(4);
    private readonly System.Collections.Generic.List<int> visibleTriangles = new System.Collections.Generic.List<int>(6);

    PolygonCollider2D col;
    public Mesh mesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        col = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (points == null || points.Length < 3) return;
        if (!points.All(x => x != null)) return;

        // 頂点更新
        Vector2[] colliderPosition = new Vector2[3];
        Vector3[] meshPosition = new Vector3[3];

        for (int i = 0; i < 3; i++)
        {
            colliderPosition[i] = points[i].transform.position - transform.position;
        }

        for (int i = 0; i < 3; i++)
        {
            meshPosition[i] = points[i].transform.position - transform.position;
        }
        col.points = colliderPosition;
        mesh.Clear();
        if (!clipBelowStage)
        {
            mesh.vertices = meshPosition;
            mesh.triangles = new int[] { 0, 1, 2 };
            return;
        }
        visibleVertices.Clear();
        visibleTriangles.Clear();
        for (int i = 0; i < 3; i++)
        {
            Vector3 a = points[(i + 2) % 3].position;
            Vector3 b = points[i].position;
            bool aInside = a.y >= stageEdgeY;
            bool bInside = b.y >= stageEdgeY;
            if (aInside != bInside)
                visibleVertices.Add(Vector3.Lerp(a, b, (stageEdgeY - a.y) / (b.y - a.y)) - transform.position);
            if (bInside) visibleVertices.Add(b - transform.position);
        }
        for (int i = 1; i + 1 < visibleVertices.Count; i++)
        {
            visibleTriangles.Add(0);
            visibleTriangles.Add(i);
            visibleTriangles.Add(i + 1);
        }
        mesh.SetVertices(visibleVertices);
        mesh.SetTriangles(visibleTriangles, 0);
        mesh.RecalculateBounds();
    }

    private void OnDestroy()
    {
        if (mesh != null) Destroy(mesh);
    }

}
