using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RoadItem
{
    public Sprite sprite;
    public int option;          // 0:몬스터, 1:상자, 2:???
    public int per;
    public int plusPer = 0;
    public int getPer()
    {
        return per+plusPer;
    }
    public void Init(int theOption, int thePer, int thePlusPer)
    {
        option = theOption;
        per = thePer;   
        plusPer = thePlusPer;
    }
}
[CreateAssetMenu(fileName = "RoadList", menuName = "Scriptable Object/RoadList")]
public class RoadList : ScriptableObject
{
    public RoadItem[] roadItems;

}