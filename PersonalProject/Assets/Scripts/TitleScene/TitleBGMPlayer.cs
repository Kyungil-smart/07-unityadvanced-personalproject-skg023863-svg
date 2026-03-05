using UnityEngine;

public class TitleBGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _TitleBGM;
    void Start()
    {
        AudioManager.Instance.PlayBGM(_TitleBGM, 0.2f);
    }
}
