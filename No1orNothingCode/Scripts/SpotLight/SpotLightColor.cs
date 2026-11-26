using UnityEngine;

public class SpotLightColor : MonoBehaviour
{
    public Color color;
    private Material material;

    private void Awake()
    {
        material =GetComponent<MeshRenderer>().material;

    }
    private void Update()
    {
        if(material.color != color)
        {
            material.color = color;
        }
    }
}
