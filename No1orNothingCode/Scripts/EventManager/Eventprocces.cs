using UnityEngine;

public class Eventprocces : MonoBehaviour
{
    public bool spotLightOff;

    float StartTime = 0;
    public float StopTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(spotLightOff == true)
        {
            transform.parent.GetComponent<EventManager>().SpotLightOff();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StartTime >= StopTime)
        {
            if (spotLightOff == true)
            {
                transform.parent.GetComponent<EventManager>().EventFalse();
            }
            Destroy(gameObject);
        }

        StartTime += Time.deltaTime; 
        
    }
}
