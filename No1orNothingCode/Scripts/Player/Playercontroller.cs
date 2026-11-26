using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerKnockBack playerKnockBack;

    [Header(">>>触るな<<<")]
    public float axisH;
    public float axisV;

    private UnityEngine.InputSystem.PlayerInput playerInput;
    private InputAction move;
    private InputAction jump;
    private InputAction attack;
    private BlueIdolAnimation characterAnimation;

    private void Awake()
    {
        playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();

        move = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];
        attack = playerInput.actions["Attack"];
        characterAnimation = GetComponent<BlueIdolAnimation>();
    }

    void Update()
    {
        if (playerKnockBack.stunTimer <= 0.0f)
        {
            axisH = move.ReadValue<Vector2>().x;
            axisV = move.ReadValue<Vector2>().y;

            if (jump.WasPressedThisFrame() && playerState.onGround)
            {
                playerJump.Jump();
                characterAnimation?.PlayJump();
            }

            if (attack.WasPressedThisFrame() && playerAttack.TryAttack())
            {
                characterAnimation?.PlayPunch();
            }
        }
    }
}
