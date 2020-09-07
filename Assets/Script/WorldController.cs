using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Scripting;

public class WorldController : MonoBehaviour
{

    //昼か夜かを制御したりする。世界全般のことをするスクリプト。
    public GameObject DirectionalLight;
    public bool night;//夜かどうか。
    [SerializeField] Material nightSkyBox;
    [SerializeField] Material daySkyBox;
    [SerializeField] Material nightFogColor;
    [SerializeField] Material dayFogColor;
    [SerializeField] GameObject nightLight;
    [SerializeField] GameObject dayLight;
    [SerializeField] Image feedObj;
    [SerializeField] Transform HeroHUD;
    [SerializeField] Vector3 position = new Vector3(0,0,0);
    [SerializeField] Text[] texts;
    [SerializeField] LiquidationExp liquidationExp;
    [SerializeField] Text bonusExpText;
    [SerializeField] Button OKButton;

   [SerializeField] HeroController heroController;
    public  HeroPrefab heroPrefab;
   public Oracle oracle;

    public float AddFaithPointPoint;//夜になる時にもらえる信仰ポイント
    public int MaxArchitectsNumber;//建築士の人数
    public int architectsNumber;
    [SerializeField] Text architectsNumberText;

    public float[]　totalGold;//魔物の所持ゴールドの合計。配列にしているのは各魔物ごとにIDで分けるから。　キル数はプレイヤープレハブの方で読み取る。
    public float[] averageGlod;//魔物の所持金の平均。所持ゴールドの合計をキル数で割る。

    public int date;//日付。
    public  int SpawnBossDay;//ボス出現日数。5日後にスポーンすることにしようかな。
    [SerializeField] GameObject BossPrefab;
    [SerializeField] Vector3 BossPos;//ボスの生まれる場所
    [SerializeField] Text spawnBossDayText;

    public BossScript bossScript;
    public bool heroDieBool;//この日勇者が死んだかどうか。死んでたら、経験値は貰えないようにする。

    public Vector3 battleStartPosition;
    void Awake()
    {

        RenderSettings.skybox = daySkyBox; RenderSettings.fogColor = dayFogColor.color; nightLight.SetActive(false); dayLight.SetActive(true); night = false;
        architectsNumber = MaxArchitectsNumber;
        CalculationAverageGlod();
        heroDieBool = false;
        //DayChanges();

    }
    private void Start()
    {
        CheckDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6)) { DayChanges(); };
        if (Input.GetKeyDown(KeyCode.Alpha7)) { CalculationAverageGlod(); };

        if (Input.GetKeyDown(KeyCode.G)) { RenderSettings.skybox = nightSkyBox; RenderSettings.fogColor = nightFogColor.color; nightLight.SetActive(true); dayLight.SetActive(false); }
        if (Input.GetKeyDown(KeyCode.H)) { RenderSettings.skybox = daySkyBox; RenderSettings.fogColor = dayFogColor.color; nightLight.SetActive(false); dayLight.SetActive(true); }
        architectsNumberText.text = "建築士"+architectsNumber + "/" + MaxArchitectsNumber+ "人";
    }
    public void BirthOfHero() { }

    public void DayChanges()
    {
        Debug.Log("dayChanges");
        if (night == true) 
        { 
            feedObj.DOFade(1f, 3f).OnComplete(() =>
            {
                RenderSettings.skybox = daySkyBox;
                RenderSettings.fogColor = dayFogColor.color;
                nightLight.SetActive(false);
                dayLight.SetActive(true);
                night = false; Result(); 
            }); 
        }
        else if (night == false)
        {
            feedObj.DOFade(1f, 3f).OnComplete(() =>
            {
                feedObj.DOFade(0f, 2.5f);
                RenderSettings.skybox = nightSkyBox; 
                RenderSettings.fogColor = nightFogColor.color; 
                nightLight.SetActive(true);
                dayLight.SetActive(false);
                night = true;
                heroController.target = null; 
            }); 
        }
   
    }

    public void Result()
    {
        texts[0].text = "獲得信仰ポイント: " + AddFaithPointPoint;
        texts[0].DOFade(1f, 1f).OnComplete(() =>
        {

            texts[1].DOFade(1f, 1f).OnComplete(() =>
            {
                texts[2].DOFade(1f, 1f).OnComplete(() =>
{
    Debug.Log("nannde");
    HeroHUD.DOLocalMove(position, 1.8f).OnComplete(() =>
    {
        if (!heroDieBool)
        {
            liquidationExp.Liquidation();
            bonusExpText.text = "×" + (1 / liquidationExp.HPdifference).ToString();
        }
        else
        {
            liquidationExp.Liquidation();
            bonusExpText.text = "0";
        }
        //bonusExpText.gameObject.SetActive(true);

        
    });

}
);
            });
        });
        oracle.GetFaith(AddFaithPointPoint);
        //oracle.faithPoint += AddFaithPointPoint;
        AddFaithPointPoint = 0;
        
        heroPrefab.Feeling -= 10;
        heroPrefab.AbilitGrowth();
        CalculationAverageGlod();
        date++;
        CheckDay();
        for (int i=0; heroPrefab.killsInstant.Length>i;i++)
        {
            heroPrefab.killsInstant[i] = 0;
            Debug.Log("何回呼ばれた");
        }


    }
    public void Result2()
    {
        bonusExpText.gameObject.SetActive(true);

        OKButton.gameObject.SetActive(true);

    }

    public void EndButtonClick()//リザルト画面で最後に出てくる確認ボタンをクリック
    {
        OKButton.gameObject.SetActive(false);
        texts[0].DOFade(0f, 1f);
        texts[1].DOFade(0f, 1f);
        texts[2].DOFade(0f, 1f);
        bonusExpText.gameObject.SetActive(false);
        HeroHUD.DOLocalMove(new Vector3(0, 0, 0), 1.5f).OnComplete(() => {  feedObj.DOFade(0f, 2f).SetDelay(1.8f); });
        heroDieBool = false;
        heroPrefab.Revive();
    }

    public void CalculationAverageGlod()//魔物ごとの平均所持金を計算。魔物合計所持金÷キル数
    {

        
        for (int i = 0; i < averageGlod.Length; i++)
        {
           averageGlod[i]  = totalGold[i] / (heroPrefab.kills[i]+1);
        }
         
    }


    //ボス関係。ボスが日数経過で出現する。
    public void CheckDay()//日数をチェック
    {
        int UmareruMadeNoNissuu = SpawnBossDay - date;
        spawnBossDayText.text = "ボス襲撃まで　あと" + UmareruMadeNoNissuu + "日";
        if (UmareruMadeNoNissuu == 0) { SpawnBoss(); SpawnBossDay += SpawnBossDay; } else { bossScript.battleMode = false; }
        
    }
    public void SpawnBoss()//ボス襲撃開始。
    {
        bossScript.raidCount++;
        bossScript.battleMode = true;
        bossScript.gameObject.transform.position = oracle.SummonDemonParent.GetComponent<SummonDemonScript>().positions[2].position;   
    }

    public void HeroDie()
    {
        if (bossScript.battleMode == true)
        {
            heroPrefab.Feeling -= 30 + (bossScript.raidCount*10);//ボスの襲撃回数が多いほど気分は下がっていく。何回も負けたら嫌な気分になるもんね。
        }
        else
        {
            heroPrefab.Feeling -= 30;
        }
        heroDieBool = true;
        DayChanges();
    }

}
