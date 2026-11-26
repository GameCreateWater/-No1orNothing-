using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject[] player = new GameObject[4];

    public static Dictionary<(int, float), float> ScoreTable
= new Dictionary<(int, float), float>();

    public static void GameReset()
    {
        //Debug.Log("リセット");

        player = new GameObject[4];
        ScoreTable.Clear();
        Time.timeScale = 1;
    }
}
