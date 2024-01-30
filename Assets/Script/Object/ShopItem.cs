using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer image;
    [SerializeField] GameObject effect;
    [SerializeField] GameObject effect2;
    [SerializeField] TextMeshPro saleInfo;
    [SerializeField] TextMeshPro coinText;
    [SerializeField] TextMeshPro saleCoinText;
    [SerializeField] TextMeshPro infoText;
    [SerializeField] Sprite[] potions;
    string[] text = {"?? ATK", "?? DEF", "??? HP", "30 coin", "15 coin" };
    EquipmentItem equpItem;
    int statPlus = 0;
    int coin = 20;
    int n=100;


    public void Setup(EquipmentItem equpItem)
    {
        this.equpItem = equpItem;

        image.sprite = equpItem.image2;
        coin += equpItem.grade * 20;
        coinText.text = coin + " coin";
        effect.GetComponent<SpriteRenderer>().sprite = equpItem.image;

        if(equpItem.sale>0) SetSale(equpItem.sale);
    }
    //0 : atk, 1 : def, 2 : hp, 4 : 50%hp, 5 : 25%hp
    public void Setup(int n)
    {
        this.n = n;
        statPlus = Random.Range(-5,5);
        effect2.GetComponent<SpriteRenderer>().sprite = potions[n];
        effect = effect2;
        image.sprite = potions[n];
        coinText.text = text[n];
        if (n == 3)
        {
            infoText.text = "+50% HP";
            coin = 30;
        }
        else if (n == 4)
        {
            infoText.text = "+25% HP";
            coin = 15;
        }
        coinText.GetComponent<Transform>().position -= new Vector3(0f, 0.2f, 0f);
        saleCoinText.GetComponent<Transform>().position -= new Vector3(0f, 0.2f, 0f);
        image.GetComponent<Transform>().position += new Vector3(0f, 0.7f, 0f);
        effect.GetComponent<Transform>().position += new Vector3(0f, 0.7f, 0f);

    }
    public int GetCoin() { return coin; }
    public int GetOption() { return n; }
    public int GetPlus() { return statPlus; }
    public EquipmentItem GetEquip() {
        return equpItem; }
    public void SetSale(int sale)
    {
        saleInfo.text = "-" + sale + "%";
        saleInfo.gameObject.SetActive(true);
        saleCoinText.text = coin + " coin";
        coinText.GetComponent<Transform>().position += new Vector3(0.6f, 0f, 0f);
        coin = coin - coin * sale / 100;
        coinText.text = coin + " coin";
    }
    public void onEffect()
    {
        effect.SetActive(true);
        effect2.SetActive(true);
    }
    public void offEffect()
    {
        effect.SetActive(false);
        effect2.SetActive(false);
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
        bool die =CardManager.Inst.ShopMouseDown(this);
        if(die) offActive();
    }
    public void offActive()
    {
        gameObject.SetActive(false);
    }
    public EquipmentItem getEquip()
    {
        return equpItem;
    }
}
