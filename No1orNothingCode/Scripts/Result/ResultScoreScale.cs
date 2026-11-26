using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ResultScoreScale : MonoBehaviour
{
    public TMP_Text scoreScale;
    public void ScaleMemori(float maxScore,float minX,float maxY)
    {
       // Debug.Log(maxScore);
       // Debug.Log(minX);
       // Debug.Log(maxY);
        scoreScale.transform.position = new Vector2(minX,maxY);
        Debug.Log(maxScore);
        scoreScale.text = Mathf.FloorToInt(Mathf.Max(0f, maxScore)).ToString("N0");

    }
}
