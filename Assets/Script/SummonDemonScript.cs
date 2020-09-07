using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
public class SummonDemonScript : MonoBehaviour
{
    public Text DemonsText;//召喚した魔物たちのリスト
    public Dropdown demonNameDropdown;
    public Dropdown demonLVDropdown;
    public InputField moneyInputField;//金額入力
    public float money;
    public Text requiredPointsText;//必要ポイントテキスト
   // public Button summonButton;
    float requiredPointDate;//enemyDateのポイントを読み取る用。
    float requiredPoint;//本物。最後に生産される正しいポイント。
   public int ID;
    int LV;
    [SerializeField] List<EnemyDate> enemyDates;
//    [SerializeField] EnemyDate ID1enemy;
//    [SerializeField] EnemyDate ID2enemy;
    [SerializeField] Oracle oracle;

    [SerializeField] GameObject enemy1Prefab;
    [SerializeField] GameObject enemy2Prefab;
    [SerializeField] GameObject enemy3Prefab;

    [SerializeField] GameObject groundGameObject;
    public EnemyBabyDate[] enemyList;
    //   public List<EnemyBabyDate> enemyList;//敵のレベルやIDを入れて、召喚時に消す。
    public GameObject EnemySlot;//表示するやつ。
    [SerializeField] Transform enemySlotParent;
    [SerializeField] Text averageGoldText;//平均所持金
    WorldController worldController;
    [SerializeField] GameObject gridParent;
    //public GameObject[] slots;
     public  List<GameObject> slots;
    [SerializeField] TextMeshProUGUI HPtext;
    [SerializeField] TextMeshProUGUI ATKtext;
    [SerializeField] TextMeshProUGUI Rangetext;
    [SerializeField] TextMeshProUGUI MStext;
    [SerializeField] TextMeshProUGUI EXPtext;
    public int targetSlot;//どのグリッドを選択しているか
    public  List<Transform> positions;
    public Vector3 bossPosition;//ボスがいる玉座の位置。

    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    { 
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        enemyList = new EnemyBabyDate[25];
     for (int i = 0; i < slots.Count; i++)
        {
            Debug.Log(i);
            //enemyList.Add(slots[i].GetComponent<EnemyBabyDate>());
            enemyList[i] = slots[i].GetComponent<EnemyBabyDate>();
        }
        SelectDemon();
        SelectLV();
        
       
    }
    public void aaa()
    {
   
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void SelectDemon()
    {
        switch (demonNameDropdown.value)
        {
            case 0:
                Debug.Log("壺");
                ID = 0;
                break;
            case 1:
                Debug.Log("袋");
                ID = 1;
                break;
            case 2:
                ID = 2;
                break;
        }
        UIUpdate();

    }
    public void SelectLV()
    {
        switch (demonLVDropdown.value)
        {
            case 0:
                LV =1;
                break;
            case 1:
                LV = 2;
                break;
            case 2:
                LV = 3;
                break;
            case 3:
                LV = 4;
                break;
            case 4:
                LV = 5;
                break;

        }
        UIUpdate();

    }
    public void MoneyInput()
    {
        float aaa = 0;
       
            if (float.TryParse(moneyInputField.text, out aaa)) { money = float.Parse(moneyInputField.text); }
            else { money = 0; }
        
        UIUpdate();
    }
    public void UIUpdate()
    {
        switch (ID)
        {
            case 0:
                //requiredPointDate = ID1enemy.requiredPoint;
                requiredPointDate = enemyDates[0].requiredPoint;

                break;
            case 1:
                //requiredPointDate = ID2enemy.requiredPoint;
                requiredPointDate = enemyDates[1].requiredPoint;

                break;
            case 2:
                requiredPointDate = enemyDates[2].requiredPoint;
                break;
        }
        requiredPoint = requiredPointDate * LV + money;
        requiredPointsText.text = "必要信仰P:"+requiredPoint.ToString();
        averageGoldText.text = "平均所持金:" + worldController.averageGlod[ID].ToString();
        HPtext.text = "HP:"+enemyDates[enemyList[targetSlot].ID].HP.ToString();
        ATKtext.text = "ATK:"+enemyDates[enemyList[targetSlot].ID].ATK.ToString();
        Rangetext.text ="Range:"+ enemyDates[enemyList[targetSlot].ID].Range.ToString();
        MStext.text = "MStext:"+enemyDates[enemyList[targetSlot].ID].MovementSpeed.ToString();
        EXPtext.text = "EXPtext:"+enemyDates[enemyList[targetSlot].ID].exp.ToString();
    

    }
    public void AddButtonClick()
    {
        if(oracle.faithPoint >= requiredPoint)
        { 
            oracle.DecreaseFaith(requiredPoint);
            //GameObject enemy;
            //enemy = Instantiate(EnemySlot, enemySlotParent); 
            //enemyList.Add(enemy.GetComponent<EnemyBabyDate>());
            enemyList[targetSlot].ID = ID;
            enemyList[targetSlot].LV = LV;
            enemyList[targetSlot].Gold = money;
            

            /* enemyList[enemyList.Count-1].ID = ID;
             enemyList[enemyList.Count-1].LV = LV;
             enemyList[enemyList.Count-1].Gold = money;
            */
            var image = slots[targetSlot].transform.GetChild(0).GetComponent<Image>();
            image.sprite = enemyDates[enemyList[targetSlot].ID].image;
            image.gameObject.SetActive(true);
            UIUpdate();
            
        }
        else { requiredPointsText.text = "信仰Pが足りない"; }
    }

    GameObject obj;
    public void SummonDemon()
    {

        for (int i=0; i < enemyList.Length; ++i)
        {
            //Vector3 vector3;
            //vector3 = new Vector3(groundGameObject.transform.position.x + Random.Range(-20, 20),
            //   groundGameObject.transform.position.y + 3, groundGameObject.transform.position.z + Random.Range(-20, 20));

            Vector3 vector3 = positions[i].position;

            switch (enemyList[i].ID)
            {
                case 0:
                    obj = Instantiate(enemy1Prefab, vector3, Quaternion.identity);
                 

                    break;

                case 1:
                    obj = Instantiate(enemy2Prefab, vector3, Quaternion.identity);
                   
                    break;
                case 2:
                    obj = Instantiate(enemy3Prefab, vector3, Quaternion.identity);
                    break;
            }
            Debug.Log(obj);
            if (!(obj == null))
            {
                var enemyPrefab = obj.GetComponent<EnemyPrefab>();
                enemyPrefab.GetDate(enemyList[i].LV, enemyList[i].Gold);
            }
            obj = null;
            //Destroy(enemyList[i].gameObject);
            enemyList[i].ResetDate();
            slots[i].transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
        }
      //  enemyList.Clear();

        enemyList = new EnemyBabyDate[25];

        for (int i = 0; i < slots.Count; i++)//エネミーリストが消えるので、もう一度読み取る。
        {            
            enemyList[i] = slots[i].GetComponent<EnemyBabyDate>();
        }
        oracle.BattleStart();
    }
    public void GridButtonClick(int aaa)
    {
        targetSlot = aaa;
        Debug.Log("targetSlot " + targetSlot);
        UIUpdate();
       //slots[aaa].transform.GetChild(0).GetComponent<Image>().sprite = 
    }
    public void BossSummonDemon()
    {
      
        int[] bossSummonList;
        bossSummonList = new int[Random.Range(2, 4)];
        Debug.Log("リストの長さ" + bossSummonList.Length);
        
        for(int i = 0; i < bossSummonList.Length; ++i)
        {
            int random = Random.Range(0, 3);
            Vector3 vector3 = positions[Random.Range(0, positions.Count )].position;

            switch (random)
            {
                case 0:
                    obj = Instantiate(enemy1Prefab, vector3, Quaternion.identity);
                    break;

                case 1:
                    obj = Instantiate(enemy2Prefab, vector3, Quaternion.identity);

                    break;
                case 2:
                    obj = Instantiate(enemy3Prefab, vector3, Quaternion.identity);
                    break;
            }
        }

      
    }
}
