using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    Oracle oracle;
    private float FaithAmount = 10f;
    WorldController worldController; 
    public bool one = true;
    bool one2 = true;
    bool completed = false; //建築が終わった。
    public Material completedMaterial;//正しいマテリアル
   [SerializeField] int requiredDays;//完成までに必要な日数。
    int firstDay;//建築開始日
    int completionDay;//建築完了日
    private void Awake()
    {
        completedMaterial = GetComponent<MeshRenderer>().material; 
    }
    void Start()
    {

        completed = false;
        oracle = GameObject.Find("FaithButton").gameObject.GetComponent<Oracle>();
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        firstDay = worldController.date;
        completionDay = firstDay + requiredDays;
        one2 = true;
        //StartCoroutine("GenerateFaith");
    }

    // Update is called once per frame
    void Update()
    {
      if(  worldController.date >= completionDay)
        {
            completed = true;
        }
        if (completed)
        { 
            if (one2) { GetComponent<MeshRenderer>().material = completedMaterial; one2 = false; }
            
            if (worldController.night == false)
            {
                GetFaithPoint(one);
            }
            else { one = true; }
        }
    }
    /* public float span = 3f;
     IEnumerator GenerateFaith()//次の目的を探す。span秒ごとに呼ばれる。
     {
         while (true)
         {
              yield return new WaitForSeconds(span);
             oracle.GetFaith(FaithAmount);
         }
     }*/
    public void GetFaithPoint(bool aaa)
    {
        if (aaa)
        {
            Debug.Log("aaa");
            worldController.AddFaithPointPoint+= FaithAmount;
            one = false;
        }
    }
}
