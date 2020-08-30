using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//神のアクション全般を管理します。
public  class Oracle : MonoBehaviour
{
    public float faithPoint;//信仰ポイント。信仰という意味。
    [SerializeField] Text faithPointText;
    [SerializeField] Button miracleButton;
    [SerializeField] Animator faithButtonAnimator;

    public float INNGeneratePoint;
    public float HouseGeneratePoint;
    public float ChurchGeneratePoint;
    public float BlacksmithGeneratePoint;
    public float ThunderboltPoint;
    //[SerializeField] Button thunderbolt;
    // Start is called before the first frame update
    EnemyPrefab targetEnemyPrefab;
    public enum MODE_TYPE//今何モード？破壊？生成？ニュートラル？
    {
        Neutral,
        Destruction,
        SummonDemon,
        Battle,
        Construction,
    }
   public  MODE_TYPE type = MODE_TYPE.Neutral;
    [SerializeField] Text modeText;//上のモードを表示する。
    [SerializeField] float DestructionPoint = 10; //障害物を消すために必要なポイント。これはあとで消したり変えたりするかも。オブジェクト毎に設定したほうがいい気がしている。
    [SerializeField] GameObject SummonDemonParent;//悪魔召喚のウィンドウ。
    [SerializeField] GameObject ConstructionParent;//建設ウィンドウ
    [SerializeField] GameObject EnemySlillWindow;//敵をクリックしたときに表示するスキルウィンドウ
    Transform enemyWindowChild;//敵のウィンドウのパネル。
    List<Transform> Texts = new List<Transform>();//敵のスキルウィンドウの子供であるパネルの更に子供達。HPとか名前とかのテキストを保持
    TextMeshProUGUI TextsHP;//上のテキストのHPの部分のみ
    private void Awake()
    {
        faithPointText.text = "信仰P:"+faithPoint.ToString();
        enemyWindowChild = EnemySlillWindow.transform.GetChild(0);
        Debug.Log(enemyWindowChild);
   
        foreach (Transform child in enemyWindowChild)
        {
            Texts.Add(child.transform);
            Debug.Log(child.name);
        }
        TextsHP = Texts[2].GetComponent<TextMeshProUGUI>();
        Debug.Log(TextsHP);
    }
    void Start()
    {
     
    }

    // Update is called once per frame
    GameObject clickedGameObject;//クリックしたオブジェクト
    void Update()
    {
        faithPointText.text = "信仰P:" + faithPoint.ToString();
      
        /*  if(ThunderboltFlag == true && faithPoint>= ThunderboltPoint) { if (Input.GetMouseButtonDown(0))
              {
                  clickedGameObject = null;

                  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  RaycastHit hit = new RaycastHit();

                  if (Physics.Raycast(ray, out hit))
                  {
                      if (hit.transform.tag == "Enemy" || hit.transform.tag == "Building" || hit.transform.tag == "Village")
                      {
                          clickedGameObject = hit.collider.gameObject;
                          Instantiate(thunderboltParticle, clickedGameObject.transform.position, Quaternion.identity);
                          Destroy(clickedGameObject);
                          ThunderboltFlag = false;
                          faithPoint -= ThunderboltPoint;
                      }
                  }
              } }*/
        if (type == MODE_TYPE.Neutral) { modeText.text = "通常モード"; SummonDemonParent.SetActive(false); ConstructionParent.SetActive(false); }
        else if(type == MODE_TYPE.SummonDemon) { modeText.text = "魔物召喚モード"; SummonDemonParent.SetActive(true); } 
        else if(type == MODE_TYPE.Destruction){ modeText.text = "破壊モード"; if (Input.GetMouseButtonDown(0)) 
            { if (ClickIvent().tag == "Obstacle") { if (faithPoint >= DestructionPoint) 
                    { DecreaseFaith( DestructionPoint); Instantiate(thunderboltParticle, clickedGameObject.transform.position, Quaternion.identity); Destroy(clickedGameObject); } } } }//破壊モードの時、建物をクリックしたら破壊。
        else if(type == MODE_TYPE.Construction) { modeText.text = "建設モード"; ConstructionParent.SetActive(true); }
        else if(type == MODE_TYPE.Battle) 
        {
            modeText.text = "戦闘モード";
            if (targetEnemyPrefab)
            {
                TextsHP.text = "HP:" + targetEnemyPrefab.HP + "/" + targetEnemyPrefab.MaxHP;
            }
            if (Input.GetMouseButtonDown(0))
            {
                GameObject target = ClickIvent(); 
                if(!(target == null))
                {  
                    if (target.tag == "Enemy") 
                    { 
                        targetEnemyPrefab = target.GetComponent<EnemyPrefab>(); ClickEnemy(); 
                    } 
                }
            }
        }//バトルモード時、敵をクリックするとスキルウィンドウ表示
        


        if (Input.GetKeyDown(KeyCode.X)) { GetFaith(100); }
    }
    public void OnClickMiracleButton()
    {
        if (faithButtonAnimator.GetBool("Open") == false) { faithButtonAnimator.SetBool("Open", true); } else { faithButtonAnimator.SetBool("Open", false); }
    }
    public void GetFaith(float faith)
    {
        faithPoint += faith;
    }
    public void DecreaseFaith(float faith)
    {
        faithPoint -= faith;
    }


    bool ThunderboltFlag;
    [SerializeField] GameObject thunderboltParticle;
    public void Thunderbolt()//神の怒り。オブジェクトを消す。
    {
        if (ThunderboltFlag) { ThunderboltFlag = false; } else { ThunderboltFlag = true; }
        
    }
    public void OnClickNeutralButton()//破壊モードや生成モードを普通モードにするボタン。見た目は×ボタン。 
    {
        type = MODE_TYPE.Neutral;
    }
    public void OnClickDestructionButoon()//破壊。施設などを破壊するボタン。
    {
        type = MODE_TYPE.Destruction;
    }
    
    public void OnClickSummonDemonButton()
    {
        type = MODE_TYPE.SummonDemon;
    }
    public void OnClickConstructionButton()//建設モード
    {
        type = MODE_TYPE.Construction;
    }
    public GameObject ClickIvent()//クリックしたオブジェクトを認識してやろうってことよ
    {
       

      clickedGameObject = null;

         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//タグMainCameraにした時のみ機能するから注意ね。
         RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
            }

            Debug.Log(clickedGameObject);
        return clickedGameObject;
    }
    public void EnemySkill1ButtonClick()//魔物の一つ目のスキルボタンクリック
    {
        targetEnemyPrefab.Skill1();
    }
    public void EnemySkill2ButtonClick()//魔物の2つ目のスキルボタンクリック
    {
        targetEnemyPrefab.Skill2();

    }
    public void BattleStart()//バトル開始ボタンを押したとき。今のところ仮で入れてる。
    {
        type = MODE_TYPE.Battle;
    }
    public void ClickEnemy()//ClickIbent()で魔物がクリックされたと判定された時、その魔物の詳細を表示するなど。
    {
        EnemySlillWindow.SetActive(true);
        Texts[0].GetComponent<TextMeshProUGUI>().text = targetEnemyPrefab.name;
        Texts[1].GetComponent<TextMeshProUGUI>().text = "LV:"+targetEnemyPrefab.LV;
        TextsHP.text = "HP:" + targetEnemyPrefab.HP + "/" + targetEnemyPrefab.MaxHP;

        Texts[3].GetComponent<TextMeshProUGUI>().text = "Exp:"+targetEnemyPrefab.EXP;
        Texts[4].GetComponent<TextMeshProUGUI>().text = "Gold:"+targetEnemyPrefab.money;
        Texts[5].GetComponent<TextMeshProUGUI>().text = "ATK:"+targetEnemyPrefab.ATK;
        Texts[6].GetComponent<TextMeshProUGUI>().text = "Range:"+targetEnemyPrefab.range;
        Texts[7].GetComponent<TextMeshProUGUI>().text = "MS:" + targetEnemyPrefab.MovementSpeed;


    }
}
