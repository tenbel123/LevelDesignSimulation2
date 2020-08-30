using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPrefab : MonoBehaviour
{
    //[SerializeField] HeroDate heroDate;
    HeroController heroController;
    [SerializeField] WorldController worldController;
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
    public float IceAbility;
    public float FrameAbility;
    public float ThunderAbility;
    public float braveAbility;//勇気
    public float composureAbility;//冷静
    public float intuitionAbility;//直感
    public float nostalgiaAbility;//郷愁 寄り道をしたがる?　というか、町への愛着の数値。町を勇者に愛されるように構築していくことも必要だということ。

    public float[] kills;//その敵を何体倒したか。



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
        if (heroController.target != null && heroController.type == HeroController.MODE_TYPE.battle) { heroController.target.gameObject.GetComponent<EnemyPrefab>().Damage(ATK); }
    }
    public void damage(float damage)
    {
      HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        if(HP <= 0)
        {
            Die();
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
        heroController.agent.isStopped = true;
    }

}
