using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    [SerializeField] private GameObject _upgradeUI; //UpgradeUI 오브젝트
    
    // 다음 웨이브 버튼의 OnClick 이벤트 함수에 구독
    public void OnClickNextWave()
    {
        Time.timeScale = 1f;
        GameManager.Instance.StartWave();
        _upgradeUI.SetActive(false);
    }
}
