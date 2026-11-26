using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public int maxRoundSet;
    [SerializeField] private GameObject result;
    public static int roundCount;
    public static int maxRound;
    public static GameObject Instance;

    public static float ScoreMultiplier
    {
        get
        {
            if (roundCount >= 3) return 2f;
            if (roundCount == 2) return 1.5f;
            return 1f;
        }
    }

    [Header("Seconds before advancing from the round result")]
    [SerializeField] private float time = 5f;
    [SerializeField] private GameObject newRoundButton;
    [SerializeField] private GameObject[] playerGoIcon;
    public static bool roundNot;

    // Existing scenes serialize these legacy references, so keep them intact.
    public SceneGateSelect select;
    public SceneGateTrigger i;
    public CursorController e;
    [SerializeField] private SceneGateSelect _selectScript;

    private bool roundResultActive;
    private GameObject autoAdvanceCanvas;
    private TMP_Text autoAdvanceText;

    private void Start()
    {
        maxRound = maxRoundSet;
        if (Instance == null)
        {
            Instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
            Instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        SetReadyObjectsVisible(false);
    }

    public void OnResult()
    {
        if (roundResultActive) return;
        roundResultActive = true;
        SetReadyObjectsVisible(false);
        CreateAutoAdvanceDisplay();
        StartCoroutine(NextStage());
    }

    public void NewRound(string selectScene)
    {
        roundResultActive = false;
        RemoveAutoAdvanceDisplay();
        result.SetActive(false);
        SetReadyObjectsVisible(false);

        ResultScore scoreResult = result.transform.parent.GetComponent<ResultScore>();
        for (int index = 0; index < scoreResult.line.Length; index++)
            scoreResult.line[index].GetComponent<ScoreLineCreate>().on = true;

        bool hasNextStage = false;
        if (selectScene == null)
        {
            if (!roundNot) roundCount++;
            hasNextStage = _selectScript.GetSceneCount() > 0;
        }

        Time.timeScale = 1f;
        if (roundCount <= maxRound && hasNextStage)
            _selectScript.GoNextScene(null);
        else
            _selectScript.GoNextScene("ScoreResult");
    }

    public void RoundCountSet(int maxRoundSets) => maxRound = maxRoundSets;

    private IEnumerator NextStage()
    {
        float remaining = Mathf.Max(0.1f, time);
        while (remaining > 0f && result.activeInHierarchy)
        {
            bool finalRound = roundCount >= maxRound;
            if (autoAdvanceText != null)
                autoAdvanceText.text = (finalRound ? "FINAL RESULT IN  " : "NEXT STAGE IN  ")
                    + Mathf.CeilToInt(remaining);
            remaining -= Time.unscaledDeltaTime;
            yield return null;
        }
        if (result.activeInHierarchy) NewRound(null);
    }

    private void SetReadyObjectsVisible(bool visible)
    {
        if (newRoundButton != null) newRoundButton.SetActive(visible);
        if (playerGoIcon == null) return;
        foreach (GameObject icon in playerGoIcon)
            if (icon != null) icon.SetActive(visible);
    }

    private void CreateAutoAdvanceDisplay()
    {
        RemoveAutoAdvanceDisplay();
        autoAdvanceCanvas = new GameObject("ResultAutoAdvanceCanvas", typeof(Canvas), typeof(CanvasScaler));
        DontDestroyOnLoad(autoAdvanceCanvas);
        Canvas canvas = autoAdvanceCanvas.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 4500;

        CanvasScaler scaler = autoAdvanceCanvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject panelObject = new GameObject(
            "AutoAdvancePanel", typeof(RectTransform), typeof(Image), typeof(Outline));
        panelObject.transform.SetParent(autoAdvanceCanvas.transform, false);
        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.31f, 0.045f);
        panelRect.anchorMax = new Vector2(0.69f, 0.15f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage = panelObject.GetComponent<Image>();
        panelImage.color = new Color(0.035f, 0.025f, 0.13f, 0.88f);
        panelImage.raycastTarget = false;

        Outline panelOutline = panelObject.GetComponent<Outline>();
        panelOutline.effectColor = new Color(1f, 0.43f, 0.82f, 0.95f);
        panelOutline.effectDistance = new Vector2(4f, -4f);
        panelOutline.useGraphicAlpha = false;

        GameObject textObject = new GameObject("AutoAdvanceCountdown", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(panelObject.transform, false);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(24f, 8f);
        rect.offsetMax = new Vector2(-24f, -8f);

        autoAdvanceText = textObject.GetComponent<TextMeshProUGUI>();
        autoAdvanceText.alignment = TextAlignmentOptions.Center;
        autoAdvanceText.fontSize = 46f;
        autoAdvanceText.fontStyle = FontStyles.Bold;
        autoAdvanceText.color = new Color(1f, 0.86f, 0.28f, 1f);
        autoAdvanceText.outlineColor = new Color32(30, 12, 66, 255);
        autoAdvanceText.outlineWidth = 0.18f;
        autoAdvanceText.raycastTarget = false;
    }

    private void RemoveAutoAdvanceDisplay()
    {
        if (autoAdvanceCanvas != null) Destroy(autoAdvanceCanvas);
        autoAdvanceCanvas = null;
        autoAdvanceText = null;
    }

    private void OnDestroy() => RemoveAutoAdvanceDisplay();
}
