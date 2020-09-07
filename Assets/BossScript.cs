using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;


public class BossScript : MonoBehaviour
{
    //NavMeshAgent agent;
    [SerializeField] EnemyDate enemyDate;
    public Transform target;
    new public string name = "New Enemy";
    public float HP;
    public float MaxHP;
    public float ATK;
    public float range;
    public HeroPrefab heroPrefab;
    WorldController worldController;
    [SerializeField] GameObject damageTextPrefab;//攻撃食らったときにダメージを数値として出すUIの奴。
   public  bool battleMode;
    float distance;//勇者との距離
    SummonDemonScript summonDemonScript;
    public int raidCount = 0;//襲撃の回数。

    private void Awake()
    {
        raidCount = 0;
        summonDemonScript = GameObject.Find("FaithButton").GetComponent<Oracle>().SummonDemonParent.GetComponent<SummonDemonScript>();
    }
    private void Start()
    {
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        enemyDate.parentEnemyPrefab = this.transform.GetComponent<EnemyPrefab>();
        try
        {
            target = serchTag(gameObject, "Hero").transform;
            heroPrefab = target.gameObject.GetComponent<HeroPrefab>();
        }
        catch { target = null; Debug.Log("aaa"); }
        battleMode = false;
        StartCoroutine("AttackAnimeStart");

    }

    GameObject serchTag(GameObject nowObj, string tagName)
    {
        Debug.Log("serchTag");
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        string nearObjName = "";    //オブジェクト名称
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {

            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                nearObjName = obs.name;
                //targetObj = obs;
            }
        }
        //最も近かったオブジェクトを返す
        return GameObject.Find(nearObjName);
        // return targetObj;
    }
    public void FixedUpdate()
    {
       distance = Vector3.Distance(target.position, transform.position);
   　　if(battleMode == false) { gameObject.transform.position = summonDemonScript.bossPosition; }

}
public float actionSpeed = 1f;
    IEnumerator AttackAnimeStart()
    {

        while (true)
        {
            yield return new WaitForSeconds(actionSpeed);
            if (battleMode)
            {
                action();
            }
            
        }
    }
    public void action()
    {
        var enemyActionProbs = new Dictionary<string, int>()//よくわからんけど、行動のパターンを確立でやれる。DictExtensionsスクリプトを見て。
    {
        { "attack1", 60 },
        { "attack2", 60 },
        { "skill1", 30 },
        { "summon", 40 },
    };
       /* for (int i = 0; i < 50; i++)
        {
            Debug.Log(enemyActionProbs.GetByRouletteSelection());
        }*/
      var aaa =  enemyActionProbs.GetByRouletteSelection();
        if (aaa == "attack1") 
        {
            if (distance < 15) { heroPrefab.damage(ATK); Debug.Log("attack1"); } else { action(); }
        }
        if (aaa == "attack2")
        {
            if (distance > 15)
            {
                var parent = this.transform;

                //Instantiate(enemyDate.Particle, this.transform.position, Quaternion.identity, parent);
                Debug.Log("attack2");
            }
            else
            {
                action();
            }

        }
        if (aaa == "skill1") { Debug.Log("skill1"); }
        if (aaa == "summon") { 
            summonDemonScript.BossSummonDemon(); Debug.Log("summon"); 
        }


        /*int aaa = Random.Range(0, 2);

          switch (aaa)
          {
              case 0:
                  if (distance < 15) { heroPrefab.damage(ATK); } else { action(); }
                  break;
              case 1:
                  if(distance > 15)
                  {
                      var parent = this.transform;

                      Instantiate(enemyDate.Particle, this.transform.position, Quaternion.identity, parent);
                  }
                  break;
          }*/
    }
    public void Damage(float damage)
    {
        Vector3 pos = transform.position;
        var aaa = Instantiate(damageTextPrefab, pos, Quaternion.identity);
        aaa.GetComponent<DamageText>().Number(damage);
        //aaa.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();
        HP -= damage;
        transform.DOJump(transform.forward * -2, 1, 1, 1f).SetRelative().SetEase(Ease.OutExpo);

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }
}
