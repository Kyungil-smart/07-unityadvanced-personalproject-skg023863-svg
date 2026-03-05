using UnityEngine;
using TMPro;

public class Gold : MonoBehaviour
{
    [SerializeField] protected TMP_Text _goldText;

    void Update()
    {
        LookGoldAmount();
    }

    private void LookGoldAmount()
    {
        _goldText.text = $"골드 : {GameManager.Instance.Gold}";
    }
}
