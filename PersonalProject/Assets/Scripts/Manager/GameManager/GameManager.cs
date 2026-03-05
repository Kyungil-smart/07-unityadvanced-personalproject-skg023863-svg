using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private GameObject _upgradeUI;
    [SerializeField] private GameObject _VictoryUI;
    [SerializeField] private int MaxWaves; // 최대 Wave

    private int _waveIndex = -1; // 0부터 시작하려고
    public int _gold; // 골드 획득 량

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // _spawnManager의 waves 배열 크기 만큼
        MaxWaves = _spawnManager._waves.Length;
        Debug.Log(MaxWaves);
    }

    void OnEnable()
    {
        _spawnManager.OnWaveClear += HandleWaveClear;
    }

    void OnDisable()
    {
        _spawnManager.OnWaveClear -= HandleWaveClear;
    }

    private void Start()
    {
        StartWave();
    }

    //UpgardeUI에서 다음 웨이브 클릭 시 다음 웨이브 시작
    public void StartWave()
    {
        _waveIndex++;

        if (_waveIndex >= MaxWaves)
        {
            Victory();
            return;
        }
        _spawnManager.StartWave(_waveIndex);
    }

    // 마지막 웨이브 클리어 했는지
    private void HandleWaveClear()
    {
        // 마지막 Wave 클리어 여부
        if (_waveIndex >= MaxWaves - 1)  
        {
            Victory();
            return;
        }
        
        // 클리어 한게 아니면 상점 UI 오픈
        ShowUpgradeUI();
    }

    // UpgradeUI를 보여주는 함수
    private void ShowUpgradeUI()
    {
        _upgradeUI.SetActive(true);
        
        // UpgradeUI가 열리면 Player 움직임 멈춤
        Time.timeScale = 0f;
    }

    public void AddGold(int gold)
    {
        _gold += gold;
    }

    private void Victory()
    {
        Debug.Log("Game Clear!");
    }
}
