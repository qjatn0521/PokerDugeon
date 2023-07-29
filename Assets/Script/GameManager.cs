using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

//차트,UI,랭킹,게임오버
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;
    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] GameObject cardInfo;
    [SerializeField] GameObject cardChange;
    [SerializeField] GameObject turnEnd;
    [SerializeField] GameObject upTitle;
    [SerializeField] GameObject hp;
 
    [SerializeField] SpriteRenderer background;
    [SerializeField] List<Sprite> BGList;

    [SerializeField] GameObject dialog;
    [SerializeField] TextMeshPro startText;

    [SerializeField] EnemyList enemyList;
    [SerializeField] List<EnemyItem> enemys;
    private int stage = 0;
    private int floor = 1;

    void Start()
    {
        OffActive();
        StartCoroutine("ChangeBackground",0);
        StartCoroutine("Typing",new string[] { 
            "Hello... You have been invited to a game where your life is at stake.",
            "I hope you have poker skills to succeed.",
            "It would be nice to see you again.",
            "Well then, let the game begin!"});
        
    }
    void OnActive()
    {
        cardInfo.SetActive(true);
        cardChange.SetActive(true);
        turnEnd.SetActive(true);
        hp.SetActive(true);
    }
    void OffActive()
    {
        cardInfo.SetActive(false);
        cardChange.SetActive(false);
        turnEnd.SetActive(false);
        hp.SetActive(false);
    }
    public IEnumerator ChangeBackground(int i)
    {
        Sprite selectedSprite = BGList[i];
        background.sprite = selectedSprite;
        StartCoroutine("FadeInBackground");
        yield return null;
    }
    public IEnumerator FadeInBackground()
    {
        for(float i=1;i<256;i++)
        {
            background.color = new Color(i/255f, i/255f, i/255f, 1);
            yield return new WaitForSeconds(0.15f/i);
        }
        yield return null;
    }
    public IEnumerator StartBattle(int diff)
    {
        OnActive();
        stage++;
        int rand;
        if (floor ==1)
        {
            rand = Random.Range(1,4);       //1부터 3까지
            EnemyItem tmp;
            if (rand == 1)       //슬라임
            {
                if (diff== 1)
                {
                    tmp = enemyList.enemyItems[0];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                } else if(diff ==2)
                {
                    tmp = enemyList.enemyItems[0];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                } else if(diff ==3)
                {
                    tmp = enemyList.enemyItems[1];
                    enemys.Add(tmp);
                    tmp = enemyList.enemyItems[0];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                } else if(diff ==4)
                {

                    tmp = enemyList.enemyItems[1];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    tmp = enemyList.enemyItems[0];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                } else if(diff == 5)
                {
                    tmp = enemyList.enemyItems[1];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                }
            } else if(rand == 2)//고블린
            {
                if (diff == 1)
                {
                    tmp = enemyList.enemyItems[2];
                    enemys.Add(tmp);
                } else if(diff == 2)
                {
                    tmp = enemyList.enemyItems[2];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                } else if(diff == 3)
                {
                    tmp = enemyList.enemyItems[3];
                    enemys.Add(tmp);
                    tmp = enemyList.enemyItems[2];
                    enemys.Add(tmp);
                } else if(diff == 4)
                {
                    tmp = enemyList.enemyItems[3];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                } else if(diff == 5)
                {
                    tmp = enemyList.enemyItems[3];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    tmp = enemyList.enemyItems[2];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                }
            } else if(rand==3)  //오크
            {
                if (diff == 1)
                {
                    tmp = enemyList.enemyItems[4];
                    enemys.Add(tmp);
                }
                else if (diff == 2)
                {
                    tmp = enemyList.enemyItems[4];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                }
                else if (diff == 3)
                {
                    tmp = enemyList.enemyItems[4];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                }
                else if (diff == 4)
                {
                    tmp = enemyList.enemyItems[5];
                    enemys.Add(tmp);
                    tmp = enemyList.enemyItems[4];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                }
                else if (diff == 5)
                {
                    tmp = enemyList.enemyItems[5];
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                    enemys.Add(tmp);
                }
            }
        } else if(floor ==2) { 
        
        } else if(floor ==3)
        {

        }
        StartCoroutine(CardManager.Inst.SpawnEnemys(enemys));
        StartCoroutine(CardManager.Inst.StartBattleCo(7, 5));
        yield return null;
    }
    public IEnumerator SelectEquipment()
    {
        OnActive();
        StartCoroutine(CardManager.Inst.SpawnEquipment());
        yield return null;
    }
    public IEnumerator SelectRoad()
    {
        OffActive();
        StartCoroutine(CardManager.Inst.SelecetRoads());
        yield return null;
    }

    public IEnumerator Typing(string[] talk)
    {
        
        yield return new WaitForSeconds(2.0f);
        //dialog.SetActive(true);
        /*
        
        for (int i = 0; i < talk.Length; i++)
        {
            startText.text = "\"";
            for (int j = 0; j < talk[i].Length; j++)
            {
                startText.text += talk[i][j];
                yield return new WaitForSeconds(0.07f);
            }
            startText.text += "\"";
            yield return new WaitForSeconds(1.0f);
        }
        
        yield return new WaitForSeconds(2.0f);
        startText.text = "";
        */
        StartCoroutine(ChangeBackground(1));
        StartCoroutine(SelectRoad());
    }
}

