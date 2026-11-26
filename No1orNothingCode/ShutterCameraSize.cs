using UnityEngine;

public class ShutterCameraSize : MonoBehaviour
{
    //ChatGPT完全コピー


    public Transform target;      // 四角オブジェクト
    public Camera cam;
    public float margin = 1.0f;   // 余白

    void LateUpdate()
    {
        // 位置を完全一致
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
        
                // スケールからサイズ取得
                Vector3 scale = target.localScale;

                float width = scale.x;
                float height = scale.y;
        
        /*
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();

        float width = sr.bounds.size.x;
        float height = sr.bounds.size.y;
        */
        // カメラサイズ調整（縦基準）
        float vertical = height / 2f;
        float horizontal = width / (2f * cam.aspect);

        cam.orthographicSize = Mathf.Max(vertical, horizontal) * margin;
    }
}
