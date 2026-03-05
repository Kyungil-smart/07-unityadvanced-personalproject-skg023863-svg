using System;
using UnityEngine;

//업그레이드 목록에 사용할 틀
[Serializable]
public class UpgradeData
{
    public UpgradeType type; // 어떤 업그레이드인지
    public string title; // 어떤 업그레이드인지 표기
    public string value; // 업그레이드 수치 표기
    public int cost; // 업그레이드 가격

    public float minValue; // 업그레이드 수치의 최소치
    public float maxValue; // 업그레이드 수치의 최대치
}
