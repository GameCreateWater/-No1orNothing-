using UnityEngine;
using UnityEngine.UI;

public class ChangeHoverImage : MonoBehaviour
{
    public Color[] playerCursorColor;
    public GameObject[] playerImage;
    public GameObject[] playerBackImage;
    public GameObject[] joinText;
    public Text[] nameText;
    public bool onTrigger = false;
    public bool[] playerIn;

    public GameObject characterSlots;

    private string[] characterNames;
    private bool[] playerReady;

    private static readonly Color InactiveColor = new Color(
        100f / 255f,
        100f / 255f,
        100f / 255f);

    public void Start()
    {
        //プレイヤー接続判定
        int slotCount = playerBackImage.Length;
        playerIn = new bool[slotCount];
        playerReady = new bool[slotCount];
        characterNames = new string[slotCount];

        GameManager.Instance.HoverCharacterManager = this;
        for (int i = 0; i < slotCount; i++)
        {
            SetPlayerJoined(i, false);
        }
    }

    public void BackImage(int index)
    {
        if (!IsValidPlayer(index))
            return;

        SetPlayerJoined(index, !playerIn[index]);
    }

    public void SetPlayerJoined(int index, bool joined)
    {
        if (!IsValidPlayer(index))
            return;

        playerIn[index] = joined;
        playerReady[index] = false;
        characterNames[index] = string.Empty;

        SpriteRenderer background = playerBackImage[index].GetComponent<SpriteRenderer>();
        background.color = joined && index < playerCursorColor.Length
            ? playerCursorColor[index]
            : InactiveColor;

        Text joinLabel = joinText[index].GetComponent<Text>();
        if (joinLabel != null)
            joinLabel.text = "PRESS A";
        joinText[index].SetActive(!joined);

        if (!joined)
        {
            SpriteRenderer image = playerImage[index].GetComponent<SpriteRenderer>();
            image.sprite = null;
            image.color = new Color(1f, 1f, 1f, 0f);
        }

        RefreshPlayerLabel(index);
    }

    public void HoverImage(Sprite characterImage, int playernumber,string name)
    {
        if (!IsValidPlayer(playernumber))
            return;

        //キャラクタースロットからスプライトを持ってくる
        playerImage[playernumber].GetComponent<SpriteRenderer>().sprite = characterImage;
        playerImage[playernumber].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        characterNames[playernumber] = name;
        RefreshPlayerLabel(playernumber);
    }

    public void HoverImagenull(int playernumber)
    {
        if (!IsValidPlayer(playernumber))
            return;

        playerImage[playernumber].GetComponent<SpriteRenderer>().sprite = null;
        playerImage[playernumber].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        characterNames[playernumber] = string.Empty;
        RefreshPlayerLabel(playernumber);
    }

    public void SetPlayerReady(int playernumber, bool ready)
    {
        if (!IsValidPlayer(playernumber))
            return;

        playerReady[playernumber] = ready;
        RefreshPlayerLabel(playernumber);
    }

    public void SlotFalse()
    {
        characterSlots.SetActive(false);
    }

    public void SlotTrue()
    {
        characterSlots.SetActive(true);
    }

    private void RefreshPlayerLabel(int index)
    {
        if (index >= nameText.Length || nameText[index] == null)
            return;

        // The selected character name is printed on the strip below each player's cheki.
        // Ready state is shown by the existing selection effect, not by extra text.
        nameText[index].text = playerIn[index] && !string.IsNullOrEmpty(characterNames[index])
            ? characterNames[index]
            : string.Empty;
    }

    private bool IsValidPlayer(int index)
    {
        return index >= 0 && index < playerIn.Length &&
               index < playerImage.Length &&
               index < playerBackImage.Length &&
               index < joinText.Length;
    }
}
