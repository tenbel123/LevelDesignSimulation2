using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveEvent : MonoBehaviour
{
    public GameObject dammy;
   // [SerializeField] Oracle oracle;

    [SerializeField] EnemyDragGenerate enemyDragGenerate;
    // Start is called before the first frame update
    private void Awake()
    {
        dammy.SetActive(false);
    }
  



    // Update is called once per frame
    void Update()
    {
       // if(enemyDragGenerate.generateStart == false) { dammy.SetActive(false); }
    }
    public void MyPointerDownUI()//敵1の生成ボタンクリック
    {
        /*dammy.SetActive(true);

        //enemyDragGenerate.RecognitionObject(teki);
        
        enemyDragGenerate.generateStart = true;
        enemyDragGenerate.enemy1Flag = true;
        enemyDragGenerate.enemy2Flag = false;
        enemyDragGenerate.INNFlag = false;
        enemyDragGenerate.houseFlag = false;
        enemyDragGenerate.ChurchFlag = false;*/
        Debug.Log("古いメソッドだ。使わんよ。工事してる");


    }
    public void SuckEnemyDownUI()
    {
        /*dammy.SetActive(true);

        enemyDragGenerate.generateStart = true;
        enemyDragGenerate.enemy1Flag = false;
        enemyDragGenerate.enemy2Flag = true;
        enemyDragGenerate.INNFlag = false;
        enemyDragGenerate.houseFlag = false;
        enemyDragGenerate.ChurchFlag = false;*/
        Debug.Log("古いメソッドだ。使わんよ。工事してる");

    }
    public void INNDown()
    {
        /*if (enemyDragGenerate.oracle.faithPoint >= enemyDragGenerate.oracle.INNGeneratePoint)
        {
            dammy.SetActive(true);
            enemyDragGenerate.generateStart = true;

            enemyDragGenerate.INNFlag = true;
            enemyDragGenerate.enemy1Flag = false;
            enemyDragGenerate.enemy2Flag = false;

            enemyDragGenerate.houseFlag = false;
            enemyDragGenerate.ChurchFlag = false;
        }*/
        Debug.Log("古いメソッドだ。使わんよ。工事してる");

    }
    public void HouseDown()
    {
        /*if (enemyDragGenerate.oracle.faithPoint >= enemyDragGenerate.oracle.HouseGeneratePoint)
        {
            dammy.SetActive(true);
            enemyDragGenerate.generateStart = true;
            enemyDragGenerate.houseFlag = true;
            enemyDragGenerate.INNFlag = false;
            enemyDragGenerate.enemy1Flag = false;
            enemyDragGenerate.enemy2Flag = false;
            enemyDragGenerate.ChurchFlag = false;
        }*/
        Debug.Log("古いメソッドだ。使わんよ。工事してる");

    }
    public void ChurchDown()
    {
        /*if(enemyDragGenerate.oracle.faithPoint >= enemyDragGenerate.oracle.ChurchGeneratePoint)
        {
            dammy.SetActive(true);
            enemyDragGenerate.generateStart = true;
            enemyDragGenerate.INNFlag = false;
            enemyDragGenerate.houseFlag = false;
            enemyDragGenerate.ChurchFlag = true;
            enemyDragGenerate.enemy1Flag = false;
            enemyDragGenerate.enemy2Flag = false;
        }*/
        Debug.Log("古いメソッドだ。使わんよ。工事してる");
    }
}
