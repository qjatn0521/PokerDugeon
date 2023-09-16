using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Equipment : MonoBehaviour { 
    [SerializeField] SpriteRenderer image;
    
    [SerializeField] GameObject effect;
    [SerializeField] Sprite[] effects;
    int plusATK;
    int plusDEF;
    int grade;
    int option;
    int id;
    Sprite image2;
    //0 : weapon, 1 : helmet, 2 : °©¿Ê, 3 : shoes, 4 : gloves, 5
    public PRS originPRS;

    private bool position = false;

    public int getPlusATK() { return plusATK; }
    public int getPlusDEF() { return plusDEF;}
    public int getOption() { return option;}
    public Sprite getSprite() { return image2; }
    public int getId() { return id; }
    public void Setup(EquipmentItem equpItem)
    {
        image.sprite = equpItem.image;
        plusATK = equpItem.plusATK;
        plusDEF = equpItem.plusDEF;     
        option = equpItem.option;
        grade = equpItem.grade;
        image2 = equpItem.image2;
        id = equpItem.id;
        Color newColor;
        if(grade == 1) newColor = new Color(0.0f, 0.714f, 1.0f, 1.0f);
        else if (grade == 2) newColor = new Color(0.694f, 0.0f, 1.0f, 1.0f);
        else if (grade == 3) newColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        else  newColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        effect.GetComponent<SpriteRenderer>().color = newColor;
    }
    public void rotate()
    {
        transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f);
        position = true;
    }
    public void onEffect()
    {
        effect.SetActive(true);
    }
    public void offEffect()
    {
        effect.SetActive(false);
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
            transform.DORotateQuaternion(Quaternion.Euler(0f, 180f, 0f), 0.01f);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    void OnMouseOver()
    {
        onEffect();
    }
    void OnMouseExit()
    {
        offEffect();
    }
    void OnMouseDown()
    {
        if (!position)
            rotate();
        else
        {
            CardManager.Inst.EquipMouseDown(this);
        }
    }
}
