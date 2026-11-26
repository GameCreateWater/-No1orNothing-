using UnityEngine;

public class TargetCreatetManager : MonoBehaviour
{
    public GameObject moveObject;
    public GameObject targetObject;
    public BoxCollider2D area;
    [Header("離れてる最低距離")]
    //次の移動目標生成地点（横：ｘ）が今の目標地点から離れてる最低距離
    public float nextDistance;
    [Header("上下左右範囲の余裕")]
    public float xDistance;
    public float yDistance;

    float x;
    float y;

    GameObject obj;
    float oldPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //次の場所を生成
    public void TargetCreate()
    {
        Bounds bounds = area.bounds;
        while (Mathf.Abs(x - oldPos) < nextDistance)
        {
            x = Random.Range(bounds.min.x + xDistance, bounds.max.x - xDistance);
        }
        y = Random.Range(bounds.min.y + yDistance, bounds.max.y - yDistance);
        var point = new Vector2(x, y);
        obj =  Instantiate(targetObject,this.transform);
        obj.transform.position = point;
        oldPos = obj.transform.position.x;
        if (moveObject.GetComponent<SpotLightMove>() != null)
         {
             moveObject.GetComponent<SpotLightMove>().Newpoint(obj);
         } else if(moveObject.GetComponent<CameraAria>() != null){
             moveObject.GetComponent<CameraAria>().Newpoint(obj);
         }


    }

    public void TaragetDestroy()
    {
        Destroy(obj);
        TargetCreate();
    }
}
