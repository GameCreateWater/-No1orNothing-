using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneGate : MonoBehaviour
{
    [SerializeField]private string _selectSceneName;
    public static Scene _nowScene;

    private PlayerInput playerInput;

    private InputAction submit;
    private InputAction cancel;
    public static string _newSceneName = null;
    public static bool sceneOn = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        submit = playerInput.actions["Submit"];
        cancel = playerInput.actions["Cancel"];
    }
    private void Update()
    {
        if (LoadAnimation.IsLoading) return;
        if (submit.WasPressedThisFrame())
        {
            GoNextScene();
        }
    }
    public void GoNextScene()
    {
        LoadScene(_selectSceneName);
    }

    private void LoadScene(string nextScene)
    {
        if (!sceneOn)return;
        sceneOn = false;
        _nowScene = SceneManager.GetActiveScene();

        _newSceneName = nextScene;
        //Debug.Log("シーン移動開始");
        SceneManager.LoadSceneAsync("LoadScene", LoadSceneMode.Additive);

    }
}
