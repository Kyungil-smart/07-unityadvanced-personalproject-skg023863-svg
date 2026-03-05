using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    
    // UpgardeUI에서 업그레이드 버튼 3개중 하나 선택 시 해당 부분 value만큼 업그레이드
    public void ApplyUpgrade(UpgradeType type, float value)
    {
        switch (type)
        {
            case UpgradeType.maxHP: // 최대 체력 증가
                _player.MaxHP += value;
                _player.CurrentHP += value;
                break;
            
            case UpgradeType.damage: // 공격력 증가
                _player.Damage += value;
                break;
            
            case UpgradeType.speed: // 이동속도 증가
                _player.PlayerSpeed += value;
                break;
            
            case UpgradeType.fireRate: // 연사력 증가
                _player.FireRate *= value;
                break;
            
            case UpgradeType.bulletSpeed: // 총알속도 증가
                _player.BulletSpeed += value;
                break;
            
            case UpgradeType.BulletDistance: //사거리 증가
                _player.BulletDistance += value;
                break;
        }
    }
}
