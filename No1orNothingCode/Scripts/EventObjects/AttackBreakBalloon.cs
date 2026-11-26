using Unity.VisualScripting;
using UnityEngine;

public class AttackBreakBalloon : MonoBehaviour
{
    [SerializeField] private bool pointUp;
    [SerializeField] private float scoreTime;
    [SerializeField] private float scoreUp;
    [SerializeField] private GameObject ballonEffect;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("AttackArea"))
        {
            if (pointUp)
            {
                other.transform.parent.GetComponent<PlayerScoreManager>().ScoreUpChanges(scoreUp, scoreTime, ballonEffect);
                pointUp = false;
            }
                Destroy(gameObject);
        }
    }
}
