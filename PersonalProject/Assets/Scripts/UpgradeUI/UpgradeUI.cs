using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    // 다음 웨이브 시작 버튼
    public void OnClickNextWave()
    {
        Time.timeScale = 1f;
        GameManager.Instance.StartWave();
        gameObject.SetActive(false);
    }
}
