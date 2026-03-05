using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("스폰 포인트")]
    [SerializeField] private Transform[] _spawnPoints; // 동,서,남,북 Spawner

    [Header("몬스터 프리팹")]
    [SerializeField] private MonsterBase _runMonsterPrefab; 
    [SerializeField] private MonsterBase _stopAndRunMonsterPrefab;
    
    [Header("몬스터 스폰 딜레이")]
    private WaitForSeconds _waitForSeconds;
    [SerializeField] private float _waveDelay = 3f; // _waveDelay 후에 몬스터 스폰 

    [Header("웨이브 관리")]
    [SerializeField] public WaveData[] _waves;

    private int _aliveCount; // 웨이브 마다 총 몬스터 수
    public System.Action OnWaveClear;
    
    public void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_waveDelay);
    }
    
    // 웨이브 시작 함수
    public void StartWave(int waveIndex)
    {
        // WaveData data = _waves[waveIndex];
        //
        // int totalMonsterCount = data.runMonsterCount + data.stopAndRunMonsterCount;
        // _aliveCount = totalMonsterCount;
        //
        // StartCoroutine(SpawnRoutine(data));
        StartCoroutine(StartWaveDelay(waveIndex));
    }

    private IEnumerator StartWaveDelay(int waveIndex)
    {
        yield return _waitForSeconds;
        
        WaveData data = _waves[waveIndex];

        int totalMonsterCount = data.runMonsterCount + data.stopAndRunMonsterCount;
        _aliveCount = totalMonsterCount;
        
        StartCoroutine(SpawnRoutine(data));
    }
    
    private IEnumerator SpawnRoutine(WaveData data)
    {
        WaitForSeconds wait = new WaitForSeconds(data.spawnDelay);
        
        // RunMonster 먼저
        for (int i = 0; i < data.runMonsterCount; i++)
        {
            Spawn(_runMonsterPrefab);
            yield return wait;
        }

        // StopAndRunMonster 다음
        for (int i = 0; i < data.stopAndRunMonsterCount; i++)
        {
            Spawn(_stopAndRunMonsterPrefab);
            yield return wait;
        }
        
        yield break;
    }
    
    // 몬스터 생성
    private void Spawn(MonsterBase prefab)
    {
        // 스포너 4곳중 랜덤하게 나옴
        Transform spawner = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        MonsterBase monster = Instantiate(prefab, spawner.position, Quaternion.identity);

        monster.OnDeath += HandleMonsterDeath;
    }
    
    //몬스터가 죽을 때 마다 _aliveCount가 1씩 적어짐
    private void HandleMonsterDeath()
    {
        _aliveCount--;

        if (_aliveCount <= 0)
        {
            OnWaveClear?.Invoke();
        }
    }
}