using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchScript : BuildingScript
{
    Oracle oracle;
    private float FaithAmount = 10f;
    WorldController worldController;
    public bool one = true;

    bool completed = false; //建築が終わった。
    public Material completedMaterial;//正しいマテリアル
    [SerializeField] int requiredDays;//完成までに必要な日数。
    int firstDay;//建築開始日
    int completionDay;//建築完了日
    bool one2;
    [SerializeField] GameObject aaaa;
   private void Awake()
    {
        completedMaterial = GetComponent<MeshRenderer>().material;
        base.ID = 2;
    }

    void Start()
    {
        Debug.Log(aaaa.GetComponent<BuildingScript>().ID);
        completed = false;
        oracle = GameObject.Find("FaithButton").gameObject.GetComponent<Oracle>();
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        firstDay = worldController.date;
        completionDay = firstDay + requiredDays;
        one = true;
        one2 = true;
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
            if (one) { GetComponent<MeshRenderer>().material = completedMaterial; }
            if (worldController.night == false)
            {
                PointUp(one2);
            }
            else { one2 = true; }
        }
    }
   /* public void OnTriggerStay(Collider other)//未完成。範囲内に効果を及ぼしたかった。
    {
      
        if (one2)
        {
            if (other.tag == "Building")
            {
                if(other.gameObject.GetComponent<BuildingScript>().ID == 0)//民家だった時
                {
                    other.GetComponent<HouseScript>().PercentageFaithAmount = 2;
                }
            }
        }
    }*/

    public void PointUp(bool aaa)//上と違って、シーン全体に効果を及ぼすようにした。
    {
        if (aaa)
        {
            //Buildingというタグ名のゲームオブジェクトを複数取得したい時
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Building");
            Debug.Log(objects.Length);
            //配列の要素一つ一つに対して処理を行う
            foreach (GameObject obj in objects)
            {
                //Debug.Log(obj);
                //Debug.Log(obj.GetComponent<BuildingScript>().ID);
              if(obj.GetComponent<BuildingScript>().ID == 0)//民家の時
                {
                    obj.GetComponent<HouseScript>().PercentageFaithAmount = 10;
                }
            }
            one2 = false;
        }
    }
}
