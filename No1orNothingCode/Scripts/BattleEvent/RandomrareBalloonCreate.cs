using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomrareBallonCreate : MonoBehaviour
{
   
    public float interval = 0.01f;
    public float StopTime = 5f;

    public BoxCollider2D area;
    Bounds bounds;
    
    
    float createrX;
    float createrY;
    GameObject obj;


    [Header("発生間隔")]

    //生成するプレハブ格納用
    public GameObject PrefabRareBalloon;



    private void Start()
    {
        bounds = area.bounds;

        StartCoroutine(SpawnRandomlyWithinTime());
    }


    IEnumerator SpawnRandomlyWithinTime()
    {
       
        float randomTime = Random.Range(0f, StopTime);
        yield return new WaitForSeconds(randomTime);

        createrX = Random.Range(bounds.min.x, bounds.max.x);
        createrY = Random.Range(bounds.min.y, bounds.max.y);
        Vector3 pos = new Vector3(createrX, createrY);

        //プレハブ生成
        obj = Instantiate(PrefabRareBalloon, pos, Quaternion.identity);
            

        

    }


}

