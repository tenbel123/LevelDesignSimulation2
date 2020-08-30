using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HeroController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

   public  Animator animator;
    [SerializeField] HeroPrefab heroPrefab;
    // public bool toINNflag = false;
    // public  bool moveflag = false;
    // public  bool battleflag = false;
    //public bool roamflag = false; //うろつき。何もしてないときそのあたりをうろつく。
    public Vector3[] points;      /* 移動ポイント roamの*/
    private int destPoint = 0;      /* 目的とするポイント roamの*/
    // bool talkflag = false;
    public enum MODE_TYPE
    {
        battle,
        roam,
        move,
        GoToINN,
    }
    public MODE_TYPE type = MODE_TYPE.roam;

    // Start is called before the first frame update
    void Start()
    {
        type = MODE_TYPE.roam;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = heroPrefab.movementSpeed;
        animator = GetComponent<Animator>();
        StartCoroutine("FindPurpose");
        StartCoroutine("AttackAnimeStart");


        points = new Vector3[6];
        GotoNextPoint();

    }
    public float span = 0.5f;
    IEnumerator FindPurpose()//次の目的を探す。span秒ごとに呼ばれる。
    {
        while (true)
        {

            if (serchTag(gameObject, "Enemy") == null) { }
            else if (!(type == MODE_TYPE.GoToINN)) { target = serchTag(gameObject, "Enemy").transform; } //敵を探す。


            yield return new WaitForSeconds(span);
        }
    }
    public float AttackSpeed = 2f;

    IEnumerator AttackAnimeStart()
    {
        while (true)
        {
            if (type == MODE_TYPE.battle) { animator.SetTrigger("Attack"); }
            yield return new WaitForSeconds(AttackSpeed);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null) { target = null; }//ターゲットが消えたらちゃんとヒーローのターゲットも無くす。
                                              // if (toVillage != true) {

        // if (battleflag == true) { moveflag = false; roamflag = false; }
        if (type == MODE_TYPE.move || type == MODE_TYPE.roam|| type == MODE_TYPE.GoToINN) { animator.SetBool("Run", true);  agent.isStopped = false; } else { animator.SetBool("Run", false); agent.isStopped = true; }

            if (target != null)
            {
               
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance <= 7) { if (target.tag == "Enemy") { type = MODE_TYPE.battle; } }
                else if (distance <= 30) { type = MODE_TYPE.move;  agent.destination = target.position;} else { if (target.tag == "Village") { agent.destination = target.position; } else { type = MODE_TYPE.move; target = null; } }

            if (type == MODE_TYPE.move) {  }
            else
            {
                if (!(type==MODE_TYPE.GoToINN))
                {
                   // agent.speed = 0;  /*transform.LookAt(target);*/
                    if (type== MODE_TYPE.battle)
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
            }
            else {
            type = MODE_TYPE.roam;
            }
        /* 目的地付近についたら次のポイントを目指す */
        if (type ==MODE_TYPE.roam)
        {
            
            if (agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
        // }
        // else { Debug.Log("matiniikitai"); agent.speed = 2;
        //       agent.destination = target.position;
        //  }
    }

    /*------------------------*/
    /*  目的地移動関数         */
    /*------------------------*/
    public void GotoNextPoint()
    {
        points[0] = new Vector3(this.transform.position.x + 10, this.transform.position.y, this.transform.position.z + 15);
        points[1] = new Vector3(this.transform.position.x - 10, this.transform.position.y, this.transform.position.z + 15);
        points[2] = new Vector3(this.transform.position.x + 10, this.transform.position.y, this.transform.position.z - 5);
        points[3] = new Vector3(this.transform.position.x + 10, this.transform.position.y, this.transform.position.z - 5);
        points[4] = new Vector3(this.transform.position.x + 0, this.transform.position.y, this.transform.position.z + 15);
        points[5] = new Vector3(this.transform.position.x + 0, this.transform.position.y, this.transform.position.z - 5);



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

    public void DownToVillageButton()
    {
        type = MODE_TYPE.GoToINN; target = serchTag(gameObject, "Village").transform;Debug.Log("よばれた");
    }
    GameObject serchTag(GameObject nowObj, string tagName)
    {

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
                //nearObjName = obs.name;
                targetObj = obs;
            }


        }
        //最も近かったオブジェクトを返す
      //  Debug.Log(nearObjName);
        //return GameObject.Find(nearObjName);
         return targetObj;
    }

    public void ChangeMode(MODE_TYPE mODE_TYPE)
    {
        type = mODE_TYPE;
    }
}
