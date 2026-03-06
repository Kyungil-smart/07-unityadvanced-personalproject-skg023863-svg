using System.Collections;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _waveStart; // 웨이브 시작 시 나오는 문구
    [SerializeField] private TMP_Text _waveCount; // 현재 몇 웨이브인지 표시
    [SerializeField] private float _waitTime = 3f; // _waveStart 문구가 몇초간 나올지
    private WaitForSeconds _waitForSeconds;
    private int _waveNumber; // 현재 웨이브

    void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_waitTime);
    }

    public void ShowWave (int waveIndex)
    {
        _waveNumber = waveIndex + 1;

        _waveCount.text = $"웨이브 {_waveNumber}";
        _waveStart.text = $"웨이브 {_waveNumber} 시작!";
        StartCoroutine(WaveStartCoroutine());
    }

    private IEnumerator WaveStartCoroutine()
    {
        _waveStart.gameObject.SetActive(true);
        yield return _waitForSeconds;
        _waveStart.gameObject.SetActive(false);

        yield break;
    }
}
