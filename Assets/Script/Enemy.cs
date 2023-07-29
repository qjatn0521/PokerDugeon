using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemy;
    [SerializeField] GameObject enemyBack;
    [SerializeField] GameObject enemySelect;
    [SerializeField] TextMeshPro textHP;
    [SerializeField] TextMeshPro textDamage;
    public EnemyItem enemyItem;
    public PRS originPRS;
    public int enemyHP;
    public int enemyDamage;
    private Transform removePoint;
    bool select = false;
    private bool die = false;
    public void Setup(EnemyItem enemyItem, Transform removePoint)
    {
        this.enemyItem = enemyItem;
        enemy.sprite = enemyItem.sprite;
        enemyHP = enemyItem.health;
        enemyDamage = enemyItem.damage;
        this.removePoint = removePoint;

        int rand = Random.Range(-enemyHP / 10, enemyHP / 10);
        enemyHP += rand;
        rand = Random.Range(-enemyDamage / 10, enemyDamage / 10);
        enemyDamage += rand;

        textHP.text = enemyHP + "";
        textDamage.text = enemyDamage + "";

        
    }
    public int getHP() { return enemyHP; }
    public bool TakeDamage(int damage)
    {
        enemyHP -= damage;
        if(enemyHP <= 0)
        {
            DieEnemy();
            return true;
        }
        textHP.text = enemyHP + "";
        return false;

    }
    public int Attack()
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
    public void DieEnemy()
    {
        die = true;
        // 투명해지는 애니메이션을 실행합니다.
        enemy.material.DOFade(0f, 0.5f).OnComplete(() => Destroy(gameObject));
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
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
    void OnMouseDown()
    {
        if(!die)
            CardManager.Inst.EnemyMouseDown(this);
    }
}
