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
    [SerializeField] HeroPrefab heroPrefab;
   [SerializeField] Oracle oracle;

    public float AddFaithPointPoint;//夜になる時にもらえる信仰ポイント
    public int MaxArchitectsNumber;//建築士の人数
    public int architectsNumber;
    [SerializeField] Text architectsNumberText;

    public float[]　totalGold;//魔物の所持ゴールドの合計。配列にしているのは各魔物ごとにIDで分けるから。　キル数はプレイヤープレハブの方で読み取る。
    public float[] averageGlod;//魔物の所持金の平均。所持ゴールドの合計をキル数で割る。

    public int date;//日付。

    void Awake()
    {

        RenderSettings.skybox = nightSkyBox; RenderSettings.fogColor = nightFogColor.color; nightLight.SetActive(true); dayLight.SetActive(false); night = true;
        architectsNumber = MaxArchitectsNumber;
        CalculationAverageGlod();
        //DayChanges();

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
        if (night == false) 
        { 
            Debug.Log("夜になれ"); 
            feedObj.DOFade(1f, 3f).OnComplete(() =>
            {
                RenderSettings.skybox = nightSkyBox;
                RenderSettings.fogColor = nightFogColor.color;
                nightLight.SetActive(true);
                dayLight.SetActive(false);
                night = true; Result(); 
            }); 
        }
        else if (night == true)
        {
            Debug.Log("昼になれ"); 
            feedObj.DOFade(1f, 3f).OnComplete(() =>
            {
                feedObj.DOFade(0f, 2.5f);
                RenderSettings.skybox = daySkyBox; 
                RenderSettings.fogColor = dayFogColor.color; 
                nightLight.SetActive(false);
                dayLight.SetActive(true);
                night = false;
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
        Debug.Log("oioi");
        liquidationExp.Liquidation();
        bonusExpText.text = "×" + (1 / liquidationExp.HPdifference).ToString();
        //bonusExpText.gameObject.SetActive(true);

        
    });

}
);
            });
        });
        oracle.faithPoint += AddFaithPointPoint;
        AddFaithPointPoint = 0;
        heroPrefab.Feeling -= 10;
        CalculationAverageGlod();
        date++;


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
        
    }

    public void CalculationAverageGlod()//魔物ごとの平均所持金を計算。魔物合計所持金÷キル数
    {

        
        for (int i = 0; i < averageGlod.Length; i++)
        {
           averageGlod[i]  = totalGold[i] / (heroPrefab.kills[i]+1);
        }
         
    }


}
