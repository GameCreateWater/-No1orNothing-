using UnityEngine;
using System.Collections;

public class CameraAria : MonoBehaviour
{
    [Header("ライトの移動にかかる時間")]
    public float moveTimeMin;
    public float moveTimeMax;
    float MoveTime;//移動にかかる時間
    //float MoveX;
    //float MoveY;
    Vector2 Nowposition;
    Vector2 Nextposition;
    [Header("目標生成間隔の最長と最短")]
    public float StopTimelow;
    public float StopTimehight;
    public float NowTime;

    Rigidbody2D rb;
    //現在地と目標地の差
    float differenceX = 0;
    float differenceY = 0;

    float probability;//これが50以下だとX以上だとYに直線に進ませる（True、False的な役割）
    [Header("進路変更間隔")]
    public float DirectionTime = 0;
    public float DirectionTimeBool = 0;//これがDirectionTimeになるたびに上の数値を0～100でランダムに決める
    string nowMode = "stop";
    string oldMode;
    public bool move = true;
    static bool shutter = true;

    [Header("周りの黒いもや0～255")]
    public GameObject backImage;
    public float backColorDefo;
    [Header("シャッター時の明るさbackColorDefo～255")]
    public float flashLight;
    SpriteRenderer backColor;
    Color backColorNow;
    public GameObject TargetCreate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backColorDefo = Mathf.Floor(backColorDefo / 255f * 100f) / 100f;
        flashLight = Mathf.Floor(flashLight / 255f * 100f) / 100f;
        backColor = backImage.GetComponent<SpriteRenderer>();
        backColorNow = new Color(backColorDefo,backColorDefo,backColorDefo,180f/255f);
        oldMode = nowMode;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeGard());
        Nowposition = transform.position;
    }

    private void FixedUpdate()
    {
        

        Nowposition = transform.position;
        if (Nextposition != Nowposition && move ==true)
        {
           

            if (DirectionTimeBool >= DirectionTime) 
            {
                probability = Random.Range(0, 100);
                DirectionTimeBool = 0;
            }
            int x=0;
            int y=0;
            //移動速度の決定
            if (System.Math.Abs(Nextposition.x - Nowposition.x) > 0.3f && (probability <50 || System.Math.Abs(Nextposition.y - Nowposition.y) < 0.3f))
            {
                x = 1;
                y = 0;
                nowMode = "up";


            }
            else if (System.Math.Abs(Nextposition.y - Nowposition.y) > 0.3f)
            {
                x = 0;
                y = 1;
                nowMode = "right";

            }
            else
            {
                x=0; y = 0;
            }

            if(nowMode != oldMode)
            {
                MoveTime = Random.Range(moveTimeMin,moveTimeMax);
                oldMode = nowMode;
                //move = false;
                GetComponent<CameraAriaSize>().SizeChange();
            }

                //移動処理
                rb.linearVelocity = new Vector2(differenceX * x, differenceY * y) / MoveTime;

        }else if(rb.linearVelocity.x !=0 || rb.linearVelocity.y != 0)
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
    }

    public void Newpoint(GameObject New)
    {
        Nextposition = New.transform.position;//次の目標位置取得
        NowTime = Random.Range(StopTimelow, StopTimehight);//滞在時間の決定
        differenceX = Nextposition.x - Nowposition.x;
        differenceY = Nextposition.y - Nowposition.y;
       // Debug.Log("zzz");
        shutter = true;



    }

    private IEnumerator TimeGard()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //目標地点にいる間滞在時間を減らす


            //指定位置に止まった時の挙動
            //シャッターチャンス
            if (System.Math.Abs(Nowposition.x - Nextposition.x) < 0.3f && System.Math.Abs(Nowposition.y - Nextposition.y) < 0.3f && NowTime > 0)
            {

                NowTime -= 0.1f;

                if (shutter == true)
                {
                    GetComponent<SpotLightScore>().Shutter();
                    backColor.color = new Color(flashLight, flashLight, flashLight,1);
                    MoveTime = Random.Range(moveTimeMin, moveTimeMax);
                    oldMode = nowMode;
                    move = false;
                    shutter = false;
                    GetComponent<CameraAriaSize>().xSize = transform.localScale.x - 0.5f;
                    GetComponent<CameraAriaSize>().ySize = transform.localScale.y - 0.5f;
                }
                
            }
            else if (NowTime <= 0)
            {
                TargetCreate.GetComponent<TargetCreatetManager>().TaragetDestroy();
            }
            if (move == true)
            {
                //移動方向の決定
                DirectionTimeBool += 0.1f;
            }
            //白くなったバックの画像をもとの色に戻していく
            if (backColorNow != backColor.color)
            {
                backColor.color = Color.Lerp(backColor.color, backColorNow, 0.1f);
            }

        }
    }
}
