using UnityEngine;

public class NoiseFalse : MonoBehaviour
{
    public GameObject noise;
    void Update()
    {
        if (TimeLimitManager.currentTime > 0 && noise.activeSelf == false)
        {
            noise.SetActive(true);
        }
        else if(TimeLimitManager.currentTime <= 0 && noise.activeSelf == true)
        {
            noise.SetActive(false);
        }
    }
}
