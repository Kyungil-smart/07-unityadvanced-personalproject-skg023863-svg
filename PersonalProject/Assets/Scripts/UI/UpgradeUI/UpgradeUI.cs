using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Message message; // 골드 부족할 때 나올 메시지
    [SerializeField] private TMP_Text _rerollGoldText;
    [SerializeField] private int _rerollGold = 10; // 새로고침 가격
    
    [SerializeField] private PlayerUpgrade _playerUpgrade;
    
    [SerializeField] private List<UpgradeData> _upgradeDatasList; // 플레이어 업그레이드 목록, Inspector창에서 조정
    
    [SerializeField] private Button[] _upgradeButtons;
    
    [SerializeField] private TMP_Text[] _upgradeTitleText; // 어떤 업그레이드인지 표기
    [SerializeField] private TMP_Text[] _upgradeValueText; // 업그레이드 수치 표기
    [SerializeField] private TMP_Text[] _upgradeCostText;  // 업그레이드 가격 표기
    
    private UpgradeValue[]  _upgradeValues =  new UpgradeValue[3]; // 업그레이드 내용을 담아둘 데이터
    
    void Awake()
    {
        _rerollGoldText.text = $"가격 : {_rerollGold}";
    }

    void OnEnable()
    {
        Init();
    }

    // 새로고침 눌렀을 시 호출되는 함수
    public void OnClickReRoll()
    {
        // 골드 부족시 새로고침 불가
        if (!message.LookGoldMassage(_rerollGold))
        {
            return;
        }
        
        Init();
        GameManager.Instance._gold -= _rerollGold;
    }

    // 새로고침이나, 웨이브 끝나고 UpgradeUI 등장시 사용할 함수
    public void Init()
    {
        // 업그레이드 버튼 다시 활성화
        foreach (Button button in _upgradeButtons)
        {
            button.interactable = true;
        }
        RandomUpgrade();
        UpgradeButtonUI();
    }
    
    //_upgradeValues배열에 랜덤한 업그레이드가 들어가도록 하는 함수
    private void RandomUpgrade()
    {
        // _upgradeDatasList를 randDataList에 복사
        List<UpgradeData> randDataList = new List<UpgradeData>(_upgradeDatasList);

        // 버튼이 3개 있으므로 for문 3번 돌리기
        for (int i = 0; i < 3; i++) 
        {
            // 0부터 현재 들어있는 업그레이드 목록 수 중 랜덤한 수를 randIndex에 저장
            int randIndex = Random.Range(0, randDataList.Count);
            
            // data에 randDataList중에서 randIndex에 해당하는 UpgradeData를 저장
            UpgradeData data = randDataList[randIndex];
            
            // 중복된 업그레이드가 안 나오도록 randIndex에 해당하는 UpgradeData는 삭제
            randDataList.RemoveAt(randIndex);
            
            // UpgradeData에서 지정해 놓은 minValue, maxValue 중 랜덤한 값이 업그레이드 수치
            float value = Random.Range(data.minValue, data.maxValue);
            
            // _upgradeValues에 차례대로 data(UpgradeData), value(float) 저장
            _upgradeValues[i] = new UpgradeValue(data, value);
        }
    }

    // 각 버튼에 어떤 업그레이드인지 표시해 주기 위한 함수
    private void UpgradeButtonUI()
    {
        for (int i = 0; i < 3; i++)
        {
            UpgradeValue upgrade =  _upgradeValues[i];
            _upgradeTitleText[i].text = upgrade.data.title;
            
            // fireRate(연사속도)는 다른 업그레이드와 달리 곱(*)으로 업그레이드 하기 때문에 %로 표기
            if (upgrade.data.type == UpgradeType.fireRate)
            {
                _upgradeValueText[i].text = $"{upgrade.data.value} {(100 - (upgrade.value * 100)):F1}% 증가";
            }
            else
            {
                _upgradeValueText[i].text = $"{upgrade.data.value} {upgrade.value:F1}";
            }
            
            _upgradeCostText[i].text = $"가격 : {upgrade.data.cost}";
        }
    }
    
    //업그레이드 버튼 눌렀을 때 동작할 함수, 각 업그레이드 버튼의 OnClick 이벤트 함수에 등록, 각 버튼에 0~2의 int index를 지정
    public void SelectUpgrade(int index)
    {
        UpgradeValue upgrade = _upgradeValues[index];

        // 골드 부족시 구매 불가
        if (!message.LookGoldMassage(upgrade.data.cost))
        {
            return;
        }
        
        //실제 업그레이드가 이루어짐
        _playerUpgrade.ApplyUpgrade(upgrade.data.type, upgrade.value);
        GameManager.Instance._gold -= upgrade.data.cost;
        
        // 한 번 구매하고 나면 새로고침하지 않는 이상 구매 불가
        foreach (Button button in _upgradeButtons)
        {
            button.interactable = false;
        }
    }
}
