using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TitleCard : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] int option;
    [SerializeField] GameObject effect;
    public PRS originPRS;

    public int getOption() { return option; }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {

            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    void OnMouseOver()
    {
        effect.GetComponent<SpriteRenderer>().sortingOrder = card.sortingOrder-1;
        effect.SetActive(true);
        TitleManager.Inst.titleCardMouseOver(this);
    }
    void OnMouseExit()
    {
        effect.SetActive(false);
        TitleManager.Inst.titleCardMouseExit(this);
    }
    void OnMouseDown()
    {
        TitleManager.Inst.titleCardMouseDown();
    }
    void OnMouseUp()
    {
        TitleManager.Inst.titleCardMouseUp(this);
    }
}
