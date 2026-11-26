using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("イベント発生間隔")]
    [SerializeField] float[] eventTimeAction;
    [Header("発生イベント指定")]
    [SerializeField] int[] eventNumberOn;
    [Header("イベント起こすオブジェクト格納")]
    public GameObject[] eventObject;
    int eventActionNumber=0;
    float eventTime;
    float nowTime = 0;
    GameObject obj;

    //スポットライト
    public GameObject spotLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        EventTimeDecision();
        StartCoroutine(TimeGard());
    }

    public void EventTimeDecision()
    {
        //Debug.Log("Timedecitsion");
        //new WaitForSeconds(0.1f);
        nowTime = 0;
        eventTime = eventTimeAction[eventActionNumber];
    }

    public void EventAction()
    {
        //Debug.Log("aaa");
        if (eventActionNumber >0)
        {
            if (eventNumberOn[eventActionNumber] == eventNumberOn[eventActionNumber - 1])
            {
                Destroy(obj);
                EventFalse();
            }
        }
        obj = Instantiate(eventObject[eventNumberOn[eventActionNumber]], this.transform);
        //obj = Instantiate(eventObject[number], this.transform);
        nowTime = 0;
        EventTimeDecision();
        eventActionNumber++;
    }

    public void SpotLightOff()
    {
        spotLight.SetActive(false);
    }


    public void EventFalse()
    {
        //EventTimeDecision();
        spotLight.SetActive(true);
        spotLight.GetComponent<TargetCreatetManager>().TaragetDestroy();
        spotLight.GetComponent<TargetCreatetManager>().moveObject.GetComponent<SpotLightScore>().player = new GameObject[4];
        spotLight.GetComponent<TargetCreatetManager>().moveObject.GetComponent<SpotLightScore>().Reset();
        spotLight.GetComponent<TargetCreatetManager>().moveObject.GetComponent<SpotLightMove>().Reset();
        spotLight.GetComponent<TargetCreatetManager>().moveObject.GetComponent<SpotLightTriangle>().mesh.Clear();
    }

    private IEnumerator TimeGard()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            nowTime += 0.1f;
            if (nowTime - eventTime < 1 && nowTime - eventTime > 0 && eventActionNumber < eventNumberOn.Length -1 )
            {
                nowTime = 0;
               // int eventNumber = Random.Range(0,eventObject.Length);
                EventAction();
            }
        }

    }

}
