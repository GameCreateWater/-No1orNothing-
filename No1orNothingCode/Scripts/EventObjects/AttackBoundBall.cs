using UnityEngine;

public class AttackBoundBall : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float _power;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        // AttackAreaに触れたときだけ反応させたいならTag判定
        if (other.CompareTag("AttackArea"))
        {
            // AttackArea自身 or その親から PlayerState を探す
            PlayerState playerState = other.GetComponentInParent<PlayerState>();
            if (playerState == null)
            {
                return;
            }

            float isforward = transform.position.x - other.transform.position.x;

            if (isforward > 0)
            {
                rb.AddForce(new Vector2(_power, 0f), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-_power, 0f), ForceMode2D.Impulse);
            }
        }
    }
}