using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerKnockBack playerKnockBack;

    Rigidbody2D rb;

    [Header("moveSpeedは『目標速度』")]
    public float moveSpeed;

    [Header("downSpeedは「急降下速度」")]
    [SerializeField] private float downSpeed;
    [SerializeField] private float downSpeedLimit;

    [Header("入力なし時の減衰（Forceでブレーキ）")]
    public float groundBrakeForce;
    public float airBrakeForce;

    [Header(">>>触るな<<<")]
    public float input;
    public float inputV;

    //private bool right = false;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        input = playerController.axisH;
        inputV = playerController.axisV;

        // 入力のデッドゾーン
        bool hasInput = Mathf.Abs(input) > 0.2f;
        bool hasInputV = inputV < -0.5f;

        //左右反転
        if (hasInput)
        {
            if (input > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (hasInput)
        {
            float targetVx = (input > 0.0f) ? moveSpeed : -moveSpeed; // 目標速度 右なら +moveSpeed、左なら -moveSpeed

            float currentVx = rb.linearVelocity.x;
            float dv = targetVx - currentVx;        // 欲しい速度変化量
            float impulseX = rb.mass * dv;          // Impulse = mass * Δv

            rb.AddForce(new Vector2(impulseX, 0f), ForceMode2D.Impulse);
        }

        if (hasInputV && !playerState.onGroundWide)
        {
            float currentVy = rb.linearVelocity.y;
            if (rb.linearVelocity.y < -downSpeedLimit) return;
            if (currentVy > 0.0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                return;
            }
            float dv = currentVy * downSpeed;        // 欲しい速度変化量
            float impulseY = rb.mass * -dv;          // Impulse = mass * Δv

            rb.AddForce(new Vector2(0f, -impulseY), ForceMode2D.Impulse);
        }


        if ((input < 0.2f) && (input > -0.2f)) //入力がないときに減衰
        {
            float bf = playerState.onGround ? groundBrakeForce : airBrakeForce;
            rb.AddForce(Vector2.right * (-rb.linearVelocity.x) * bf, ForceMode2D.Force);
        }
    }

    public void BallStun(PlayerInput playerInput, float stunTime)
    {
        StartCoroutine(StunPlayer(playerInput, stunTime));
    }

    IEnumerator StunPlayer(PlayerInput playerInput, float stunTime)
    {
        playerInput.actions.Disable();

        yield return new WaitForSeconds(stunTime);

        playerInput.actions.Enable();
    }
}
