using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSE", menuName = "Scriptable Objects/PlayerSE")]
public class PlayerSE : ScriptableObject
{
    [Tooltip("歩きSE")]
    public AudioClip walkAudio;
    [Tooltip("攻撃SE")]
    public AudioClip attackAudio;
    [Tooltip("攻撃を食らった時のSE")]
    public AudioClip damageAudio;
    [Tooltip("ジャンプ時SE")]
    public AudioClip jumpAudio;
    [Tooltip("着地SE")]
    public AudioClip landingAudio;

    [System.Serializable]
    public enum AudioMode 
    {walk,
    attack,
    damage,
    jump,
    landing}

}
