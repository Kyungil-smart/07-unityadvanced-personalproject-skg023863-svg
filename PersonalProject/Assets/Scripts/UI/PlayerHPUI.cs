using UnityEngine;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerHPUI;
    [SerializeField] private PlayerController _player;

    void Start()
    {
        _playerHPUI.text = $"HP: {_player.CurrentHP:F1} / {_player.MaxHP:F1}";
    }
    void OnEnable()
    {
        _player.OnPlayerHPChanged += ShowPlayerHPUI;
    }

    void OnDisable()
    {
        _player.OnPlayerHPChanged -= ShowPlayerHPUI;
    }
    
    void ShowPlayerHPUI(float currentHP, float maxHP)
    {
        _playerHPUI.text = $"HP: {currentHP:F1} / {maxHP:F1}";
    }
}
