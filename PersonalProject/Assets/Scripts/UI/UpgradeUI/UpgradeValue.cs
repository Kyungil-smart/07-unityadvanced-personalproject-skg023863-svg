using UnityEngine;

// 업그레이드 내용을 잠시 담아두려고 사용
public struct UpgradeValue
{
    public UpgradeData data;
    public float value;

    public UpgradeValue(UpgradeData data, float value)
    {
        this.data = data;
        this.value = value;
    }
}
