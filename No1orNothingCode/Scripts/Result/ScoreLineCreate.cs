using System.Linq;
using UnityEngine;

public class ScoreLineCreate : MonoBehaviour
{
    [SerializeField] private Material material;
    public LineRenderer lr;
    private Vector2[] newPosition;
    public int Size = 0;
    public int nowSize;
    public bool on = true;
    public Color color;
    public float t = 0;
    [SerializeField] private float speed;
    [SerializeField] private GameObject icon;
    [SerializeField]private Sprite playerIcon;
    public GameObject[] obj = new GameObject[3];

    private void Awake()
    {
        obj = new GameObject[3];
       // Debug.Log(obj.Length);
        //Debug.Log("配列設定" + RaundManager.maxRound);
        newPosition = new Vector2[5];
    }
    private void Update()
    {
        if (on) return;//初期化が完了していなければ戻る
        if (Size < nowSize) return;
        if (lr.GetPosition(nowSize) == null) return;
        if (lr.GetPosition(nowSize).x != newPosition[nowSize].x)
        {
            if(nowSize == 1)
            {
                //最初の地点を生成
                lr.SetPosition(nowSize-1, newPosition[0]);
            }
            if (t < 1)
            {
                    t = Mathf.Min(1f, t + speed * 60f * Time.unscaledDeltaTime);
                // Debug.Log(nowSize);
                for (int i = nowSize; i <= Size; i++)
                {
                    Vector3 positionMove = Vector3.Lerp(newPosition[nowSize - 1], newPosition[nowSize], t);
                    lr.SetPosition(i, positionMove);
                }
                
            }
        }
        else if(Size >= nowSize)
        {
                //アイコン表示
                if (obj[nowSize] == null)
                {
                    obj[nowSize] = Instantiate(icon, transform);
                    obj[nowSize].transform.position = newPosition[nowSize];
                    obj[nowSize].GetComponent<SpriteRenderer>().sprite = playerIcon;
                }
            if (Size > nowSize)
            {
                for (int i = nowSize; i < Size; i++)
                {
                    lr.SetPosition(i, newPosition[nowSize]);
                }
                nowSize++;//次の点へ
                t = 0;
                // Debug.Log("next" + nowSize);
            }
        }
    }

    //リザルト表示最初に実行
    //初期設定
    public void On(Vector2 newPosition,ResultScore result)
    {
        if (on)
        {
            //初期設定
            this.newPosition[0] = new Vector2(result.xMin, result.yMin);
            // Debug.Log("on");
            // Debug.Log(this.newPosition[0]);
            // Debug.Log(result.yMin);

            for (int i = obj.Length-1; i > 0; i--)
            {
                if (obj[i] == null) break;
                //アイコン消去
                Destroy(obj[i].gameObject);
            }
            Debug.Log(RoundManager.roundCount);
            obj = new GameObject[RoundManager.roundCount+1];
            //線の初期設定
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.material = material;
            lr.material.color = color;
            on = false;
            Size = 0;
            t = 0;
            for (int i=0;i < lr.positionCount;i++)
            {
                lr.SetPosition(i, this.newPosition[0]);
            }
        }
        Create(newPosition);
    }

    //新しい地点を取得
    public void Create(Vector2 newPosition)
    {
        Size++;
        //Debug.Log("線");
        Debug.Log(Size);
        this.newPosition[Size] = newPosition;
        //Debug.Log(this.newPosition[Size]);
    }
}
