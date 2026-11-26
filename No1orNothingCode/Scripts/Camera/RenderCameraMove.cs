using UnityEngine;

public class RenderCameraMove : MonoBehaviour
{
    private const float TargetHoldSeconds = 1.25f;
    private const float TargetSwitchLead = 3f;
    private const float FollowSmoothSeconds = 0.18f;

    private static int evaluatedFrame = -1;
    private static int activeSceneHandle = int.MinValue;
    private static GameObject sharedTargetPlayer;
    private static float lastTargetSwitchTime;
    private static Vector2 sharedFocusPosition;
    private static Vector2 sharedFocusVelocity;
    private static bool hasSharedFocusPosition;

    [Header("プレイヤーを中心としたカメラ位置")]
    public Vector2 offset;

    [SerializeField] float distance;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSharedTracking()
    {
        evaluatedFrame = -1;
        activeSceneHandle = int.MinValue;
        sharedTargetPlayer = null;
        lastTargetSwitchTime = float.NegativeInfinity;
        sharedFocusPosition = Vector2.zero;
        sharedFocusVelocity = Vector2.zero;
        hasSharedFocusPosition = false;
    }

    private void LateUpdate()
    {
        UpdateSharedTracking(gameObject.scene.handle);

        if (sharedTargetPlayer == null)
            return;

        float x = Mathf.Clamp(
            sharedFocusPosition.x + offset.x,
            minX + distance + offset.x,
            maxX - distance + offset.x);
        float y = Mathf.Clamp(
            sharedFocusPosition.y + offset.y,
            minY + distance + offset.y,
            maxY - distance + offset.y);
        transform.position = new Vector3(x, y, -10f);

    }

    private static void UpdateSharedTracking(int sceneHandle)
    {
        if (activeSceneHandle != sceneHandle)
        {
            activeSceneHandle = sceneHandle;
            evaluatedFrame = -1;
            sharedTargetPlayer = null;
            sharedFocusVelocity = Vector2.zero;
            hasSharedFocusPosition = false;
            lastTargetSwitchTime = float.NegativeInfinity;
        }

        if (evaluatedFrame == Time.frameCount)
            return;

        evaluatedFrame = Time.frameCount;
        GameObject bestPlayer = null;
        float bestScore = float.NegativeInfinity;

        for (int i = 0; i < PlayerManager.player.Length; i++)
        {
            GameObject candidate = PlayerManager.player[i];
            if (candidate == null)
                continue;

            PlayerScoreManager scoreManager = candidate.GetComponent<PlayerScoreManager>();
            float candidateScore = scoreManager != null ? scoreManager.score : 0f;
            if (bestPlayer == null || candidateScore > bestScore)
            {
                bestPlayer = candidate;
                bestScore = candidateScore;
            }
        }

        if (bestPlayer == null)
        {
            sharedTargetPlayer = null;
            hasSharedFocusPosition = false;
            return;
        }

        if (!IsCurrentPlayer(sharedTargetPlayer))
        {
            SwitchTarget(bestPlayer);
        }
        else if (bestPlayer != sharedTargetPlayer)
        {
            PlayerScoreManager currentScoreManager = sharedTargetPlayer.GetComponent<PlayerScoreManager>();
            float currentScore = currentScoreManager != null ? currentScoreManager.score : 0f;
            bool holdFinished = Time.unscaledTime - lastTargetSwitchTime >= TargetHoldSeconds;
            bool hasClearLead = bestScore >= currentScore + TargetSwitchLead;

            if (holdFinished && hasClearLead)
                SwitchTarget(bestPlayer);
        }

        Vector2 targetPosition = sharedTargetPlayer.transform.position;
        if (!hasSharedFocusPosition)
        {
            sharedFocusPosition = targetPosition;
            sharedFocusVelocity = Vector2.zero;
            hasSharedFocusPosition = true;
            return;
        }

        sharedFocusPosition = Vector2.SmoothDamp(
            sharedFocusPosition,
            targetPosition,
            ref sharedFocusVelocity,
            FollowSmoothSeconds,
            Mathf.Infinity,
            Time.unscaledDeltaTime);
    }

    private static bool IsCurrentPlayer(GameObject target)
    {
        if (target == null)
            return false;

        for (int i = 0; i < PlayerManager.player.Length; i++)
        {
            if (PlayerManager.player[i] == target)
                return true;
        }

        return false;
    }

    private static void SwitchTarget(GameObject target)
    {
        sharedTargetPlayer = target;
        lastTargetSwitchTime = Time.unscaledTime;
    }
}
