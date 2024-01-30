using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentItem
{
    public Sprite image;
    public Sprite image2;
    public int grade;
    public int plusATK;
    public int plusDEF;
    public int option;
    public int id;
    public int sale;
}
[CreateAssetMenu(fileName = "EquipmentList", menuName = "Scriptable Object/EquipmentList")]
public class EquipmentList : ScriptableObject
{
    public EquipmentItem[] commonItems;
    public EquipmentItem[] rareItems;
    public EquipmentItem[] epicItems;
    public EquipmentItem[] legendItems;
}
