using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer enemy;
    [SerializeField] private GameObject enemyBack;
    [SerializeField] private GameObject enemySelect;
    [SerializeField] private TextMeshPro textHP;
    [SerializeField] private TextMeshPro textDamage;
    private EnemyItem enemyItem;
    private int enemyHP;
    private int enemyDamage;
    private Transform removePoint;
    private bool select = false;
    private bool die = false;

    public PRS originPRS;
    public void setup(EnemyItem theEnemyItem, Transform theRemovePoint)
    {
        this.enemyItem = theEnemyItem;
        enemy.sprite = theEnemyItem.sprite;
        enemyHP = theEnemyItem.health;
        enemyDamage = theEnemyItem.damage;
        this.removePoint = theRemovePoint;

        int rand = Random.Range(-enemyHP / 10, enemyHP / 10);
        enemyHP += rand;
        rand = Random.Range(-enemyDamage / 10, enemyDamage / 10);
        enemyDamage += rand;

        textHP.text = enemyHP + "";
        textDamage.text = enemyDamage + "";

        
    }
    public int getHP() { return enemyHP; }
    public bool takeDamage(int theDamage)
    {
        enemyHP -= theDamage;
        if(enemyHP <= 0)
        {
            dieEnemy();
            return true;
        }
        textHP.text = enemyHP + "";
        return false;

    }
    public int attack()
    {
        Vector3 targetScale = Vector3.one * 1.5f;
        transform.DOScale(targetScale, 0.3f)
            .OnComplete(() => // 애니메이션이 끝난 후에 실행될 코드를 정의합니다.
            {
                // 물체를 원래 크기로 돌려놓습니다.
                transform.DOScale(Vector3.one, 0.15f);
            });

        return enemyDamage;

    }
    private void dieEnemy()
    {
        die = true;
        // 투명해지는 애니메이션을 실행합니다.
        enemy.material.DOFade(0f, 0.5f).OnComplete(() => Destroy(gameObject));
    }

    public void moveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, 1);
        }
        else
        {
            transform.position = prs.pos;
        }
    }
    public void onBack()
    {
        enemyBack.SetActive(true);
    }
    public void offBack()
    {
        enemyBack.SetActive(false);
    }
    public void onSelect()
    {
        enemySelect.SetActive(true);
        select = true;
    }
    public void offSelect()
    {
        enemySelect.SetActive(false);
        select = false;
    }
    public bool stateSelect()
    {
        return select;
    }
    void OnMouseOver()
    {
        CardManager.Inst.EnemyMouseOver(this);
    }
    void OnMouseExit()
    {
        CardManager.Inst.EnemyMouseExit(this);
    }
    void OnMouseUp()
    {
        if(!die)
            CardManager.Inst.EnemyMouseUp(this);
    }
}
