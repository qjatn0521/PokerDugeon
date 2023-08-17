using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshPro textCurHP;
    [SerializeField] TextMeshPro textMaxHP;
    [SerializeField] TextMeshPro textATK;
    [SerializeField] TextMeshPro textShield;
    [SerializeField] TextMeshPro textMoney;
    [SerializeField] SpriteRenderer HPAttack;

    [SerializeField] GameObject hpBar;
    [SerializeField] private Transform fullHp;
    [SerializeField] private Transform noHp;

    [SerializeField] TextMeshPro textChange;
    [SerializeField] SpriteRenderer[] changes = new SpriteRenderer[5];
    int maxHP = 200;
    int ATK = 10;
    int DEF = 10;
    int curHP = 200;
    int plusATK=0;
    int plusDEF=0;
    int plusHP=0;
    int coin = 0;
    public int rewardNum = 4;
    public int changeNum = 3;
    public int cardNum = 7;
    public int curchangeNum = 3;
    [SerializeField] Sprite[] baseEquipment = new Sprite[10];
    [SerializeField] SpriteRenderer[] equipment = new SpriteRenderer[10];
    //0 : weapon, 1 : helmet, 2 : shoulder, 3 : shoes, 4 : gloves, 5 : cape, 6: ring, 7: necklace, 8:bracelet, 9 : earrings
    Equipment[] equip = new Equipment[10];
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            equipment[i].sprite = baseEquipment[i];
        }
        curHP = maxHP;
        textCurHP.text = maxHP+plusHP+"";
        textMaxHP.text = "/"+ maxHP;
        textATK.text = ATK + plusATK+"";
        textShield.text = DEF + plusDEF+"";
        textMoney.text = "0 coin";
    }
    public int GetATK() { return ATK+ plusATK; } 
    public int GetDEF() { return DEF+plusDEF; }
    public int GetHP() { return curHP; }
    public void GetCoin(int theCoin) { 
        coin += theCoin;
        textMoney.text = coin+" coin";
    }
    public void TakeHP(int hp) {
        curHP -= hp;
        textCurHP.text = curHP +"";
        float hpRatio = (float)curHP / maxHP;
        Vector3 targetPosition = Vector3.Lerp(noHp.position, fullHp.position, hpRatio);
        hpBar.transform.DOMove(targetPosition, 0.5f);
        StartCoroutine("FadeInBackground");
    }
    public IEnumerator FadeInBackground()
    {
        for (float i = 1; i < 256; i++)
        {
            HPAttack.color = new Color(1, 0, 0, i/255);
            yield return new WaitForSeconds(0.00015f);
        }
        for (float i = 255; i >= 0; i--)
        {
            HPAttack.color = new Color(1, 0, 0, i / 255);
            yield return new WaitForSeconds(0.00015f );
        }
        yield return null;
    }
    public void GetEquip(Equipment e)
    {
        equip[e.getOption()] = e;
        equipment[e.getOption()].sprite = e.getSprite();
        plusATK += e.getPlusATK();
        plusDEF += e.getPlusDEF();
        textATK.text = ATK + plusATK + "";
        textShield.text = DEF + plusDEF + "";
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
    }
    public void resetChange()
    {
        for (int i = 0; i < 5; i++)
        {
            changes[i].material.DOFade(0f, 0.01f);
        }
        textChange.text = " ";
        if (changeNum < 6)
        {
            for(int i=0;i< changeNum;i++)
            {
                changes[i].material.DOFade(1f, 0.5f);
            }
        } else
        {
            for (int i = 0; i < 5; i++)
            {
                changes[i].material.DOFade(1f, 0.5f);
            }
            textChange.text = "+" + (changeNum - 5);
        }
        curchangeNum = changeNum;
    }
    public void useChange()
    {
        if(curchangeNum > 0)
        {
            if(curchangeNum >6)
            {
                textChange.text = "+" + (curchangeNum - 6);
            } else if(curchangeNum ==5)
            {
                textChange.text = " ";
            } else
            {
                changes[curchangeNum - 1].material.DOFade(0f, 0.5f);
            }
            curchangeNum--;
        } else
        {
            return;
        }
    }
}
