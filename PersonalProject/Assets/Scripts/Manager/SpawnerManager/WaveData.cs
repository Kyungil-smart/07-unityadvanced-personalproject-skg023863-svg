using System;
using UnityEngine;

[Serializable]
public class WaveData
{
    [Header("몬스터 종류 / 등장 개수")]
    public int runMonsterCount;
    public int stopAndRunMonsterCount;

    [Header("몬스터 스폰 딜레이")]
    public float spawnDelay;
}