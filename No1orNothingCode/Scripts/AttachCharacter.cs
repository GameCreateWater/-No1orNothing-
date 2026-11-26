using UnityEngine;

public class AttachCharacter : MonoBehaviour
{
    public GameObject[] characterSlot;

    private void Start()
    {
        GameManager.Instance.charaSlot = new GameObject[characterSlot.Length];
        for(int i = 0; i < GameManager.Instance.charaSlot.Length; i++)
        {
            GameManager.Instance.charaSlot[i] = characterSlot[i];//キャラスロットの割り当て
        }
    }
}
