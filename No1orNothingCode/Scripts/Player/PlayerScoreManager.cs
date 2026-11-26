using System.Collections;
using System.Collections.Generic;
using NUnit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScoreManager : MonoBehaviour
{
    public float score = 0;
    public Text text;
    public uint DeductionVersion { get; private set; }

    private float scoreUp = 1;
    public float scoreChange = 0;
    public float scoreChangeTimes = 0;
    private float nowtime=0;

    GameObject effectPrefab;

    PlayerInput input;

    // Update is called once per frame
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        if (RoundManager.roundCount <= 1) return;
        score = PlayerManager.ScoreTable[(input.playerIndex, RoundManager.roundCount - 2)];
    }
    void Update()
    {
        if (text == null) return;

        if (score < 0) score = 0;
        text.text = Mathf.FloorToInt(score).ToString("N0");

        nowtime += Time.deltaTime;
        if(nowtime > 1 && scoreChangeTimes > -1)
        {
            nowtime = 0;
            scoreChangeTimes--;
            if (scoreChangeTimes <= 0 && scoreChange > 0) { ScoreUpRemove(); }
        }


    }

    public void ScoreUpChanges(float newScoreUp, float newTime, GameObject effect)
    {
        if(scoreChange < newScoreUp)
        {
            scoreChange = newScoreUp;
            effectPrefab = Instantiate(effect,transform);
            effectPrefab.transform.localPosition = new Vector3(0, 0.5f, -2);

        }
        if(scoreChangeTimes < newTime)
        {
            scoreChangeTimes = newTime;
        }
    }

    public void ScoreUpRemove()
    {
        scoreChange = 0;
        Destroy(effectPrefab);
    }

    public void ScoreChange(float change)
    {
        float delta = change * (scoreUp + scoreChange) *
            RoundManager.ScoreMultiplier;
        if (delta < 0f && score > 0f)
            unchecked { DeductionVersion++; }
        score += delta;

    }

    public float FinalizeRoundScore()
    {
        score = Mathf.Max(0f, Mathf.Floor(score));

        if (text != null)
        {
            text.text = Mathf.FloorToInt(score).ToString("N0");
        }

        return score;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            ScoreChange(3);
        }
    }



}
