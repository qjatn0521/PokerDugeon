using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] GameManager gameManager;

    [SerializeField] List<TitleCard> myTitleCards;
    [SerializeField] ECardState eCardState;

    TitleCard selectTitleCard;
    bool onMyCardArea;

    bool isMyCardDrag = false;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }
    void Start()
    {
        StartCoroutine("GoCardAlignment", 0.2f);
        SetOriginOrder();
        eCardState = ECardState.CanMouseDrag;
    }
    void Update()
    {
        if (isMyCardDrag)
            CardDrag();
        DetectCardArea();
    }
    private IEnumerator GoCardAlignment(float waitTime)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, 5, 0.5f, Vector3.one * 1.0f);
        for (int i = 0; i < 5; i++)
        {
            myTitleCards[i].originPRS = originCardPRSs[i];
            myTitleCards[i].MoveTransform(myTitleCards[i].originPRS, true, 0.7f);
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitForSeconds(1f);
    }
    private void SetOriginOrder()
    {
        for (int i = 0; i < myTitleCards.Count; i++)
        {
            myTitleCards[i].GetComponent<Order>().SetOriginOrder(i);
        }
    }//카드의 Order 설정
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        float interval = 1f / (objCount - 1);
        for (int i = 0; i < objCount; i++)
        {
            objLerps[i] = interval * i;
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;

            float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));

            if (height > 0)
            {
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }

            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }
    void EnlargeCard(bool isEnlarge, TitleCard titleCard)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(titleCard.originPRS.pos.x, -3f, -10f);
            titleCard.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.2f), false);
        }
        else
            titleCard.MoveTransform(titleCard.originPRS, false);

        titleCard.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }
    void SetECardState(int state)
    {

        if (state == 0)
            eCardState = ECardState.Nothing;
        else if (state == 1)
            eCardState = ECardState.CanMouseOver;
        else if (state == 2)
            eCardState = ECardState.CanMouseDrag;

    }
    void CardDrag()
    {
        if (!onMyCardArea)
        {
            selectTitleCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectTitleCard.originPRS.scale), false);
        }
        else
        {
            selectTitleCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, Vector3.one * 1.2f), false);
        }
    }
    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }
    #region MyCard
    public void titleCardMouseOver(TitleCard titleCard)
    {
        if (eCardState == ECardState.Nothing)
            return;
        selectTitleCard = titleCard;
        EnlargeCard(true, titleCard);

    }

    public void titleCardMouseExit(TitleCard titleCard)
    {
        EnlargeCard(false, titleCard);
    }
    public void titleCardMouseDown()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;
        isMyCardDrag = true;
    }
    public void titleCardMouseUp(TitleCard titleCard)
    {
        isMyCardDrag = false;
        if (eCardState != ECardState.CanMouseDrag|| onMyCardArea)
            return;
        if(titleCard.getOption() == 0)
        {
            SceneManager.LoadScene("SampleScene");
        } else if(titleCard.getOption() == 1)
        {

        } else if (titleCard.getOption() == 2)
        {

        } else if (titleCard.getOption() == 3)
        {

        }
        else if (titleCard.getOption() == 4)
        {
            Application.Quit();
        }
        //추가 예정
    }
    

    #endregion
}
