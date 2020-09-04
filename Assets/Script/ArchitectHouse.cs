using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectHouse : BuildingScript
{
    WorldController worldController;
    bool completed = false; //建築が終わった。
    public Material completedMaterial;//正しいマテリアル
    [SerializeField] int requiredDays;//完成までに必要な日数。
    int firstDay;//建築開始日
    int completionDay;//建築完了日
    bool one;
    private void Awake()
    {
        completedMaterial = GetComponent<MeshRenderer>().material;
        base.ID = 3;
    }
    void Start()
    {
        completed = false;
        one = true;
        //oracle = GameObject.Find("FaithButton").gameObject.GetComponent<Oracle>();
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        firstDay = worldController.date;
        completionDay = firstDay + requiredDays;
       
      
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
            if (one)
            {
                GetComponent<MeshRenderer>().material = completedMaterial;
                worldController.MaxArchitectsNumber++; 
                one = false; 
            }

        }
    }
        private void OnDestroy()
    {
        worldController.MaxArchitectsNumber--;
    }
}
