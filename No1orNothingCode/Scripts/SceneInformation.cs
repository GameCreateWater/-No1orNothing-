using UnityEngine;

public class SceneInformation : MonoBehaviour
{
    [SerializeField] private bool scoreSave;
    [SerializeField] private bool notroundCount;

    private void Start()
    {
        ResultScoreInput.scoreSave = this.scoreSave;

        RoundManager.roundNot = this.notroundCount;
    }
}
