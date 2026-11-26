using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpotLightMove : MonoBehaviour
{
    [Header("ライトの移動速度")]
    public float MoveTime;//移動にかかる時間
    //float MoveX;
    //float MoveY;
    Vector2 Nowposition;
    Vector2 Nextposition;
    [Header("目標生成間隔の最長と最短")]
    [SerializeField] float StopTimelow;
    [SerializeField] float StopTimehight;
    public float NowTime;

    Rigidbody2D rb;
    //現在地と目標地の差
    float differenceX = 0;
    float differenceY = 0;

    public GameObject TargetCreate;

    private void Start()
    {
        Reset();
    }
    public void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeGard());
        Nowposition = transform.position;
    }

    private void FixedUpdate()
    {
        Nowposition = transform.position;
        if (Nextposition != Nowposition)
        {
            //移動完了したら速度を0に
            if (System.Math.Abs(Nextposition.x - Nowposition.x) < 0.1f)
            {
                differenceX = 0;
            }

            if (System.Math.Abs(Nextposition.y - Nowposition.y) < 0.1f)
            {
                differenceY= 0;
            }

            //移動処理
            rb.linearVelocity = new Vector2(differenceX * MoveTime, differenceY * MoveTime);
        }


    }

    //新しい目標地点の取得
    //滞在時間のリセット
    public void Newpoint(GameObject New)
    {
        Nextposition = New.transform.position;//次の目標位置取得
        NowTime = Random.Range(StopTimelow,StopTimehight);//滞在時間の決定
        //移動速度の決定
        if (Nextposition.x != Nowposition.x)
        {
            differenceX = Nextposition.x - Nowposition.x;
            differenceX = differenceX / Mathf.Abs(differenceX);
        }

        if (Nextposition.y != Nowposition.y)
        {
            differenceY = Nextposition.y - Nowposition.y;
            differenceY = differenceY / Mathf.Abs(differenceY);

        }
    }

    //0.1秒ごと処理
    private IEnumerator TimeGard()
    {
        while (true)
        {
            //Debug.Log("33");
            yield return new WaitForSeconds(0.1f);

            //目標地点にいる間滞在時間を減らす

            NowTime -= 0.1f;
            if(NowTime <= 0)
            {
                float pos = transform.position.x;
                TargetCreate.GetComponent<TargetCreatetManager>().TaragetDestroy();
            }

        }
    }
}
