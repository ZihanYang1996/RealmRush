using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 50;
    Bank bank;
    public GameObject CreateTower(GameObject tower, Vector3 position, Quaternion quaternion, GameObject parent)
    {
        bank = FindObjectOfType<Bank>();
        // If bank is not found in the scene, log an error and return false
        if (bank == null)
        {
            Debug.LogError("Bank not found in scene");
            return null;
        }

        // If the bank's current balance is less than the cost of the tower, log an error and return false
        if (bank.CurrentBalance < cost)
        {
            Debug.Log("Insufficient funds");
            return null;
        }

        // Instantiate a new tower at the specified position and rotation
        GameObject newTower = Instantiate(tower, position, quaternion);
        // Also works, as Instantiate() can take components as well
        // GameObject newTower = Instantiate(gameObject, position, quaternion);
        newTower.transform.SetParent(parent.transform);  // or tower.transform.parent = towers.transform;
        newTower.name = "Tower Instance";
        
        // Deduct the cost of the tower from the bank's current balance
        bank.Withdraw(cost);
        
        return newTower;
    }

    public void DestroyTower(bool refound = false)
    {
        if (refound)
        {
            bank?.Deposit(cost);
        }
        Destroy(gameObject);
    }
}
