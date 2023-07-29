using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Road : MonoBehaviour
{
    [SerializeField] SpriteRenderer Image;
    private int option;
    private int per;
    private int difficulty=1;
    private bool select = false;
    private bool position = false;
    private Transform removePoint;
    private bool ready = false;
    public List<int> enemyList;
    private bool die = false;
    
    public PRS originPRS;
    [SerializeField] GameObject effect;

    public int getOption() { return option; }
    public bool Front() { return position; }
    public void Setup(RoadItem roadItem, Transform removePoint)
    {
        this.option = roadItem.option;
        this.per = roadItem.per;
        this.removePoint = removePoint;
        Image.sprite = roadItem.sprite;
        
    }
    public void setSelect(int difficulty)
    {
        this.difficulty = difficulty;
        select = true;
    }
    public void onEffect()
    {
        effect.SetActive(true);
    }
    public void offEffect()
    {
        effect.SetActive(false);
    }
    public void rotate()
    {
        transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
        position = true;
    }
    public void DieRoad()
    {
        die = true;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -45f);

        // 이동하고 회전하는 애니메이션을 실행합니다.
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(targetRotation.eulerAngles, 1f))
            .Join(transform.DOLocalMove(removePoint.position, 1f))
            .OnComplete(() => Destroy(gameObject));
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime=0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
        }
    }
    void OnMouseOver()
    {
        CardManager.Inst.RoadMouseOver(this);
    }
    void OnMouseExit()
    {
        CardManager.Inst.RoadMouseExit(this);
    }
    void OnMouseDown()
    {
        if(!die)
        {
            if (!select)
            {
                if (!position)
                    CardManager.Inst.RoadMouseDown1(this);
            }
            else
                CardManager.Inst.RoadMouseDown2(option, difficulty);
        }
        
    }
}
