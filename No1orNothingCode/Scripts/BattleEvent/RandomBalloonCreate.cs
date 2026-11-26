using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomBallonCreate : MonoBehaviour
{
    private float timer = 0f;
    public float interval = 0.01f;
    private float StartTime = 0f;
    public float StopTime = 5f;

    public BoxCollider2D area;
    Bounds bounds;
    public float balloonDistance;
    public float nowBalloonDistance;
    float createrX;
    float createrY;
    GameObject obj;


    [Header("発生間隔")]
    
    //生成するプレハブ格納用
    public GameObject PrefabBalloon;
    


    private void Start()
    {
        bounds = area.bounds;
        
        // StartCoroutine(TimeGard());
    }

    private void Update()
    {

       
            StartTime += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                Generate();
                timer = 0f;
            }
        
    }

    void Generate()
    {
         
        {
               
             //プレハブの位置をランダムで設定
              while (Mathf.Abs(createrX - nowBalloonDistance) < balloonDistance)
                {
                   createrX = Random.Range(bounds.min.x, bounds.max.x);
                }

                    
            createrY = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 pos = new Vector3(createrX, createrY);

           //プレハブ生成
            obj = Instantiate(PrefabBalloon, pos, Quaternion.identity);
           
            nowBalloonDistance = obj.transform.position.x;
               
         }
        
    }
    
    /*
    private IEnumerator TimeGard()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Generate();
        }
    }
    */
}
