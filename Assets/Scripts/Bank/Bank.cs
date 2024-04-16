using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;

    int currentBalance;

    public int CurrentBalance { get {return currentBalance;} }

    TextMeshProUGUI balanceText;

    private void Awake()
    {
        currentBalance = startingBalance;
        balanceText = GameObject.FindObjectOfType<TextMeshProUGUI>();
        updateDisplay();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        Debug.Log("Current balance: " + currentBalance);
        updateDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        Debug.Log("Current balance: " + currentBalance);
        if (currentBalance < 0)
        {
            GameManager.Instance.GameOver();
        }
        updateDisplay();
    }

    void updateDisplay()
    {
        balanceText.text = "Gold: " + currentBalance.ToString("D8");
    }


}
