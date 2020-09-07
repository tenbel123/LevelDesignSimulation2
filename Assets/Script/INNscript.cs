using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.AI;

public class INNscript : BuildingScript
{
    GameObject liquidationExpGameObject;
    LiquidationExp liquidationExp;
    GameObject hero;
    HeroController heroController;
    WorldController worldController;
    bool one2 = true;
    bool completed = false; //建築が終わった。
    public Material completedMaterial;//正しいマテリアル
    [SerializeField] int requiredDays;//完成までに必要な日数。
    int firstDay;//建築開始日
    int completionDay;//建築完了日
    Oracle oracle;
    NavMeshObstacle navMeshObstacle;
    private void Awake()
    {
        completedMaterial = GetComponent<MeshRenderer>().material;
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        base.ID = 1;
    }
    void Start()
    {
        liquidationExpGameObject = GameObject.Find("LiquidationExp");
        liquidationExp = liquidationExpGameObject.GetComponent<LiquidationExp>();
        hero = GameObject.Find("Hero");
        heroController = hero.GetComponent<HeroController>();
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
       // oracle = worldController.oracle;
    
        firstDay = worldController.date;
        completionDay = firstDay + requiredDays;
        one2 = true;
        completed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (worldController.date >= completionDay)
        {
            completed = true;
        }
        if (completed)
        {
            if (one2) { GetComponent<MeshRenderer>().material = completedMaterial; one2 = false; }
        }
        if(heroController.type == HeroController.MODE_TYPE.GoToINN)
        {
            navMeshObstacle.enabled = false;
        }
        else
        {
            navMeshObstacle.enabled = true;
        }
            //if(heroController.toINNflag == true) { this.gameObject.GetComponent<MeshCollider>().isTrigger = true; }
            //else{ this.gameObject.GetComponent<MeshCollider>().isTrigger = false; }
        }
    public void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Hero") { Debug.Log("町に入った"); worldController.DayChanges();//liquidationExp.Liquidation(); 
           // collider.gameObject.GetComponent<HeroController>().target = null;
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Hero")
        {
            HeroController aaa = collider.gameObject.GetComponent<HeroController>();
            aaa.type = HeroController.MODE_TYPE.roam;
        }
    }

 
}
