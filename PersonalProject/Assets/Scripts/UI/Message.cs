using System.Collections;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private float _secondsRealtime = 2f; // 화면에 메시지가 언제까지 나올지
    private WaitForSecondsRealtime _waitForSecondsRealtime; // UpgradeUI화면이 나오면 Time.timeScale = 0이므로 실제 시간을 사용
    [SerializeField] private GameObject _lackGoldMassageText; // "골드가 부족합니다!"가 있는 오브젝트
    [SerializeField] private GameObject _fullHPText; // "체력이 가득 차 있습니다!"가 있는 오브젝트
    

    void Awake()
    {
        _waitForSecondsRealtime = new WaitForSecondsRealtime(_secondsRealtime);
    }
    
    // 골드가 부족하면 false, 충분하면 true 반환
    public bool LookGoldMassage(int gold)
    {
        if (GameManager.Instance.Gold < gold)
        {
            StartCoroutine(MessageCoroutine(_lackGoldMassageText));
            return false;
        }
        return true;
    }

    // Player의 체력이 가득 차 있으면 false, 아니면 true 반환
    public bool LookFullHPMessage(float currentHP, float maxHP)
    {
        if (currentHP >= maxHP)
        {
            StartCoroutine(MessageCoroutine(_fullHPText));
            return false;
        }
        return true;
    }
    
    // _waitForSecondsRealtime만큼 보여줌
    private IEnumerator MessageCoroutine(GameObject message)
    {
        message.SetActive(true);
        yield return _waitForSecondsRealtime;
        message.SetActive(false);
        
        yield break;
    }
}
