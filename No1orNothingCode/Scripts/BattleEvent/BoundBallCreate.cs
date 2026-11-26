using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundBallCreate : MonoBehaviour
{

    Rigidbody2D rb;
    public BoxCollider2D area;
    Bounds bounds;
    public float ballDistance;
    public float nowBallDistance;
    float createrX;
    float createrY;
    GameObject obj;

    [Header("ボール間隔")]
    public float interval;


    public GameObject PrefabBall;

    private void Start()
    {
       
        bounds = area.bounds;
        StartCoroutine(TimeGard());
    }


    void BoundBall()
    {
       
        {
            
            while (Mathf.Abs(createrX - nowBallDistance) < ballDistance)
            {
                createrX = Random.Range(bounds.min.x, bounds.max.x);
            }

            createrY = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 pos = new Vector3(createrX, createrY);

            
            obj = Instantiate(PrefabBall, pos, Quaternion.identity);

            Rigidbody2D ballRb = obj.GetComponent<Rigidbody2D>();

            Vector3 dir = new Vector3(Random.Range(-3f,3f),0f,0f);

            nowBallDistance = obj.transform.position.x;


            ballRb.linearVelocity = (dir * 5f);
        }
    }

    private IEnumerator TimeGard()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            BoundBall();
        }
    }
}
