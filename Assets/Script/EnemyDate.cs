using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObject/Create Enemy")]


public class EnemyDate :ScriptableObject
{
    new public string name = "New Enemy";
    public int ID;
    public Sprite image;
    public float HP;
    public float ATK;
    public float exp;
    public float MovementSpeed;
    public float Range;
    public float requiredPoint;
    public GameObject Particle;

   public EnemyPrefab parentEnemyPrefab;

    [Header("壺の魔物用")]
    [Header("スキル１の爆発時判定オブジェクト")]
    public GameObject enemy1Particle1;
    public GameObject enemy1Particle2;

    [Header("袋の魔物用")]
    [Header("スキル１")]
    public GameObject enemy2Particle1;
    public float enemy2skill1Speed = 500;
    public GameObject enemy2Particle2;

    [Header("箱の魔物用")]
    public GameObject enemy3Particle1;
    public GameObject enemy3Particle2;






    public void Attack()
    {
        if(ID == 0) {       
            parentEnemyPrefab.target.gameObject.GetComponent<HeroPrefab>().damage(ATK);
        }
        if(ID == 1) {
          //  Instantiate(Particle, parentEnemyPrefab.transform.position, Quaternion.identity);
        }
    }
    public void Test()
    {
        Debug.Log(parentEnemyPrefab.HP);
    }


  
       

    
}