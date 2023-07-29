using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Equipment : MonoBehaviour { 
    [SerializeField] SpriteRenderer image;
    int plusATK;
    int plusDEF;
    int plusHP;
    string name;
    public int option;
    //0 : weapon, 1 : helmet, 2 : shoulder, 3 : shoes, 4 : gloves, 5 : cape, 6: ring, 7: necklace, 8:bracelet, 9 : earrings
    public PRS originPRS;

    public int getPlusATK() { return plusATK; }
    public int getPlusDEF() { return plusDEF;}
    public int getPlusHP() { return plusHP;}
    public int getOption() { return option;}
    public Sprite getSprite() { return image.sprite; }
    public void Setup(EquipmentItem equpItem)
    {
        image.sprite = equpItem.image;
        plusATK = equpItem.plusATK;
        plusDEF = equpItem.plusDEF;     
        plusHP = equpItem.plusHP;
        name = equpItem.name;
        option = equpItem.option;
    }
    void mount()
    {

    }
    public void DieEquip()
    {
        Destroy(gameObject);
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {

            transform.DOMove(prs.pos, 3);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    void OnMouseDown()
    {
        CardManager.Inst.EquipMouseDown(this);
    }
}
