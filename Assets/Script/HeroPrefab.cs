using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPrefab : MonoBehaviour
{
    //[SerializeField] HeroDate heroDate;
    HeroController heroController;
    public  WorldController worldController;
   public float HP;
    public float MaxHP;
    public float ATK;
    public float movementSpeed;
    //public float HPGrowthValue;
    //public float ATKGrowthValue;

    public int LV;
    public float expStorage;
    public float exp;
    public float gold = 0;//所持金
    Slider slider;
    GameObject HPBar;
    public Text expStrageText;
    public Text expText;
    public Text LVtext;
    [SerializeField] Text HPtext;
    public float NextLvUpExp;

    [SerializeField] HeroDate heroDate;

    public float Feeling;//気分

    //ここからは才能ability 基本的にこれらの数値を上げ下げして、スキルの素質を発現する（特殊な行動をしないと覚えないスキルも山ほどある）。そのあとは神が信仰心を使って勇者にスキルを与える。
    public float iceAbility;
    public float frameAbility;
    public float thunderAbility;
    public float waterAbility;
    public float braveAbility;//勇気。タイマン寄り。単体火力や単純なバフ。
    public float composureAbility;//冷静。狡猾なスキル寄り。
    public float gloryAbility;//栄光。殲滅より。派手なスキル。AOE
    public float nostalgiaAbility;//郷愁 寄り道をしたがる?　というか、町への愛着の数値。町を勇者に愛されるように構築していくことも必要だということ。

    public float[] kills;//その敵を何体倒したか。合計。
    public int[] killsInstant;//その日倒した限定の敵の数。
    public List<string> skills = new List<string>();



    [SerializeField] TrailRenderer trailRenderer;//剣の軌跡。斬撃

    private void Awake()
    {
        Scan();
        heroController = gameObject.GetComponent<HeroController>();
        HPBar = GameObject.Find("Hero_HP_Bar");
        slider = HPBar.GetComponent<Slider>();
    }
    public void Scan()
    {
        MaxHP = heroDate.HP;
        ATK = heroDate.ATK;
        movementSpeed = heroDate.movementSpeed;

    }
    void Start()
    {
        
        HP = MaxHP;
        Feeling = 100;
      
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = MaxHP;
        slider.value = HP;
        expStrageText.text ="仮EXP:"+ expStorage.ToString();
        expText.text = "EXP:" + exp.ToString("f1")+"/"+NextLvUpExp.ToString("f1");
        LVtext.text = "LV:"+LV.ToString();
        HPtext.text = HP + "/" + MaxHP;

        if (Input.GetKeyDown(KeyCode.G)) { AbilitGrowth(); CheckSlill(); }
    }
    public void LvUp()
    {
        LV++;
        MaxHP += heroDate.HPGrowthValue;
        ATK += heroDate.ATKGrowthValue;
        HP = MaxHP;
        movementSpeed += heroDate.movementSpeedGrowthValue;
        Feeling += 10;
    }
    public void AttackStart()
    {
        trailRenderer.emitting = true;
    }
    public void Attack()
    {
        Debug.Log("勇者の攻撃");
        trailRenderer.emitting = false;
        if (heroController.target != null && heroController.type == HeroController.MODE_TYPE.battle)
        {
            if (heroController.target.tag == "Enemy")
            {
                heroController.target.gameObject.GetComponent<EnemyPrefab>().Damage(ATK);
            }
            if(heroController.target.tag == "Boss")
            {
                heroController.target.gameObject.GetComponent<BossScript>().Damage(ATK);
            }
        }
    }
    public void damage(float damage)
    {
      HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        if(HP <= 0)
        {
            Die();
        }
        else
        {
            heroController.animator.SetTrigger("GetHit");
            trailRenderer.emitting = false;
        }
        //slider.value = HP;
    }
    public void GetExp(float EnemyExp)
    {
        expStorage += EnemyExp;
    }
    public void Die()
    {
        heroController.animator.SetTrigger("Die");
        heroController.type = HeroController.MODE_TYPE.die;
        heroController.agent.isStopped = true;
        worldController.HeroDie();


    }
    public void Revive()
    {
        HP = MaxHP;
        heroController.type = HeroController.MODE_TYPE.roam;
        heroController.animator.SetTrigger("Revive");
    }

    public void AbilitGrowth()
    {
       float killNumber = 0;//キル数の合計。その日の
       foreach(float kill in killsInstant)
        {
            killNumber += kill;
        }
        switch (killNumber)
        {
            case 1:
                GetAbility("brave", 5);
                GetAbility("glory", 1);
                break;
            case 2:
                GetAbility("brave", 4);
                GetAbility("glory", 2);
                break;
            case 3:
                GetAbility("brave", 3);
                GetAbility("glory", 3);
                break;
            case 4:
                GetAbility("brave", 2);
                GetAbility("glory", 4);

                break;
            case 5:
                GetAbility("brave", 1);
                GetAbility("glory", 5);
                break;
        }
        if ((HP / MaxHP) >= 0.1f)
        {
           // GetAbility("composure", 1);
        }
        else if ((HP / MaxHP) >= 0.2f)
        {
           // GetAbility("composure", 5);
        }
        else if ((HP / MaxHP) >= 0.3f)
        {
            GetAbility("composure", 1);
        }
        else if ((HP / MaxHP) >= 0.4f)
        {
            GetAbility("composure", 2);
        }
        else if ((HP / MaxHP) >= 0.5f)
        {
            GetAbility("composure", 3);
        }
        else if ((HP / MaxHP) >= 0.6f)
        {
            GetAbility("composure", 4);
        }
        else if ((HP / MaxHP) >= 0.7f)
        {
            GetAbility("composure", 5);
        }

        GetAbility("thunder", killsInstant[0]);
        GetAbility("water", killsInstant[1]);
        GetAbility("frame", killsInstant[2]);


        Debug.Log("HP/MaxHP" + HP / MaxHP);

    }
    public void GetAbility(string name, int suuti)
    {
        switch (name)
        {
            case "ice":
                iceAbility += suuti;
                break;
            case "frame":
                frameAbility += suuti;
                break;
            case "thunder":
                thunderAbility += suuti;
                break;
            case "water":
                waterAbility += suuti;
                break;
            case "brave":
                braveAbility += suuti;
                break;
            case "composure":
                composureAbility += suuti;
                break;
            case "glory":
                gloryAbility += suuti;
                break;
            default:
                Debug.Log("GetAbility()エラー！！！！");
                break;
        }
}
    public void CheckSlill()//スキルを発現するに値するか
    {
        
        if(braveAbility >= 10) { if (!skills.Contains("smash")) { skills.Add("smash"); } }
        if(composureAbility >= 10) { if (!skills.Contains("PowerUp")) { skills.Add("PowerUp"); } }
        if (iceAbility >= 10) { if (!skills.Contains("slow")) { skills.Add("slow"); } }
        if (frameAbility >= 10) { if (!skills.Contains("fire")) { skills.Add("fire"); } }
        if (thunderAbility >= 10) { if (!skills.Contains("thunder")) { skills.Add("thunder"); } }
        if (waterAbility >= 10) { if (!skills.Contains("water")) { skills.Add("water"); } }



    }


}
