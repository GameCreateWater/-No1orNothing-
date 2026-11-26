using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextRound : MonoBehaviour
{
    private RoundManager roundManager;
    [SerializeField] private GameObject result;
    private bool resultProcessed;


    private void Start()
    {
        roundManager = GetComponent<RoundManager>();
    }
    private void Update()
    {
        if (!result.activeInHierarchy)
        {
            resultProcessed = false;
            return;
        }

        if (resultProcessed || !ResultAnime()) return;

        resultProcessed = true;
        roundManager.OnResult();
    }

    private bool ResultAnime()
    {
        foreach (Transform player in result.transform)
        {
            if (player.CompareTag("line"))
            {
                var lineCreate = player.gameObject.GetComponent<ScoreLineCreate>();
                int finalPointIndex = Mathf.Min(RoundManager.roundCount, lineCreate.obj.Length - 1);
                if (lineCreate.obj[finalPointIndex] == null && !lineCreate.on) return false;

            }
        }
        return true;
    }
}
