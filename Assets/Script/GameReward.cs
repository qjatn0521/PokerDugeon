using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReward : MonoBehaviour
{
    [SerializeField] GameObject enemyBack;
    [SerializeField] int what;

    void OnMouseOver()
    {
        enemyBack.SetActive(true);
    }
    void OnMouseExit()
    {
        enemyBack.SetActive(false);
    }
    void OnMouseDown()
    {
        
        CardManager.Inst.RewardMouseDown(what);
    }
}
