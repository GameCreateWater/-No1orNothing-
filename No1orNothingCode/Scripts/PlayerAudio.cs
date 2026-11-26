using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private PlayerSE seList;
    [SerializeField] private AudioSource audioSource;

    public void AudioPlay(PlayerSE.AudioMode mode)
    {
        if(seList == null)
        {
            Debug.Log("プレイヤーSEがアタッチされていません。");
            return;
        }
        switch (mode)
        {
            case PlayerSE.AudioMode.walk:
                audioSource.PlayOneShot(seList.walkAudio);
                break;
            case PlayerSE.AudioMode.jump:
                audioSource.PlayOneShot(seList.jumpAudio);
                break;
            case PlayerSE.AudioMode.attack:
                audioSource.PlayOneShot(seList.attackAudio);
                break;
            case PlayerSE.AudioMode.damage:
                audioSource.PlayOneShot(seList.damageAudio);
                break;
            case PlayerSE.AudioMode.landing:
                audioSource.PlayOneShot(seList.landingAudio);
                break;
        }
    }
}
