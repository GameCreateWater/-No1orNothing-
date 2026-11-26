using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerKnockBack : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerController playerController;

    private Rigidbody2D rb;

    [Header("殴られて止まる時間")]
    public float knockBackStunTime;

    [Header("同じプレイヤーから食らわない時間")]
    public float hitCoolTime = 1.0f;
    
    private Dictionary<Transform, float> hitCooldown = new Dictionary<Transform, float>();


    [Header(">>>触るな<<<")]
    public float stunTimer = 0.0f;
    public static Action<Transform> OnAttack;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(stunTimer > 0.0f)
        {
            stunTimer -= Time.deltaTime;
        }
    }
    
    public bool CanHit(Transform attacker)  // 攻撃を食らえるかチェック
    {
        if (!hitCooldown.ContainsKey(attacker)) return true;

        return Time.time >= hitCooldown[attacker];
    }

    public void SetCooldown(Transform attacker)
    {
        hitCooldown[attacker] = Time.time + hitCoolTime;
    }

    public void Knockback(Transform attackerRoot, float power, float upward )// attackerRoot：殴った側の「プレイヤー本体（root）」を渡す
    {
        stunTimer = knockBackStunTime;
        playerMove.input = 0.0f;
        playerController.axisH = 0.0f;
        playerController.axisV = 0.0f;
        rb.linearVelocity = Vector2.zero;
        OnAttack?.Invoke(attackerRoot);
        rb.AddForce(new Vector2(power, upward) * attackerRoot.right, ForceMode2D.Impulse);

    }
}
