using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentItem
{
    public string name;
    public Sprite image;
    public int plusATK;
    public int plusDEF;
    public int plusHP;
    public int option;
    
}
[CreateAssetMenu(fileName = "EquipmentList", menuName = "Scriptable Object/EquipmentList")]
public class EquipmentList : ScriptableObject
{
    public EquipmentItem[] equipmentItems;
}
