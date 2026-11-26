using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class ResultScore : MonoBehaviour
{
    [Header("表の余白0～1")] public float maxScoreDoun;
    //[Header("最大スコア")] 
    private float maxScore = 0;
    [Header("変化完了までの時間")]
    [SerializeField] float changeTime;
    [Header("線になる空オブジェクト")]
    public GameObject[] line;

    [Header("グラフ範囲")]
    [SerializeField]float xMax;
    [SerializeField] float yMax;
    public float xMin;
    public float yMin;
    private float xDistance;
    private float yDistance;

    private static GameObject Instance;


    ResultScoreInput memory;
    ResultScoreScale scale;


    void Start()
    {
        scale = GetComponent<ResultScoreScale>();
        memory= GetComponent<ResultScoreInput>();
        memory.Result.SetActive(false);
        if (Instance == null)
        {
            Instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
            Instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    public void CreateLine()
    {
        //線の初期化
        for(int i=0;i<line.Length; i++)
        {
            line[i].GetComponent<ScoreLineCreate>().nowSize = 1;
        }
        //スコア間隔取得
        for(int p = 0; p < PlayerManager.player.Length; p++)
        {
            if (PlayerManager.player[p] == null) break;
            for (int i = 0; i < RoundManager.roundCount; i++)
            {
                var input = PlayerManager.player[p].GetComponent<PlayerInput>();
                if (maxScore < PlayerManager.ScoreTable[(input.playerIndex,i)])
                {
                        maxScore = PlayerManager.ScoreTable[(input.playerIndex, i)];
                }
            }
        }

        //わかりやすいように切りよく表示
        if (maxScore <= 10)
        {
        }
        else if (maxScore > 10)
        {
            float scaleRemainder = maxScore % 10f;
            float scale = (maxScore - scaleRemainder) / 10f;
            maxScore = 10 * (scale + 1);
        }

        //表示範囲の計算
        xDistance = Mathf.Abs(xMax - xMin) / RoundManager.maxRound;
        yDistance = Mathf.Abs(yMax - yMin);

        // 指標表示
        if (maxScore <= 0)
        {
            maxScore = 1;
        }
        var YmaxScore = maxScore * (maxScoreDoun + 1);

        //Debug.Log(yMin);
        // Debug.Log(yDistance);
        // Debug.Log(YmaxScore);
        // Debug.Log(maxScoreDoun);

        scale.ScaleMemori(maxScore, xMin, yMin + yDistance * (maxScore / YmaxScore));

        //表示回数の設定
        for (int i = 0; i < line.Length; i++)
        {
            if (RoundManager.roundCount > 1)
            {
                line[i].GetComponent<ScoreLineCreate>().lr.positionCount = RoundManager.roundCount+1;
            }
            else
            {
                line[i].GetComponent<ScoreLineCreate>().lr.positionCount = 2;
            }
        }

        //線の終点を決定
        for (int i = 0; i < RoundManager.roundCount ; i++)
        {
                for (int p = 0; p < PlayerManager.player.Length; p++)
                {
                    if (PlayerManager.player[p] == null) break;
                    var input = PlayerManager.player[p].GetComponent<PlayerInput>();
                    var y = yMin + ((PlayerManager.ScoreTable[(input.playerIndex,i)] / YmaxScore) * yDistance);//高さ設定
                    Vector2 linePosition = new Vector2(xMin + (xDistance * (i + 1)),y);//次の点位置を決定
                    //Debug.Log(linePosition);
                    // Debug.Log(line[p].gameObject);
                    line[p].GetComponent<ScoreLineCreate>().On(linePosition, this); 
                }
        }
    }
}
