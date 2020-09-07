using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;
using UnityEngine.Scripting;
using Chronos;
public class EnemyPrefab : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] EnemyDate enemyDate;
    public  Transform target;
    bool moveflag = true;
    bool battleflag = false;
    public bool roamflag = false; //うろつき。何もしてないときそのあたりをうろつく。
    public Vector3[] points;      /* 移動ポイント roamの*/
    private int destPoint = 0;      /* 目的とするポイント roamの*/

    public int ID;
    new public string name = "New Enemy";
    public int LV;
    public float EXP;
    public float money;
    public float HP;
    public float MaxHP;
    public float ATK;
    public float MovementSpeed;
    public float range;

    public HeroPrefab heroPrefab;
    GameObject hero;
    WorldController worldController;
    [SerializeField] GameObject damageTextPrefab;//攻撃食らったときにダメージを数値として出すUIの奴。
    [SerializeField] GameObject destroyParticle;

    void Start()
    {
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        enemyDate.parentEnemyPrefab = this.transform.GetComponent<EnemyPrefab>();
       
        agent = GetComponent<NavMeshAgent>();
        //target = GameObject.Find("Hero").transform;
          try{ target = serchTag(gameObject, "Hero").transform;  
             heroPrefab = target.gameObject.GetComponent<HeroPrefab>();
        }
        catch { target = null;Debug.Log("aaa"); }
       // GetDate(1);
       // hero = GameObject.Find("Hero");
     
        StartCoroutine("AttackAnimeStart");


        points = new Vector3[5];
        points[0] = new Vector3(this.transform.position.x + 5, this.transform.position.y, this.transform.position.z + 5);
        points[1] = new Vector3(this.transform.position.x - 5, this.transform.position.y, this.transform.position.z + 5);
        points[2] = new Vector3(this.transform.position.x + 5, this.transform.position.y, this.transform.position.z - 5);
        points[3] = new Vector3(this.transform.position.x - 5, this.transform.position.y, this.transform.position.z - 5);
        points[4] = new Vector3(this.transform.position.x , this.transform.position.y, this.transform.position.z );
        roamflag = true;

        agent.speed = MovementSpeed;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { Test(); }
        if (Input.GetKeyDown(KeyCode.S)) { Damage(1); }
        if (Input.GetKeyDown(KeyCode.Y)) {GetComponent<Timeline>(); }
        if (worldController.heroDieBool) { Destroy(gameObject); }
    }

    public void GetDate(int lv,float gold)
    {
        ID = enemyDate.ID;
        LV = lv; //生成した時に決める。
        EXP = enemyDate.exp * LV;
        name = enemyDate.name;
        HP = enemyDate.HP*LV;//今はとりあえずレベルの数値をかけるようにしている、あとで調整するかもね。
        MaxHP = HP;
        ATK = enemyDate.ATK*LV;
        MovementSpeed = enemyDate.MovementSpeed;
        range = enemyDate.Range;
        money = gold;
       
    }
    public float AttackSpeed = 3f;

    IEnumerator AttackAnimeStart()
    {
        
        while (true)
        {
            
          
            if (battleflag) {  Attack(); }
            yield return new WaitForSeconds(AttackSpeed);
        }
    }
    public void Attack()
    {

        if (ID == 0) { target.gameObject.GetComponent<HeroPrefab>().damage(ATK); }
        if (ID == 1)
        { //var obj =  Instantiate(enemyDate.Particle, transform.position, Quaternion.identity); obj.transform.parent = this.transform; }
            var parent = this.transform;
           
            Instantiate(enemyDate.Particle, this.transform.position, Quaternion.identity, parent);


        }
        if (ID == 2) { target.gameObject.GetComponent<HeroPrefab>().damage(ATK); }

    }
 
    void FixedUpdate() 
    {
        if (worldController.oracle.type == Oracle.MODE_TYPE.Battle)
        {
            if (Input.GetKeyDown(KeyCode.A)) { enemyDate.Test(); }
            if (target == null) { target = null; }//ターゲットが消えたらちゃんとヒーローのターゲットも無くす。
            if (battleflag == true) { moveflag = false; }
            if (moveflag == true) { battleflag = false;/*agent.speed = MovementSpeed;*/agent.isStopped = false; } else { agent.isStopped = true; /*agent.speed = 0; */}


            if (target != null)
            {

                float distance = Vector3.Distance(target.position, transform.position);
                if (distance <= range) { battleflag = true; }
                else if (distance <= 15) { moveflag = true; battleflag = false; agent.destination = target.position; } else { roamflag = true; }
                if (moveflag == true) { }
                else
                {
                    if (battleflag == true)
                    {
                        //バトル中に、移動を辞めているので敵の方向を向かない。ので向きを無理やり回転させる。
                        // 補完スピードを決める
                        float speed = 0.1f;
                        // ターゲット方向のベクトルを取得
                        Vector3 relativePos = target.position - transform.position;
                        // 方向を、回転情報に変換
                        Quaternion rotation = Quaternion.LookRotation(relativePos);
                        // 現在の回転情報と、ターゲット方向の回転情報を補完する
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
                    }
                }
            }
            else
            {



            }

            if (roamflag == true) /* 目的地付近についたら次のポイントを目指す */
            {
                moveflag = true;
                if (agent.remainingDistance < 0.5f)
                {
                    GotoNextPoint();
                }
            }
        }

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


    /*------------------------*/
    /*  目的地移動関数         */
    /*------------------------*/
    void GotoNextPoint()
    {

        /* ポイントなかったらreturn */
        if (points.Length == 0)
        {
            return;
        }

        /* 目的ポイントをランダムで生成 */
        destPoint = (int)Random.Range(0.0f, points.Length);

        /* 次の目的地へGo */
        agent.destination = points[destPoint];
    }

    public void Damage(float damage)
    {
        Vector3 pos = transform.position;
        var aaa=Instantiate(damageTextPrefab,pos, Quaternion.identity);
        aaa.GetComponent<DamageText>().Number(damage);
        //aaa.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();
        HP -= damage;
        transform.DOJump(transform.forward*-2,1, 1,1f).SetRelative().SetEase(Ease.OutExpo);

        if(HP<= 0) {
            heroPrefab.GetExp(EXP);
            Destroy(gameObject); }
        
    }


    public void Test()//ヒーローが誕生した時に呼ぶ。ターゲットを設定するため。
    {
        target = serchTag(gameObject, "Hero").transform;
        heroPrefab = target.gameObject.GetComponent<HeroPrefab>();
    }
    public void OnDestroy()
    {
        if (heroPrefab.gameObject.GetComponent<HeroController>().serchTag(gameObject,"Enemy"))
        {
            Debug.Log("次の敵探すよ");
            heroPrefab.gameObject.GetComponent<HeroController>().target = heroPrefab.gameObject.GetComponent<HeroController>().serchTag(gameObject, "Enemy").transform;
        }
            heroPrefab.gold += money;
        heroPrefab.kills[ID] += 1;
        heroPrefab.killsInstant[ID] += 1;
        worldController.totalGold[ID] += money;
        heroPrefab.Feeling += (money - worldController.averageGlod[ID]) / worldController.averageGlod[ID] * 10f;//平均所持金と魔物のお金の差があればあるほど気分に差が出る計算。最後の２０ｆとは、無かったら少なすぎるので入れてる。この数値も建物のレベルアップなどで変更したらいいかも。
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
    }

    public void Skill1()//スキルスロットの一つ目
    {
        switch (ID)
        {
            case 0:
                Rigidbody rb = Instantiate(enemyDate.enemy1Particle1, transform.Find("射出口").position, Quaternion.identity).AddComponent<Rigidbody>(); 
              
             
                
                //SkillObj = null;
                //rb = null;
                break;
            case 1:

                GameObject aaa = Instantiate(enemyDate.enemy2Particle1, transform.Find("射出口").transform.position, Quaternion.identity);
                aaa.transform.LookAt(target.transform.Find("心臓"));
                aaa.GetComponent<Rigidbody>().AddForce(aaa.transform.forward * enemyDate.enemy2skill1Speed);
                break;

            case 2:
                //爆発　付近のキャラにダメージを与えてから死ぬ。
                Instantiate(enemyDate.enemy3Particle1, transform.position, Quaternion.identity);
                GameObject ChildObject;
                ChildObject = transform.GetChild(0).gameObject;
                ChildObject.SetActive(true);
                DOVirtual.DelayedCall(0.1f,   // 遅延させる（待機する）時間  
                    () => {
                        Damage(999);
                    }
                        );
                break;

        }
    }
    public void Skill2()
    {
        switch (ID)
        {
            case 0:
                break;
        }
    }

  
}

