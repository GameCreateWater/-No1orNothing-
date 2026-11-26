using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using NUnit.Framework;
using System.Collections.Generic;


public class SceneGateSelect : MonoBehaviour
{
    [Header("本編のシーン一覧")]
    public string[] gameScene;

    public static AsyncOperation _newScene = null;
    public static string _newSceneName = null;
    public static Scene _nowScene;

    private List<string> _sceneList = new List<string>();
    private void Start()
    {
        foreach (var scene in gameScene)
        {
            _sceneList.Add(scene);
        }
    }

    public void GoNextScene(string selectScene)
    {
        if (LoadAnimation.IsLoading) return;
        Debug.Log("シーン移動");

        if(selectScene != null)
        {
            LoadScene(selectScene);
            return;
        }

        int random = 0;
        //次のステージを決める
        random = Math.Abs(UnityEngine.Random.Range(0, _sceneList.Count-1));
        var _selectScene = _sceneList[random];
        _sceneList.Remove(_selectScene);

        LoadScene(_selectScene);

    }

    private void LoadScene(string nextScene)
    {
        _nowScene = SceneManager.GetActiveScene();

        _newSceneName = nextScene;
        //Debug.Log("シーン移動開始");
        SceneManager.LoadSceneAsync("LoadScene", LoadSceneMode.Additive);//ロードシーンを追加

    }

    public int GetSceneCount()
    {
        return _sceneList.Count;
    }
}
