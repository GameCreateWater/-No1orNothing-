using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public int characterID;
    public int HoverImageID;
    public string _name;
    public GameObject HoverCharacterImages;

    public void Select(int playerIndex)
    {
        GameManager.Instance.SetCharacter(playerIndex, characterID);
        Debug.Log("Player " + playerIndex + " selected " + characterID);
    }

    public void Deselect(int playerIndex)
    {
        GameManager.Instance.SetCharacter(playerIndex, -1); // 未選択状態
        Debug.Log("Player " + playerIndex + " canceled selection");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Enter : " + other.name);

        CursorController cursor = other.GetComponent<CursorController>();

        if (cursor != null)
        {
            if (cursor.currentSlot != this && !cursor.moveStop)
            {
                cursor.currentSlot = this;
                Debug.Log("currentSlot set : " + characterID);
                if (cursor.selectSlot == null)
                {
                    HoverCharacterImages.GetComponent<ChangeHoverImage>().playerImage[other.GetComponent<PlayerInput>().playerIndex].GetComponent<SelectCharaAnimation>().SelectSlot();
                    HoverCharacterImages.GetComponent<ChangeHoverImage>().HoverImage(GetComponent<SpriteRenderer>().sprite, other.gameObject.GetComponent<PlayerInput>().playerIndex,_name);
                }
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit : " + other.name);

        CursorController cursor = other.GetComponent<CursorController>();
        if (cursor != null)
        {
            //自身に入ってるときのみ消す
            if (cursor.currentSlot == this)
            {
                cursor.currentSlot = null;

                if (cursor.selectSlot == null)
                {
                    HoverCharacterImages.GetComponent<ChangeHoverImage>().HoverImagenull(other.gameObject.GetComponent<PlayerInput>().playerIndex);
                }
            }
        } 
    }
}