using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    private const string GamepadControlScheme = "Gamepad";
    private const string GameplayActionMap = "Player";

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private PlayerInput[] characterPrefabs;

    [SerializeField] private Text[] scoreText;
    [Header("Gameplay effects (original assets stay unchanged)")]
    [SerializeField] private GameObject runningSmokePrefab;
    [SerializeField] private GameObject punchImpactPrefab;

    [SerializeField] private GameObject gameManager;

    [SerializeField] private Color[] colors;
 

    private void Start()
    {
        UpdateScoreboardVisibility();
        //SpawnPlayers();
        StartCoroutine(OnSpawnPlayer());
    }

    private void SpawnPlayers()
    {
        UpdateScoreboardVisibility();
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance が null");
            return;
        }

        var players = GameManager.Instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            var data = players[i];
            int playerSlot = data.playerIndex;

            if (data.selectedCharacter < 0 || data.selectedCharacter >= characterPrefabs.Length)
            {
                Debug.LogError("selectedCharacter が不正です : " + data.selectedCharacter);
                continue;
            }

            if (characterPrefabs[data.selectedCharacter] == null)
            {
                Debug.LogError("characterPrefabs[" + data.selectedCharacter + "] が null");
                continue;
            }

            if (data.device == null)
            {
                Debug.LogError("device が null");
                continue;
            }

            if (playerSlot < 0 || playerSlot >= spawnPoints.Length || spawnPoints[playerSlot] == null)
            {
                Debug.LogError("spawnPoints[" + playerSlot + "] が未設定");
                continue;
            }

            // The selection-scene PlayerInput is destroyed during the scene change.
            // Explicitly restore both the saved gamepad and the gameplay action map;
            // relying on automatic scheme selection can leave later players with no
            // usable action map even though their character was instantiated.
            PlayerInput player = PlayerInput.Instantiate(
                characterPrefabs[data.selectedCharacter].gameObject,
                playerIndex: data.playerIndex,
                controlScheme: GamepadControlScheme,
                splitScreenIndex: -1,
                pairWithDevice: data.device);

            player.SwitchCurrentControlScheme(GamepadControlScheme, data.device);
            player.ActivateInput();
            player.SwitchCurrentActionMap(GameplayActionMap);
            data.playerInput = player;

            bool pairedWithSavedDevice = false;
            foreach (InputDevice pairedDevice in player.devices)
            {
                if (pairedDevice == data.device)
                {
                    pairedWithSavedDevice = true;
                    break;
                }
            }

            if (!pairedWithSavedDevice)
            {
                Debug.LogError(
                    $"Player {data.playerIndex} のコントローラー再接続に失敗しました: " +
                    $"{data.device.displayName} (deviceId={data.device.deviceId})");
            }

            PlayerManager.player[playerSlot] = player.gameObject;
            Debug.Log(
                $"Spawned Player {data.playerIndex}: {player.gameObject.name}, " +
                $"device={data.device.displayName} (deviceId={data.device.deviceId}), " +
                $"scheme={player.currentControlScheme}, map={player.currentActionMap?.name}");
            
            player.GetComponent<PlayerScoreManager>().text = scoreText[playerSlot];
            player.transform.position = spawnPoints[playerSlot].position;
            if (runningSmokePrefab != null || punchImpactPrefab != null)
            {
                var effects = player.GetComponent<PlayerGameplayEffects>();
                if (effects == null) effects = player.gameObject.AddComponent<PlayerGameplayEffects>();
                effects.Configure(runningSmokePrefab, punchImpactPrefab);
            }
        }
    }

    private void UpdateScoreboardVisibility()
    {
        if (scoreText == null) return;
        for (int slot = 0; slot < scoreText.Length; slot++)
        {
            if (scoreText[slot] == null) continue;
            bool joined = GameManager.Instance != null &&
                GameManager.Instance.players.Exists(data => data.playerIndex == slot);
            // Each scene's score text is a direct child of its individual board.
            Transform board = scoreText[slot].transform.parent;
            if (board != null && board.name.StartsWith("ScoreBoard"))
                board.gameObject.SetActive(joined);
        }
    }

    private IEnumerator OnSpawnPlayer()
    {
        yield return new WaitUntil(() =>SceneManager.GetActiveScene().name == SceneGateSelect._newSceneName);//今Activeのシーンは移動後シーンか
        // 旧シーンのPlayerInputが残っている間に同じplayerIndexを生成すると、
        // 1Pの生成失敗や後続プレイヤーの割り当てずれが起きる。
        yield return new WaitUntil(() =>
            !SceneGateSelect._nowScene.IsValid() || !SceneGateSelect._nowScene.isLoaded);
        SpawnPlayers();

    }
}
