using UnityEngine;
using System.Collections;
public class CameraAriaSize: MonoBehaviour
{
    //シャッターのサイズン港
    public float xSize;
    public float ySize;
    public float xSizemin;
    public float xSizemax;
    public float ySizemin;
    public float ySizemax;
    public bool xSizeOn = false;
    public bool ySizeOn = false;

    [Header("サイズ変化にかかる時間")]
    public float changeTime;

    private BoxCollider2D bounds;
    public GameObject aria;
    private BoxCollider2D boundsAria;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bounds = this.GetComponent<BoxCollider2D>();
        boundsAria = aria.GetComponent<BoxCollider2D>();
        xSize = transform.localScale.x;
        ySize = transform.localScale.y;
    }

    public void SizeChange()
    {
        xSize = Random.Range(xSizemin, xSizemax);
        ySize = Random.Range(ySizemin, ySizemax);

        xSizeOn = true;
        ySizeOn = true;
    }

    void Update()
    {

        /*
        if ((Mathf.Abs(transform.localScale.x - xSize) > 0.3f || Mathf.Abs(transform.localScale.y - ySize) > 0.3f) && (xSizeOn || ySizeOn))
        {
                if (bounds.bounds.min.x < boundsAria.bounds.min.x || bounds.bounds.max.x > boundsAria.bounds.max.x)
                {
                }
                else if(xSizeOn)
                {
                    transform.localScale += new Vector3((xSize - transform.localScale.x), 0) * Time.deltaTime / changeTime;
                }

                if (bounds.bounds.min.y < boundsAria.bounds.min.y || bounds.bounds.max.y > boundsAria.bounds.max.y)
                {
                }
                else if(ySizeOn)
                {
                    transform.localScale += new Vector3(0, (ySize - transform.localScale.y)) * Time.deltaTime / changeTime;
                }

        }*/

        if (((Mathf.Abs(transform.localScale.x - xSize) < 0.3f && !ySizeOn) || (Mathf.Abs(transform.localScale.y - ySize) < 0.3f && !xSizeOn)) || 
            (Mathf.Abs(transform.localScale.x - xSize) < 0.3f && Mathf.Abs(transform.localScale.y - ySize) < 0.3f)
            || (!xSizeOn && !ySizeOn))
        {
            // 再始動
            GetComponent<CameraAria>().move = true;
        }
        else
        {
            if (bounds.bounds.min.x < boundsAria.bounds.min.x || bounds.bounds.max.x > boundsAria.bounds.max.x)
            {
            }
            else if (xSizeOn)
            {
                transform.localScale += new Vector3((xSize - transform.localScale.x), 0) * Time.deltaTime / changeTime;
            }

            if (bounds.bounds.min.y < boundsAria.bounds.min.y || bounds.bounds.max.y > boundsAria.bounds.max.y)
            {
            }
            else if (ySizeOn)
            {
                transform.localScale += new Vector3(0, (ySize - transform.localScale.y)) * Time.deltaTime / changeTime;
            }
        }
        //はみ出し防止

        if (bounds.bounds.min.x < boundsAria.bounds.min.x || bounds.bounds.max.x > boundsAria.bounds.max.x)
        {
           // Debug.Log("x大きい");
            transform.localScale += new Vector3(-10,0) * Time.deltaTime;
            xSizeOn = false;
        }

        if (bounds.bounds.min.y < boundsAria.bounds.min.y || bounds.bounds.max.y > boundsAria.bounds.max.y)
        {
            //Debug.Log("y大きい");
            transform.localScale += new Vector3(0,-10) * Time.deltaTime;
            ySizeOn = false;
        }

    }
}
