using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class TimeLimitManager : MonoBehaviour
{
    [Header("時間設定(秒)")]
    public float timeLimitSeconds;

    [Header("表示設定")]
    [SerializeField] private TMP_Text timeText; // 左側に出す時間表示

    public static float currentTime;
    private bool isTimeUp = false;

    [SerializeField] private GameObject noise;

    public bool IsTimeUp => isTimeUp;

    private void Start()
    {
        currentTime = timeLimitSeconds;
        UpdateTimeText();
        Time.timeScale = 1;
        ResultScoreInput.gameOn = true;//ゲーム開始の宣言
    }

    private void Update()
    {
        if (isTimeUp)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isTimeUp = true;

            // 最後に 00:00 を一瞬反映したいならここで更新
            UpdateTimeText();

            // 時間表示を消す
            if (timeText != null)
            {
                timeText.gameObject.SetActive(false);
            }

            FinishNoiseFalse();
            Time.timeScale = 0;
            return;
        }

        UpdateTimeText();
    }

    private void FinishNoiseFalse()
    {
        if (noise == null) return;

        //ノイズ削除
        if (currentTime > 0 && noise.activeSelf == false)
        {
            noise.SetActive(true);
        }
        else if (currentTime <= 0 && noise.activeSelf == true)
        {
            noise.SetActive(false);
        }
    }

    private void UpdateTimeText()
    {
        if (timeText == null)
            return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}