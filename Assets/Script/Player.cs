using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshPro textCurHP;
    [SerializeField] TextMeshPro textMaxHP;
    [SerializeField] TextMeshPro textATK;
    [SerializeField] TextMeshPro textShield;
    [SerializeField] TextMeshPro textMoney;
    [SerializeField] SpriteRenderer HPAttack;
    int maxHP = 200;
    int ATK = 10;
    int DEF = 10;
    int curHP = 200;
    int plusATK=0;
    int plusDEF=0;
    int plusHP=0;
    int coin = 0;

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
        plusHP += e.getPlusHP();
        curHP += e.getPlusHP();
        if(curHP > maxHP)
        {
            curHP = maxHP;
        }
    }
}
