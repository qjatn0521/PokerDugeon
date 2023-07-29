using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardItem
{
    public string shape;       //스페이드 1, 하트 2, 다이아 3, 클로버 4
    public Sprite sprite;
    public int value;
    public CardItem(string theShape, int theValue){
        shape = theShape;
        value = theValue;
    } 
}
[CreateAssetMenu(fileName = "CardList", menuName = "Scriptable Object/CardList")]
public class CardList : ScriptableObject
{
    public CardItem[] cardItems;
    [ContextMenu("Initialize Card Items")]
    private void InitializeCardItems()
    {
        // 배열의 크기와 초기값을 지정합니다.
        cardItems = new CardItem[]
        {
            new CardItem("spade", 1), new CardItem("spade", 2),new CardItem("spade", 3),new CardItem("spade", 4),
            new CardItem("spade", 5), new CardItem("spade", 6),new CardItem("spade", 7),new CardItem("spade", 8),
            new CardItem("spade", 9), new CardItem("spade", 10),new CardItem("spade", 11),new CardItem("spade", 12),
            new CardItem("spade", 13),
            new CardItem("diamond", 1), new CardItem("diamond", 2),new CardItem("diamond", 3),new CardItem("diamond", 4),
            new CardItem("diamond", 5), new CardItem("diamond", 6),new CardItem("diamond", 7),new CardItem("diamond", 8),
            new CardItem("diamond", 9), new CardItem("diamond", 10),new CardItem("diamond", 11),new CardItem("diamond", 12),
            new CardItem("diamond", 13),
            new CardItem("heart", 1), new CardItem("heart", 2),new CardItem("heart", 3),new CardItem("heart", 4),
            new CardItem("heart", 5), new CardItem("heart", 6),new CardItem("heart", 7),new CardItem("heart", 8),
            new CardItem("heart", 9), new CardItem("heart", 10),new CardItem("heart", 11),new CardItem("heart", 12),
            new CardItem("heart", 13),
            new CardItem("clover", 1), new CardItem("clover", 2),new CardItem("clover", 3),new CardItem("clover", 4),
            new CardItem("clover", 5), new CardItem("clover", 6),new CardItem("clover", 7),new CardItem("clover", 8),
            new CardItem("clover", 9), new CardItem("clover", 10),new CardItem("clover", 11),new CardItem("clover", 12),
            new CardItem("clover", 13)
            // 추가적으로 원하는 값을 여기에 계속 추가할 수 있습니다.
        };
    }
}
