using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;

using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] CardList cardList;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform cardRemovePoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] List<Card> myCards;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnPoint;
    [SerializeField] Transform enemyRemovePoint;
    [SerializeField] Transform[] myEnemyPosition= new Transform[5];
    private PRS[] PositionPRS = new PRS[5];
    [SerializeField] List<Enemy> myEnemys;
    [SerializeField] List<Enemy> selectedEnemys;

    [SerializeField] RoadList roadList;
    [SerializeField] GameObject roadPrefab;
    [SerializeField] List<Road> myRoads;

    [SerializeField] EquipmentList equipmentItems;
    [SerializeField] GameObject equipPrefab;
    [SerializeField] List<Equipment> myEquips;

    [SerializeField] Transform[] itemPoint = new Transform[4];

    [SerializeField] TextMeshPro textCardSet;
    [SerializeField] TextMeshPro textCardChange;

    [SerializeField] ECardState eCardState;
    [SerializeField] TextMeshProUGUI textInfo;

    [SerializeField] Player player;

    [SerializeField] Road perInfo;
    [SerializeField] Transform perInfoPoint;
    [SerializeField] TextMeshPro[] PerInfoTexts = new TextMeshPro[4];


    List<CardItem> cardBuffer = new List<CardItem>(52);
    List<EnemyItem> enemyBuffer = new List<EnemyItem>(5); //현존 몬스터 수
    List<RoadItem> roadBuffer = new List<RoadItem>();
    List<EquipmentItem> commonBuffer = new List<EquipmentItem>();
    List<EquipmentItem> rareBuffer = new List<EquipmentItem>();
    List<EquipmentItem> epicBuffer = new List<EquipmentItem>();
    List<EquipmentItem> legendBuffer = new List<EquipmentItem>();

    //상점 아이템
    [SerializeField] GameObject storeItemPrefab;
    [SerializeField] Transform[] itemPositions = new Transform[5];
    [SerializeField] Transform[] potionPositions = new Transform[5];


    [SerializeField] GameObject RoadButton;

    [SerializeField] GameObject GameRewardPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] TextMeshPro GameOverPanelText;
    [SerializeField] TextMeshPro GameRewardCointext;

    [SerializeField] GameObject TurnEndButton;
    [SerializeField] GameObject ShopEndButton;
    [SerializeField] TextMeshProUGUI ShopInfo;
    List<ShopItem> ShopItems = new List<ShopItem>();

    Card selectCard;
    bool isMyCardDrag= false;
    bool onMyCardArea;
    int cardNum;
    
    int damage;
    int maxSelect;
    int numSelect;
    int numEnemy=0;
    int coin = 0;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag}      //0 아무것도 1 손 올리기 2 드래그
    int[] roads = new int[4];

    int boss = 0;

    void Start()
    {
        player.resetChange();
        roadList.roadItems[0].Init(0, 1, 0);
        roadList.roadItems[1].Init(1, 5, 0);
        roadList.roadItems[2].Init(2, 1, 0);
        roadList.roadItems[3].Init(3, 1, 0);
        player.PlusCoin(300);
        for (int i=0;i<5;i++)
        {
            PRS tmpPRS = new PRS(myEnemyPosition[i].position, Quaternion.identity, Vector3.one);
            PositionPRS[i] = tmpPRS;
            
        }

        //아이템 등급별로 버퍼에 넣어두기
        SetupBuffer(commonBuffer, equipmentItems.commonItems);
        SetupBuffer(rareBuffer, equipmentItems.rareItems);
        SetupBuffer(epicBuffer, equipmentItems.epicItems);
        SetupBuffer(legendBuffer, equipmentItems.legendItems);
    }
    private List<T> PopList<T>(int max, List<T> buffer)
    {
        List<T> tmp = new List<T>();
        for (int i = 0; i < max; i++)
        {
            tmp.Add(buffer[i]);
        }
        return tmp;
    }
    private void SetupBuffer<T>(List<T> buffer, T[] items)
    {
        for(int i=0;i<items.Length;i++)
        {
            T item = items[i];
            buffer.Add(item);
        }
        Shuffle(buffer);

    }
    public void ChangeCard(Card card)
    {
        
        if (player.changeNum <= 0)
        {
            return;
        }
        player.useChange();
        SetECardState(0);
        myCards.Remove(card);
        GameObject.Destroy(card.gameObject);
        StartCoroutine("GoCardAlignment",0f);
        GetCardDamage();
    }
    public IEnumerator StartBattleCo(int boss)
    {
        if (boss == 1) this.boss = boss;
        player.resetChange();
        SetECardState(0);       //드래그 가능한 상태
        SetupBuffer(cardBuffer,cardList.cardItems);      
        this.cardNum = player.cardNum; //카드 개수
        SetCard();              //카드 리스트에 cardNum만큼 넣기
        GetCardDamage();
        yield return null;
    }
    public IEnumerator SelecetRoads()
    {
        SetECardState(0);
        for (int i = 0; i < roadList.roadItems.Length; i++)
        {
            RoadItem item = roadList.roadItems[i];
            for(int j=0; j < item.getPer();j++)
            {
                roadBuffer.Add(item);
            }
                
            PerInfoTexts[i].text = item.getPer().ToString();
        }
        
        for (int i = 0; i < roadBuffer.Count; i++)
        {
            int rand = Random.Range(i, roadBuffer.Count);
            RoadItem tmp = roadBuffer[i];
            roadBuffer[i] = roadBuffer[rand];
            roadBuffer[rand] = tmp;
        }
        SetRoad();
        yield return null;

        PRS originCardPRSs = new PRS(perInfoPoint.position, Quaternion.identity, Vector3.one);
        perInfo.transform.DOMove(originCardPRSs.pos, 2.2f).OnComplete(() =>
        {
            SetECardState(2);
            RoadButton.SetActive(true);
        });
    }
    public IEnumerator SpawnEnemys(List<EnemyItem> items)
    {
        numEnemy = items.Count;
        SetECardState(0);
        for (int i = 0; i < items.Count; i++)
        {
            enemyBuffer.Add(items[i]);
        }
        SetEnemy();
        yield return null;
    }
    public IEnumerator SpawnEquipment(int num)
    {
        SetEquipment(num);
        yield return null;
    }
    public IEnumerator SpawnStoreItems(int floor, int diff)
    {
        SetStoreItems(floor,diff);
        yield return null;
    }
    void Update()
    {
        if (isMyCardDrag)
            CardDrag();
        DetectCardArea();
    }

   
    private void SetCard()
    {
        List<CardItem> tmp = PopList(player.cardNum+player.changeNum,cardBuffer);   //총 카드 = cardNum + changeNum
        for (int i=0;i< player.cardNum + player.changeNum; i++)
        {
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            var card = cardObject.GetComponent<Card>();
            myCards.Add(card);
            card.setup(tmp[i],cardRemovePoint);
        }
        StartCoroutine("GoCardAlignment",0.2f);
        SetOriginOrder();

    }       //총 카드 수 만큼 소환 -> GoCardAlignment
    private void SetEnemy()
    {
        List<EnemyItem> tmp = PopList(numEnemy, enemyBuffer);  
        for (int i = 0; i < numEnemy; i++)
        {
            var enemyObject = Instantiate(enemyPrefab, enemySpawnPoint.position, Utils.QI);
            var enemy = enemyObject.GetComponent<Enemy>();
            myEnemys.Add(enemy);
            enemy.setup(tmp[i], enemyRemovePoint);
        }
        int[] pos = new int[] {2};
        if(numEnemy == 1) pos = new int[] {2};
        else if(numEnemy == 2) pos = new int[] {1,3};
        else if(numEnemy == 3) pos = new int[] {1,2,3};
        else if (numEnemy == 4) pos = new int[] { 0,1, 3,4 };
        else if (numEnemy == 5) pos = new int[] { 0,1,2,3,4 };
        StartCoroutine("GoEnemyAlignment", pos);
        //SetOriginOrder();

    }
    private void SetRoad()
    {
        List<RoadItem> tmp = PopList(5, roadBuffer); 
        for (int i = 0; i < 5; i++)
        {
            var roadObject = Instantiate(roadPrefab, enemySpawnPoint.position, Utils.QI);
            var road = roadObject.GetComponent<Road>();
            myRoads.Add(road);
            road.Setup(tmp[i], enemyRemovePoint);
        }
        StartCoroutine("GoRoadAlignment");
    }
    private void SetEquipment(int num)
    {
        int commonPro=80;
        int rarePro=100;
        int epicPro=101;
        int legendPro=101;
        if (num == 2)
        {
            commonPro = 40;
            rarePro = 80;
            epicPro = 100;
            legendPro = 101;
        }
        else if (num == 3)
        {
            commonPro = 0;
            rarePro = 60;
            epicPro = 100;
            legendPro = 101;
        }
        else if (num == 4)
        {
            commonPro = 0;
            rarePro = 20;
            epicPro = 80;
            legendPro = 100;
        }
        else if (num == 5)
        {
            commonPro = 0;
            rarePro = 0;
            epicPro = 40;
            legendPro = 100;
        }
        int commonNum=0;
        int rareNum=0;
        int epicNum=0;
        int legendNum=0;
        for (int i=0;i < player.rewardNum;i++)
        {
            int rand = Random.Range(1, 101);
            if (rand <= commonPro) {
                commonNum++;
            } else if (rand <= rarePro) {
                rareNum++;
            } else if (rand <= epicPro) {
                epicNum++;
            } else if(rand <= legendPro) {
                legendNum++;
            }
        }
        List<EquipmentItem> combinedList = new List<EquipmentItem>();

        combinedList.AddRange(PopList(commonNum, commonBuffer));
        combinedList.AddRange(PopList(rareNum, rareBuffer));
        combinedList.AddRange(PopList(epicNum, epicBuffer));
        combinedList.AddRange(PopList(legendNum, legendBuffer));

        Shuffle(combinedList);

        for (int i = 0; i < player.rewardNum; i++)
        {
            var equipObject = Instantiate(equipPrefab, enemySpawnPoint.position, Utils.QI);
            var equip = equipObject.GetComponent<Equipment>();
            myEquips.Add(equip);
            equip.Setup(combinedList[i]);
        }
        int[] pos = new int[] { 2 };
        if (player.rewardNum == 1) pos = new int[] { 2 };
        else if (player.rewardNum == 2) pos = new int[] { 1, 3 };
        else if (player.rewardNum == 3) pos = new int[] { 1, 2, 3 };
        else if (player.rewardNum == 4) pos = new int[] { 0, 1, 3, 4 };
        else if (player.rewardNum == 5) pos = new int[] { 0, 1, 2, 3, 4 };
        StartCoroutine("GoEquipAlignment",pos);
    }
    private void SetStoreItems(int stage,int diff)
    {
        int commonPro = 70;
        int rarePro = 90;
        int epicPro = 100;
        int legendPro = 101;
        if (stage == 2)
        {
            commonPro = 40;
            rarePro = 70;
            epicPro = 90;
            legendPro = 100;
        }
        else if (stage == 3)
        {
            commonPro = 10;
            rarePro = 40;
            epicPro = 60;
            legendPro = 100;
        }
        int commonNum = 0;
        int rareNum = 0;
        int epicNum = 0;
        int legendNum = 0;
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(1, 101);
            if (rand <= commonPro)
            {
                commonNum++;
            }
            else if (rand <= rarePro)
            {
                rareNum++;
            }
            else if (rand <= epicPro)
            {
                epicNum++;
            }
            else if (rand <= legendPro)
            {
                legendNum++;
            }
        }
        List<EquipmentItem> combinedList = new List<EquipmentItem>();

        combinedList.AddRange(PopList(commonNum, commonBuffer));
        combinedList.AddRange(PopList(rareNum, rareBuffer));
        combinedList.AddRange(PopList(epicNum, epicBuffer));
        combinedList.AddRange(PopList(legendNum, legendBuffer));
        int n = Random.Range(0, 7);
        if (n < 3) stage -= n;
        if (n == 0) stage--;
        Shuffle(combinedList);
        if (stage < 0) stage = 0;
        for(int i = 0; i< stage+1;i++)
        {
            float t = Mathf.Pow(Random.value,4); // 낮은 숫자에 편향되도록 랜덤 값을 제곱
            int rand = Mathf.RoundToInt(Mathf.Lerp(5*diff, 20*diff, t));
            combinedList[i].sale = rand;
        }
        for (int i = stage+1;i  < 4; i++)
        {
            combinedList[i].sale = 0;
        }
        /*
        int[] arr = new int[101];
        for( int i=0;i<10000000;i++)
        {
            float t = Mathf.Pow(Random.value, 3); // 낮은 숫자에 편향되도록 랜덤 값을 제곱
            int rand = Mathf.RoundToInt(Mathf.Lerp(5 * diff, 20 * diff, t));
            arr[rand]++;
        }
        for (int i = 5*diff; i <= 20*diff; i++)
        {
            print("Element " + i + ": " + arr[i]);
        }*/
        Shuffle(combinedList);

        for (int i = 0; i < 4; i++)
        {
            var equipObject = Instantiate(storeItemPrefab, itemPositions[i].position, Utils.QI);
            var equip = equipObject.GetComponent<ShopItem>();
            ShopItems.Add(equip);
            equip.Setup(combinedList[i]);
        }

        for (int i = 0; i < 5; i++)
        {
            var equipObject = Instantiate(storeItemPrefab, potionPositions[i].position, Utils.QI);
            var equip = equipObject.GetComponent<ShopItem>();
            ShopItems.Add(equip);
            equip.Setup(i);
            float t = Mathf.Pow(Random.value, 4); // 낮은 숫자에 편향되도록 랜덤 값을 제곱
            int rand = Mathf.RoundToInt(Mathf.Lerp(5 * diff, 20 * diff, t));
            if (n==1)
            {
                if(i==3) equip.SetSale(rand);
            } else if(n==2)
            {
                if (i == 3 || i == 4) equip.SetSale(rand);
            } else if(n==0)
            {
                if (i==4) equip.SetSale(rand);
            }
        }
        ShopEndButton.SetActive(true);
    }
    private void SetOriginOrder()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].GetComponent<Order>().SetOriginOrder(i);
        }
    }//카드의 Order 설정
    private IEnumerator GoCardAlignment(float waitTime)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, cardNum, 0.5f, Vector3.one * 0.4f);
        for (int i = 0; i < cardNum; i++)
        {
            myCards[i].originPRS = originCardPRSs[i];
            myCards[i].moveTransform(myCards[i].originPRS, true, 0.7f);
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitForSeconds(1f);
        
        if (player.curchangeNum <= 0)
            SetECardState(1);
        else 
            SetECardState(2);
    }
    private IEnumerator GoEnemyAlignment(int[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            myEnemys[i].originPRS = PositionPRS[pos[i]];
            myEnemys[i].moveTransform(myEnemys[i].originPRS, true, 0.7f);
        }
        yield return null;

    }
    private IEnumerator GoRoadAlignment()
    {
        for (int i = 0; i < 5; i++)
        {
            myRoads[i].originPRS = PositionPRS[i];
            myRoads[i].MoveTransform(myRoads[i].originPRS, true, 1f);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
        
    }
    private IEnumerator GoEquipAlignment(int[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            myEquips[i].originPRS = PositionPRS[pos[i]];
            myEquips[i].MoveTransform(myEquips[i].originPRS, true, 0.7f);
        }
        yield return null;
    }
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
            
            if(height>0)
            {
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
                
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }
    public void roadBtn()
    {
        int max = roads[0];
        for (int i = 0; i < 3; i++)
        {
            if (roads[i + 1] > max) max = roads[i + 1];
        }
        List<int> tmp = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (roads[i]==max)
            {
                tmp.Add(i);
            }
        }
        for (int i = 0; i < myRoads.Count; i++)
        {
            if (!myRoads[i].Front())
                myRoads[i].DieRoad();
            bool isMax=false;
            for(int j=0;j<tmp.Count; j++)
            {
                if (myRoads[i].getOption() == tmp[j])
                    isMax = true;
            }
            if(isMax)
                myRoads[i].setSelect(max);
            else
                myRoads[i].DieRoad();
        }
        RoadButton.SetActive(false);
        
    }
    private void GetCardDamage()
    {
        int []value = new int[13];
        int []newValue = new int[13];
        int []shape = new int[4];
        CardItem tmp;
        string set="";
        for (int i = 0; i < cardNum; i++)
        {
            tmp = myCards[i].getCard();
            if (tmp.shape == "spade") shape[0]++;
            else if (tmp.shape == "diamond") shape[1]++;
            else if (tmp.shape == "heart") shape[2]++;
            else if (tmp.shape == "clover") shape[3]++;
            value[tmp.value - 1]++;
            newValue[tmp.value - 1]++;
        }
        
        Array.Sort(newValue);
        Array.Reverse(newValue);
        Array.Sort(shape);
        Array.Reverse(shape);
        if (newValue[0] == 4)
        {
            set = "Foker - 4마리 10배 데미지";
            damage = player.GetATK() * 10;
            textCardSet.text = set;
            maxSelect = 4;
            resetSelect();
            return;
        }
        if (newValue[0] == 3 && (newValue[1] ==3 || newValue[1] == 2))
        {
            set = "Full House - 5마리 6배 데미지";
            damage = player.GetATK() * 6;
            textCardSet.text = set;
            maxSelect = 5;
            resetSelect();
            return;
        }
        
        if (shape[0] == 5)
        {
            set = "Flush";
            damage = player.GetATK() * 7;
            textCardSet.text = set;
            maxSelect = 3;
            resetSelect();
            return;
        }
        for (int i = 0; i < 9; i++)
        {
            if (value[i] >0&& value[i+1]>0 && value[i + 2] > 0 && value[i + 3] > 0 && value[i + 4] > 0)
            {
                set = "Straight";
                damage = player.GetATK() * 5;
                textCardSet.text = set;
                maxSelect = 1;
                resetSelect();
                return;
            }
        }
        if (newValue[0]==3)
        {
            set = "Triple";
            damage = player.GetATK() * 2;
            textCardSet.text = set;
            maxSelect = 3;
            resetSelect();
            return;
        }
        if (newValue[0] == 2 && newValue[1] == 2)
        {
            set = "Two Pair";
            damage = player.GetATK() * 2;
            textCardSet.text = set;
            maxSelect = 2;
            resetSelect();
            return;
        }
        if (newValue[0] == 2)
        {
            set = "One Pair";
            damage = player.GetATK() * 2;
            textCardSet.text = set;
            maxSelect = 1;
            resetSelect();
            return;
        }
        set = "Top";
        damage = player.GetATK();
        textCardSet.text = set;
        maxSelect = 1;
        resetSelect();
        return;

    }
    public void resetSelect()
    {
        while (numSelect > maxSelect)
        {
            numSelect--;
            selectedEnemys[numSelect].offSelect();
            selectedEnemys.RemoveAt(numSelect);
        }
        if ((maxSelect == numSelect || maxSelect > numEnemy)) textInfo.text = "";
    }
    private void EnemyAlignment(Enemy target, PRS originCardPRS)
    {
        target.originPRS = originCardPRS;
        target.moveTransform(target.originPRS, true, 0.7f);
    }
    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -3f, -10f);
            card.moveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 0.5f), false);
        }
        else
            card.moveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }
    
    void CardDrag()
    {
        if(!onMyCardArea)
        {
            selectCard.moveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        } else
        {
            selectCard.moveTransform(new PRS(Utils.MousePos, Utils.QI, Vector3.one * 0.5f), false);
        }
    }
    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }
    public void MyTurnEnd()
    {
         StartCoroutine("AttakTurn");
        TurnEndButton.SetActive(false);
    }
    public void MyShopEnd()
    {
        for (int i = 0; i < ShopItems.Count; i++)
        {
            ShopItems[i].DieEquip();            
        }
        ShopItems.Clear();
        StartCoroutine(GameManager.Inst.ChangeBackground(1));
        StartCoroutine(GameManager.Inst.SelectRoad());
        ShopEndButton.SetActive(false);
    }
    public IEnumerator AttakTurn()
    {
        if(maxSelect == numSelect || maxSelect > numEnemy)
        {
            SetECardState(0);
            int selectedEnemysNum = selectedEnemys.Count;
            for(int i=0; i< selectedEnemysNum;i++)
            {
                selectedEnemys[i].offSelect();
                if(selectedEnemys[i].takeDamage(damage)) numEnemy--;
            }
            selectedEnemys.Clear();
            cardBuffer.Clear();

            for (int i = 0; i < myCards.Count; i++)
            {
                myCards[i].dieCard();
            }
            numSelect = 0;
            myCards.Clear();

            if (numEnemy==0)
            {
                if (boss == 1)
                {
                    GameOverPanelText.text = "Game Clear!";
                    GameOverPanel.GetComponent<SpriteRenderer>().material.DOFade(0f, 0f);
                    GameOverPanel.SetActive(true);
                    GameOverPanel.GetComponent<SpriteRenderer>().material.DOFade(1f, 0.5f);
                } else
                {
                    GameRewardPanel.GetComponent<SpriteRenderer>().material.DOFade(0f, 0f);
                    GameRewardPanel.SetActive(true);
                    GameRewardPanel.GetComponent<SpriteRenderer>().material.DOFade(1f, 0.5f);
                    GameRewardCointext.text = "+" + coin + " coin";

                    player.PlusCoin(coin);
                    coin = 0;
                }
            } else
            {
                yield return new WaitForSeconds(0.3f);
                StartCoroutine("AttakedTurn");
                yield return new WaitForSeconds(0.8f);
                if (player.GetHP() > 0)
                {
                    player.resetChange();
                    SetupBuffer(cardBuffer, cardList.cardItems);
                    SetCard();
                    player.resetChange();
                    GetCardDamage();
                    TurnEndButton.SetActive(true);
                }
                else
                {
                    GameOverPanel.GetComponent<SpriteRenderer>().material.DOFade(0f, 0f);
                    GameOverPanel.SetActive(true);
                    GameOverPanel.GetComponent<SpriteRenderer>().material.DOFade(1f, 0.5f);
                }
            }

        } else
        {
            textInfo.text = "Need More Select";
            yield return null;
        }
    }
    public IEnumerator AttakedTurn()
    {
        for(int i=0; i<myEnemys.Count;i++)
        {
            if (myEnemys[i] != null && myEnemys[i].getHP()>0)
            {
                player.TakeHP(myEnemys[i].attack());
                 
            }
        }
        
        yield return new WaitForSeconds(1.0f);
    }
    
    #region MyCard
    public void CardMouseOver(Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;
        selectCard = card;
        EnlargeCard(true,card);

    }
    
    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }
    public void CardMouseDown()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;
        isMyCardDrag = true;
    }
    public void CardMouseUp(Card card)
    {
        isMyCardDrag = false;
        if (eCardState != ECardState.CanMouseDrag)
            return;
        if (!onMyCardArea)
            ChangeCard(card);
        //추가 예정
    }
    void SetECardState(int state)
    {
        
        if (state ==0)
            eCardState = ECardState.Nothing;
        else if (state == 1)
            eCardState = ECardState.CanMouseOver;
        else if (state == 2)
            eCardState = ECardState.CanMouseDrag;
        
    }
    
    #endregion

    #region MyEnemy
    public void EnemyMouseOver(Enemy e)
    {
        if (eCardState == ECardState.Nothing)
            return;
        e.onBack();
    }
    public void EnemyMouseExit(Enemy e)
    {
        e.offBack();
    }
    public void EnemyMouseUp(Enemy e)
    {
        
        if (e.stateSelect())
        {
            e.offSelect();
            selectedEnemys.Remove(e);
            numSelect--;
        }
        else if (numSelect<maxSelect)
        {
            selectedEnemys.Add(e);
            numSelect++;
            e.onSelect();
            if ((maxSelect == numSelect || maxSelect > myEnemys.Count)) textInfo.text = "";
        } 
    }

    #endregion
    #region MyRoad
    public void RoadMouseOver(Road r)
    {
        r.onEffect();
    }
    public void RoadMouseExit(Road r)
    {
        r.offEffect();
    }
    public void RoadMouseDown1(Road r)
    {
        if (eCardState == ECardState.CanMouseDrag)
        {
            r.rotate();
            roads[r.getOption()]++;
        }
    }
    public void RoadMouseDown2(int option,int difficulty)
    {
        for(int i=0; i<myRoads.Count;i++)
        {
            if (myRoads[i]!=null)
                myRoads[i].DieRoad();
            PRS originCardPRSs = new PRS(cardSpawnPoint.position, Quaternion.identity, Vector3.one);
            perInfo.MoveTransform(originCardPRSs, true, 6.2f);
        }
        roads[0] = 0;
        roads[1] = 0;
        roads[2] = 0;
        roads[3] = 0;
        roadBuffer.Clear();
        myRoads.Clear();
        for (int i = 0; i < roadList.roadItems.Length; i++)
        {
            RoadItem item = roadList.roadItems[i];
            if (item.option == 0)
                item.plusPer++;
            PerInfoTexts[i].text = item.getPer().ToString();
        }
        if (option == 0)    //보스
        {
            StartCoroutine(GameManager.Inst.BossBattle());
        }
        else if (option == 1)   //몬스터
        {
            StartCoroutine(GameManager.Inst.StartBattle(difficulty));
            coin = difficulty * 20;
        }
        else if (option == 2)   //상자
        {
            StartCoroutine(GameManager.Inst.SelectEquipment(difficulty));
        } else if (option == 3) //이벤트
        {
            StartCoroutine(GameManager.Inst.EnterShop(difficulty));
        }

        
    }
    #endregion
    #region MyEquip
    public void EquipMouseDown(Equipment e)
    {
        player.GetEquip(e,null);
        for (int i = 0; i < myEquips.Count; i++)
        {
            myEquips[i].DieEquip();
        }
        removeItem(e.getId());

        Shuffle(commonBuffer);
        Shuffle(rareBuffer);
        Shuffle(epicBuffer);
        Shuffle(legendBuffer);
        myEquips.Clear();
        StartCoroutine(GameManager.Inst.SelectRoad());
    }

    #endregion

    #region MyReward

    public void RewardMouseDown(int rewardCard)
    {
        GameOverPanel.SetActive(false);
        GameRewardPanel.SetActive(false);
        myEnemys.Clear();
        if(rewardCard == 0)
        {
            roadList.roadItems[3].plusPer++;
        } else
        {
            roadList.roadItems[2].plusPer++;
        }
        StartCoroutine(GameManager.Inst.SelectRoad());
    }

    #endregion

    #region Shop

    public bool ShopMouseDown(ShopItem s)
    {
        if(s.GetCoin() <= player.GetCoin())
        {
            if (s.GetOption() == 0)
            {
                player.PlusATK(s.GetPlus());
            }
            else if (s.GetOption() == 1)
            {
                player.PlusDEF(s.GetPlus());
            }
            else if (s.GetOption() == 2)
            {
                player.PlusHP(s.GetPlus());
            }
            else if (s.GetOption() == 3)
            {
                player.Heal(player.GetMaxHP() / 2);
            }
            else if (s.GetOption() == 4)
            {
                player.Heal(player.GetMaxHP() / 4);
            }
            else 
            {
                var equipObject = Instantiate(equipPrefab, enemySpawnPoint.position, Utils.QI);
                var equip = equipObject.GetComponent<Equipment>();
                equip.Setup(s.GetEquip());
                removeItem(equip.getId());
                bool change= player.GetEquip(equip,s);
                Destroy(equipObject);
                if (!change) return false;
                
               
            }
            player.PlusCoin(-1*s.GetCoin());
            return true;
        } else
        {
            ShopInfo.text = "Not Enough coin";
        }
        return false;
        //StartCoroutine(GameManager.Inst.SelectRoad());
    }

    #endregion


    //함수들
    public static void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T tmp = list[i];
            list[i] = list[rand];
            list[rand] = tmp;
        }
    }

    public IEnumerator removeItem(int selectId)
    {
        if (selectId / 100 == 0)
        {
            for (int i = 0; i < commonBuffer.Count; i++)
            {
                if (selectId == commonBuffer[i].id)
                {
                    commonBuffer.Remove(commonBuffer[i]);
                    break;
                }
            }
        }
        else if (selectId / 100 == 1)
        {
            for (int i = 0; i < rareBuffer.Count; i++)
            {
                if (selectId == rareBuffer[i].id)
                {
                    rareBuffer.Remove(rareBuffer[i]);
                    break;
                }
            }
        }
        else if (selectId / 100 == 2)
        {
            for (int i = 0; i < epicBuffer.Count; i++)
            {
                if (selectId == epicBuffer[i].id)
                {
                    epicBuffer.Remove(epicBuffer[i]);
                    break;
                }
            }
        }
        else if (selectId / 100 == 3)
        {
            for (int i = 0; i < legendBuffer.Count; i++)
            {
                if (selectId == legendBuffer[i].id)
                {
                    legendBuffer.Remove(legendBuffer[i]);
                    break;
                }
            }
        }
        yield return null;
    }
    
}
