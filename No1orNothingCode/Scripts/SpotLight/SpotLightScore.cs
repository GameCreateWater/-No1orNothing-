using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SpotLightScore : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float addScore;
    public float removeScore;
    public float addTime;
    public GameObject[] player = new GameObject[4];
    [SerializeField] bool ticktokMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        Reset();
    }
    public void Reset()
    {
        rb = GetComponent<Rigidbody>();
        //ティックトックモードじゃない場合範囲内のプレイヤーに常時ポイント付与
        if (ticktokMode == false)
        {
            StopCoroutine(TimeGard());
            StartCoroutine(TimeGard());
            OnEnableAttack();
        }
    }

    //Ticktokシャッター時の処理
    public void Shutter()
    {
        if (player != null)
        {
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i] != null)
                {
                   // Debug.Log("点数");
                    player[i].GetComponent<PlayerScoreManager>().ScoreChange(addScore);
                }
            }
        }
    }
    //ライト内にいる人のスコア供給の開始
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("attatata");
        if(collision.tag == "Player")
        {
            for(int i = 0; i <player.Length; i++)
            {
                if (player[i] == null)
                {
                   // Debug.Log("hairu");
                    player[i] = collision.gameObject;
                    break;
                }else if(player[i] == collision.gameObject)
                {
                    break;
                }
            }
        }
    }

    //ライトから離れた人へのスコア供給を停止
    public void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("deta");
        if(collision.tag == "Player")
        {
            //Debug.Log("player");
            for (int i = 0; i < player.Length; i++)
            {
                //Debug.Log("keisan");
                if (player[i] == collision.gameObject)
                {
                   //Debug.Log("kieta");
                    player[i] = null;
                    break;
                }
            }
        }
    }

    //PlayerKnockBackのOnAttackのInvokeの登録と処理
    public void OnEnableAttack()
    {
        PlayerKnockBack.OnAttack += SpotLightOnAttack;
    }

    public void OnDisaAttack()
    {
        PlayerKnockBack.OnAttack -= SpotLightOnAttack;
    }

    public void SpotLightOnAttack(Transform playe)
    {
        var attackPlayer = playe.gameObject;
        if (player == null) return;
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] == attackPlayer)
            {
               // Debug.Log("点数");
                player[i].GetComponent<PlayerScoreManager>().
                    ScoreChange(-removeScore);
            }
        }
    }


    //スコアを入れる
    private IEnumerator TimeGard()
    {
        while (true)
        {
            yield return new WaitForSeconds(addTime);

                if (player != null)
                {
                    for (int i = 0; i < player.Length; i++)
                    {
                        if (player[i] != null)
                        {
                            //Debug.Log("点数");
                            player[i].GetComponent<PlayerScoreManager>().
                            ScoreChange(addScore);
                        }
                    }
                }
        }
    }

}
