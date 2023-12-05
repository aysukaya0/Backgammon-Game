using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SNG.Save;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] Text _displayedMoney;
    private long _money;

    private void Start()
    {
        _money = SaveGame.Instance.PlayerData.Money;
        string text = _money.ToString();
        _displayedMoney.text = text;
    }

    private void Update()
    {
        _money = SaveGame.Instance.PlayerData.Money;
        string text = _money.ToString();
        _displayedMoney.text = text;
    }
}
