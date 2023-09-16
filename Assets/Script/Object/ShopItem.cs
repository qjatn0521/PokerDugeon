using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer image;
    [SerializeField] GameObject effect;
    EquipmentItem equpItem;
    int[] statPlus = new int[3];

    public void Setup(EquipmentItem equpItem)
    {
        this.equpItem = equpItem;
        image.sprite = equpItem.image;
    }
    //0 : atk, 1 : def, 2 : hp,
    public void Setup(int n)
    {
        int randPlus = Random.Range(0,5*(n-1));
        int what = Random.Range(0, 3);
    }
    public void onEffect()
    {
        effect.SetActive(true);
    }
    public void offEffect()
    {
        effect.SetActive(false);
    }
    public void DieEquip()
    {
        Destroy(gameObject);
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
        //CardManager.Inst.EquipMouseDown(this);
        
    }
}
