using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(-10000)]
public class LoadAnimation : MonoBehaviour
{
    public static bool IsLoading { get; private set; }
    private readonly HashSet<PlayerInput> blockedPlayers = new HashSet<PlayerInput>();
    private readonly HashSet<BaseInputModule> blockedUI = new HashSet<BaseInputModule>();
    private readonly HashSet<PlayerInputManager> blockedJoining = new HashSet<PlayerInputManager>();

    private void Awake()
    {
        IsLoading = true;
        SceneManager.sceneLoaded += BlockLoadedScene;
        BlockInput();
    }

    private void BlockLoadedScene(Scene scene, LoadSceneMode mode) => BlockInput();
    private void Update() => BlockInput();
    private void LateUpdate() => BlockInput();

    private void BlockInput()
    {
        if (!IsLoading) return;
        foreach (var input in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
        {
            if (!input.isActiveAndEnabled || !input.inputIsActive) continue;
            blockedPlayers.Add(input);
            input.DeactivateInput();
        }
        foreach (var module in FindObjectsByType<BaseInputModule>(FindObjectsSortMode.None))
        {
            if (!module.enabled) continue;
            blockedUI.Add(module);
            module.enabled = false;
        }
        foreach (var manager in FindObjectsByType<PlayerInputManager>(FindObjectsSortMode.None))
        {
            if (!manager.joiningEnabled) continue;
            blockedJoining.Add(manager);
            manager.DisableJoining();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= BlockLoadedScene;
        IsLoading = false;
        foreach (var input in blockedPlayers)
        {
            if (input == null || !input.isActiveAndEnabled) continue;
            bool heldForCountdown = false;
            foreach (var presentation in FindObjectsByType<GameStartPresentation>(FindObjectsSortMode.None))
            {
                if (!GameStartPresentation.IsCountdownActive || presentation.gameObject.scene != input.gameObject.scene) continue;
                presentation.HoldInputUntilCountdownEnds(input);
                heldForCountdown = true;
                break;
            }
            if (!heldForCountdown) input.ActivateInput();
        }
        foreach (var module in blockedUI) if (module != null) module.enabled = true;
        foreach (var manager in blockedJoining) if (manager != null) manager.EnableJoining();
    }

    [SerializeField] private Slider loadSlider;
    [SerializeField] private GameObject image;
    public GameObject endPosition;
    [SerializeField] private float chageTime = 1f;
    [SerializeField] private float nowTime;

    private IEnumerator Start()
    {
        string destination;
        Scene previous;
        if (!string.IsNullOrEmpty(SceneGate._newSceneName))
        {
            destination = SceneGate._newSceneName;
            previous = SceneGate._nowScene;
            SceneGate._newSceneName = null;
        }
        else
        {
            destination = SceneGateSelect._newSceneName;
            previous = SceneGateSelect._nowScene;
            SceneGateSelect._newScene = null;
        }

        if (string.IsNullOrEmpty(destination) || !Application.CanStreamedLevelBeLoaded(destination))
        {
            Debug.LogError("LoadScene: destination is missing or not in the build scene list.");
            yield break;
        }

        Scene loadingScene = gameObject.scene;
        Vector3 closedPosition = image != null ? image.transform.position : Vector3.zero;
        if (loadSlider != null)
        {
            loadSlider.maxValue = 1f;
            loadSlider.value = 0f;
        }

        Time.timeScale = 1f;
        // Finish removing the old PlayerInputs before activating the next scene.
        if (previous.IsValid() && previous.isLoaded && previous != loadingScene)
            yield return SceneManager.UnloadSceneAsync(previous);

        var load = SceneManager.LoadSceneAsync(destination, LoadSceneMode.Additive);
        if (load == null) yield break;
        load.allowSceneActivation = false;
        while (load.progress < 0.9f)
        {
            if (loadSlider != null) loadSlider.value = Mathf.Clamp01(load.progress / 0.9f);
            yield return null;
        }

        // Keep the shutter CLOSED until the destination's cameras and Start methods are ready.
        load.allowSceneActivation = true;
        yield return load;
        Scene next = SceneManager.GetSceneByName(destination);
        SceneManager.SetActiveScene(next);
        yield return null;
        yield return null;

        // LoadScene's canvas is Screen Space Overlay; its blue-clear camera is no longer needed.
        foreach (var root in loadingScene.GetRootGameObjects())
            foreach (var camera in root.GetComponentsInChildren<Camera>(true))
                camera.enabled = false;

        if (loadSlider != null) loadSlider.gameObject.SetActive(false);
        nowTime = 0f;
        float duration = Mathf.Max(0.01f, chageTime);
        while (image != null && endPosition != null && nowTime < duration)
        {
            // Gameplay countdown pauses scaled time while LoadScene exists.
            nowTime += Time.unscaledDeltaTime;
            image.transform.position = Vector3.Lerp(closedPosition,
                endPosition.transform.position, Mathf.Clamp01(nowTime / duration));
            yield return null;
        }

        SceneGate.sceneOn = true;
        SceneManager.UnloadSceneAsync(loadingScene);
    }
}
