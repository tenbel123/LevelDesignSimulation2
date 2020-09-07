using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LiquidationExp: MonoBehaviour
{
   [SerializeField] HeroPrefab hero;
    public float HPdifference;
    Slider slider;
    GameObject EXPBar;
   [SerializeField] WorldController worldController;
  /*  public void Liquidation()
    {//ＭＡＸＨＰから減ったＨＰの割合を、その数で3回割ると、0.1のとき10倍。０．２の時5倍となる！ギリギリ～～。ちなみに8の時は１．２５倍
    //これは、１/その数で同じことができる。今回はそれで行く。
        HPdifference = 1-((hero.MaxHP - hero.HP) / hero.MaxHP);
       
        hero.exp += Mathf.Floor( hero.expStorage *(1/HPdifference)*100)/100;
        hero.expStorage = 0;
        Debug.Log(1 / HPdifference);
        Debug.Log(Mathf.Floor(hero.expStorage * (1 / HPdifference) * 100) / 100);
        hero.HP = hero.MaxHP;
        ExpTable();
        hero.gameObject.GetComponent<HeroController>().toINNflag = false;
        hero.gameObject.GetComponent<HeroController>().target = null;
    }*/

    public void Liquidation()//こっちが最新版の経験値清算
    {
        Debug.Log("経験値清算");
        HPdifference = 1 - ((hero.MaxHP - hero.HP) / hero.MaxHP);
        float HP = hero.HP;
        Debug.Log("どこまで呼ばれてる？");
        DOTween.To(() => hero.HP,(n) =>hero.HP = n,hero.MaxHP, 2f).OnComplete(() => 
        {
            if (!(hero.gameObject.GetComponent<HeroController>().type == HeroController.MODE_TYPE.die)) 
            { 
                hero.exp += Mathf.Floor(hero.expStorage * (1 / HPdifference) * 100) / 100;
                ExpTable(); 
            }
            worldController.Result2();
        });
    }
    public void bbb()
    {

    }

    public void Awake()
    {
        ExpTable();
    }
    public void ExpTable()
    {
        EXPBar = GameObject.Find("Exp_Bar");
        slider = EXPBar.GetComponent<Slider>();
        hero.NextLvUpExp = (hero.LV-1) * 10f + 10;
        slider.maxValue = hero.NextLvUpExp;
        startExp = hero.exp;
        startPoint = point;
        point = 0;
        point2 = 0;
       // start = true;

    }
    float point;
    bool start;
    float startExp;
    float startPoint;
    float point2;
    private void Update()
    {
        

        //if (start == true)
        {
            if (hero.exp > 0) {hero.exp =  Mathf.Clamp(hero.exp -= 0.01f*hero.NextLvUpExp, 0, hero.exp);point2 = startExp - hero.exp; /* point+= 0.1f;*/ }
            point = startPoint + point2;
            slider.value = point;
            if(point>= hero.NextLvUpExp) { hero.LvUp(); point = 0; ExpTable(); }//ここでレベルアップ
           
        }

    }


}
