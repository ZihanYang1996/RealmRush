using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;

    int currentBalance;

    public int CurrentBalance { get {return currentBalance;} }

    private void Awake()
    {
        currentBalance = startingBalance;
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        Debug.Log("Current balance: " + currentBalance);
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        Debug.Log("Current balance: " + currentBalance);
        if (currentBalance < 0)
        {
            throw new NotImplementedException("TBD");
        }
    }


}
