using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldAward = 10;
    [SerializeField] int goldPenalty = 10;

    Bank bank;
    void Awake()
    {
        bank = GameObject.FindObjectOfType<Bank>();
    }

    public void RewardGold()
    {
        bank?.Deposit(goldAward);
    }

    public void PunishGold()
    {
        bank?.Withdraw(goldPenalty);
    }
}
