using UnityEngine;

public class SceneGateTrigger : MonoBehaviour
{
    [SerializeField] private SceneGateSelect sceneGate;


    private void Awake()
    {
        //セレクトシーン開始時GameManagerへのアタッチ
        GameManager.Instance.SceneGate = this.gameObject;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CursorController cursor = other.GetComponent<CursorController>();

        if (cursor != null)
        {
            cursor.currentGate = sceneGate;
            cursor.selectScene = null;
            Debug.Log("Gate Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CursorController cursor = other.GetComponent<CursorController>();

        if (cursor != null)
        {
            if (cursor.currentGate == sceneGate)
            {
                cursor.currentGate = null;
                cursor.selectScene = null;
            }

            Debug.Log("Gate Exit");
        }
    }
}