using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public const int MaxPlayers = 4;
    public const int MinPlayers = 2;

    public static GameManager Instance;

    public GameObject SceneGate;
    public ChangeHoverImage HoverCharacterManager;
    public GameObject[] charaSlot;

    public List<PlayerData> players = new List<PlayerData>();

    public static bool gameIn;//本編シーン？

    public bool CanStartGame => AreAllPlayersSelected();

    private void Awake()
    {
        gameIn = false;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("重複");
            return;
        }
    }

    void Start()
    {
        //遷移ボタンを非表示にする
        if (SceneGate != null)
            SceneGate.SetActive(false);
    }

    public void RegisterPlayer(PlayerInput player)
    {
        if (player == null || player.playerIndex < 0 || player.playerIndex >= MaxPlayers)
        {
            Debug.LogWarning("最大4人まで参加できます");
            if (player != null)
                Destroy(player.gameObject);
            return;
        }

        if (player.devices.Count == 0)
        {
            Debug.LogWarning("device が無いので登録しません");
            Destroy(player.gameObject);
            return;
        }

        var device = player.devices[0];

        if (!(device is Gamepad))
        {
            Debug.LogWarning("Gamepad以外なので登録しません: " + device);
            Destroy(player.gameObject);
            return;
        }

        PlayerData data = players.FirstOrDefault(n => n.playerIndex == player.playerIndex);
        if (data == null)
        {
            if (players.Count >= MaxPlayers)
            {
                Debug.LogWarning("最大4人まで参加できます");
                Destroy(player.gameObject);
                return;
            }

            data = new PlayerData();
            players.Add(data);
        }

        data.playerIndex = player.playerIndex;
        data.device = device;
        data.playerInput = player;
        data.selectedCharacter = -1;
        players.Sort((left, right) => left.playerIndex.CompareTo(right.playerIndex));

        //カーソルに変数割り当て
        CursorController cursor = player.GetComponent<CursorController>();
        if (cursor != null && HoverCharacterManager != null)
        {
            cursor.HoverCharacterImages = HoverCharacterManager.gameObject;
            cursor.charaSelectImage = charaSlot;
        }

        HoverCharacterManager?.SetPlayerJoined(player.playerIndex, true);

        UpdateSceneGate();
    }

    public void SetCharacter(int playerIndex, int characterID)
    {
        PlayerData player = players.FirstOrDefault(data => data.playerIndex == playerIndex);
        if (player == null)
        {
            Debug.LogWarning($"Player {playerIndex + 1} は参加していません");
            return;
        }

        player.selectedCharacter = characterID;
        HoverCharacterManager?.SetPlayerReady(playerIndex, characterID >= 0);

        UpdateSceneGate();
    }

    public void UpdateSceneGate()
    {
        bool canStart = AreAllPlayersSelected();
        if (canStart)
            HoverCharacterManager?.SlotFalse();
        else
            HoverCharacterManager?.SlotTrue();

        if (SceneGate != null)
            SceneGate.SetActive(canStart);
    }

    private bool AreAllPlayersSelected()
    {
        if (players.Count < MinPlayers)
            return false;

        foreach (var player in players)
        {
            if (player.selectedCharacter < 0)
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class PlayerData
{
    public int playerIndex;
    public InputDevice device;
    public int selectedCharacter;
    public PlayerInput playerInput;
}
