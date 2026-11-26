using UnityEngine;

public class SelectCharaAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 myPosition;
    Rigidbody2D rb;
    public float moveSpeed;

    private SpriteRenderer sr;
    private BoxCollider2D col;

    public bool set = false;
    private GameObject obj;
    [SerializeField]private GameObject effect;
    private void Awake()
    {
        myPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        sr.color = new Color(1,1,1,0);
        sr.size = col.size;
    }

    private void FixedUpdate()
    {
        if (!Mathf.Approximately(transform.position.x, myPosition.x))
        {
            rb.linearVelocityX = (myPosition.x - transform.position.x) * moveSpeed;
        }
        else if (rb.linearVelocityX > 0)
        {
            rb.linearVelocityX = 0;
        }

        if (set && obj == null)
        {
            obj = Instantiate(effect, this.transform);
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = new Vector3(2, 2, 2);
            foreach (SpriteRenderer effectRenderer in obj.GetComponentsInChildren<SpriteRenderer>(true))
            {
                effectRenderer.sortingOrder = Mathf.Max(effectRenderer.sortingOrder, sr.sortingOrder + 10);
            }
        }
        else if (set == false && obj != null)
        {
            Destroy(obj);
            obj = null;
        }
    }

    public void SelectSlot()
    {
        if (Mathf.Approximately(transform.position.x, myPosition.x))
        {
            float dir = transform.position.x < 0 ? 1 : -1;
            transform.position = new Vector2(myPosition.x + 3 * dir, myPosition.y);
        }
    }
}
