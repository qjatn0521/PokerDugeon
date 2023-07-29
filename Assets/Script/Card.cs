using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    public CardItem cardItem;
    private Transform removePoint;
    public PRS originPRS;

    public void Setup(CardItem cardItem, Transform removePoint)
    {
        this.cardItem = cardItem;
        this.removePoint = removePoint;
        card.sprite = cardItem.sprite;
    }
    public void DieCard()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 45f);

        // 이동하고 회전하는 애니메이션을 실행합니다.
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(targetRotation.eulerAngles, 1f))
            .Join(transform.DOLocalMove(removePoint.position, 0.8f))
            .OnComplete(() => Destroy(gameObject));
    }
    public CardItem GetCard()
    {
        return cardItem;
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if(useDotween)
        {
           transform.DOMove(prs.pos, dotweenTime);
           transform.DORotateQuaternion(prs.rot, dotweenTime);
           transform.DOScale(prs.scale, dotweenTime);
        } else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }
    void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }
    void OnMouseDown()
    {
        CardManager.Inst.CardMouseDown();
    }
    void OnMouseUp()
    {
        CardManager.Inst.CardMouseUp(this);
    }

}
