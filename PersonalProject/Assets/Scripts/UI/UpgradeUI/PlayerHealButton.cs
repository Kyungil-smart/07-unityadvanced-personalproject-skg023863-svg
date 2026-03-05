using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealButton : MonoBehaviour
{
    [SerializeField] private Message message; // 골드 부족할 때 나올 메시지
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _heal = 20f; // 체력회복 수치
    [SerializeField] private TMP_Text _healText; 
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private int _cost = 25; // 체력회복 가격
    
    void Awake()
    {
        _healText.text = $"체력 + {_heal}";
        _costText.text = $"가격 : {_cost}";
    }

    // 체력 회복 버튼의 OnClick 이벤트 함수에 구독
    public void Heal()
    {
        // 골드가 부족하면 힐 불가
        if (!message.LookGoldMassage(_cost))
        {
            return;
        }
        
        // 체력이 가득 차 있으면 힐 안 함
        if (!message.LookFullHPMessage(_player.CurrentHP, _player.MaxHP))
        {
            return;
        }
        
        GameManager.Instance._gold -= _cost;
        
        _player.CurrentHP += _heal;
        // 힐량이 초과되면 최대 체력과 같도록
        if(_player.CurrentHP >= _player.MaxHP)
        {
            _player.CurrentHP = _player.MaxHP;
        }
    }
}
