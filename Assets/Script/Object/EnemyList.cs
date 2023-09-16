using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class EnemyItem
{
    public string name;
    public int health;
    public Sprite sprite;
    public int damage;
}
[CreateAssetMenu(fileName = "EnemyList", menuName = "Scriptable Object/EnemyList")]
public class EnemyList : ScriptableObject
{
    public EnemyItem[] enemyItems;
}
