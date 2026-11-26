using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResultScoreInput : MonoBehaviour
{
    public static bool scoreSave = true;
    public GameObject Result;
    public int count;
    public int countRaund;
    public static bool gameOn = false;

    void Update()
    {
        //Debug.Log(gameOn);
        if (!scoreSave && gameOn)//ロービーシーンならスコアをセーブせず次へ
        {
            if (TimeLimitManager.currentTime <= 0)
            {
                Debug.Log("ddd");
                gameOn = false;
                RoundManager.Instance.GetComponent<SceneGateSelect>().GoNextScene(null);
            }
            return;
        }
       // Debug.Log("存在する");
        if (PlayerManager.player != null)
        {
            //Debug.Log("playerあり");
            if (TimeLimitManager.currentTime <= 0 && gameOn == true && RoundManager.roundCount > 0)
            {
                if (!Result.activeInHierarchy)
                {
                    Result.SetActive(true);//リザルト表示
                    Debug.Log("ResultOn");
                }
                if (PlayerManager.player[0] != null)
                {
                    for (int p = 0; p < PlayerManager.player.Length; p++)
                    {
                        if (PlayerManager.player[p] == null) break;

                        var player = PlayerManager.player[p];
                        var input = player.GetComponent<PlayerInput>();
                        var scoreManager = player.GetComponent<PlayerScoreManager>();
                        float finalizedScore = scoreManager.FinalizeRoundScore();
                        PlayerManager.ScoreTable.Add((input.playerIndex, RoundManager.roundCount - 1), finalizedScore);
                        //スコア管理のオブジェクトに今回のラウンドスコアを記録
                    }
                }
                gameOn = false;//ゲーム停止

               // Debug.Log("カウント" + count);

                GetComponent<ResultScore>().CreateLine();//リザルトスコア線生成
            }
        }
    }
}
