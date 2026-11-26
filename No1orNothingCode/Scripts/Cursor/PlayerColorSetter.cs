using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerColorSetter : MonoBehaviour
{
    public Sprite[] playerColors; // インスペクタで設定

    private PlayerInput playerInput;
    private SpriteRenderer sprite;

   SpriteRenderer sr;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        sprite = GetComponent<SpriteRenderer>();

        sr = GetComponentInChildren<SpriteRenderer>();

        ApplyColor();
    }

    void ApplyColor()
    {
        int index = playerInput.playerIndex;

        if (index < playerColors.Length)
        {
            sprite.sprite = playerColors[index];
        }
    }
}
