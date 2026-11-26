using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonitorNoiseManager : MonoBehaviour
{
    [SerializeField] private GameObject[] noiseObjects;
    [SerializeField] private float minInterval = 0.3f;
    [SerializeField] private float maxInterval = 1.2f;
    public Image prefab;
    Bounds bounds;

    private void Start()
    {
        StartCoroutine(NoiseLoop());



    }

    private IEnumerator NoiseLoop()
    {
        while (true)


            for (int x = 0; x < noiseObjects.Length; x++)
            {
                GameObject selectedNoise = noiseObjects[x];

                for (int i = 0; i < 10; i++)
                {

                    // モニター内に収まる範囲でランダム
                    bounds = selectedNoise.GetComponent<BoxCollider2D>().bounds;

                    float randomX = Random.Range(bounds.min.x, bounds.max.x);
                    float randomY = Random.Range(bounds.min.y, bounds.max.y);

                    var prefabPos = new Vector2(randomX,randomY);
                    Image obj = Instantiate(prefab, selectedNoise.transform);
                    obj.transform.position = prefabPos;
                    obj.GetComponent<Image>().color = new Color(0, 0, Random.Range(0f, 1f), Random.Range(0.75f, 1f));
                    obj.transform.localScale = new Vector3(Random.Range(0.05f, 0.3f), 0.07f, 1);
                }
                
             
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            }
    }
}