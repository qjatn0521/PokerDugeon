using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    private int playerHp;
    [SerializeField]
    private int playerAttack;
    [SerializeField]
    private int playerShield;

    public void setHp(int num)
    {
        playerHp += num;
    }
    public void setAttack(int num)
    {
        playerAttack += num;
    }
    public void setShield(int num)
    {
        playerShield += num;
    }
    public int getHp()
    {
        return playerHp;
    }
    public int getAttack()
    {
        return playerAttack;
    }
    public int getShield()
    {
        return playerShield;
    }
}
