using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    Rigidbody2D rb;

    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("降下ができない範囲")]
    public float groundDistance;

    [Header(">>>触るな<<<")]
    public bool onGround = false;
    public bool onGroundWide = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        int layerMask = groundLayer | playerLayer;

        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), layerMask);   //地面判定の距離
        onGroundWide = Physics2D.Linecast(transform.position, transform.position - (transform.up * groundDistance), layerMask);   //地面判定の距離
    }
}
