using Unity.VisualScripting;
using UnityEngine;

public class BreakBalloonScript : MonoBehaviour
{
    [SerializeField] private bool pointUp;
    [SerializeField] private float scoreTime;
    [SerializeField] private float scoreUp;
    [SerializeField] private GameObject ballonEffect;
    // private void OnCollisionEnter2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pointUp)
            {
                pointUp = false;
                other.GetComponent<PlayerScoreManager>().ScoreUpChanges(scoreUp, scoreTime, ballonEffect);
            }
            Destroy(gameObject); 
        }
    


        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Ground")
        {
            Destroy(gameObject);
        }

       
    }
}
