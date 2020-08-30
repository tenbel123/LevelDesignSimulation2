using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//参考サイトhttps://gomafrontier.com/unity/3307
public class EnemyDragGenerate : MonoBehaviour
{
    WorldController worldController;
    public Camera mainCamera;
    private Vector3 currentPosition = Vector3.zero;
    [SerializeField] UIController uIController;
    public GameObject enemy1;//実際に生成される敵キャラ
    public GameObject enemy2;
    public GameObject INN;
    public GameObject house;
    public GameObject church;
    [SerializeField] ReceiveEvent receiveEvent;
    public Oracle oracle;
    public bool generateStart = false;

    public bool enemy1Flag = false;
    public bool enemy2Flag = false;
    public bool INNFlag = false;
    public bool houseFlag = false;
    public bool ChurchFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
    }

    public void RecognitionObject(GameObject sousa)
    {
        
        enemy1 = sousa;
    }
    // Update is called once per frame

            float delayTime = 0;//クリックと同時に敵が生成されるのを防ぐ
    
    public void Update()
    {

        //if (Input.GetMouseButton(0))
        if(generateStart == true)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHitList = Physics.RaycastAll(ray).ToList();
            
            
            if (raycastHitList.Any())//虚空をクリックしていない時のみ。
            {
                var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point);
                var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

                //currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                //currentPosition.y = 0;
                float duration = 3; // 光線の出ている時間
               receiveEvent.dammy.transform.position = raycastHitList.First().point;
              
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
            }
            

            //  if (raycastHitList.First().collider != null)
            {
                // test.transform.position = raycastHitList.First().point;
            }
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
            //ヒットしたオブジェクトの名前
            //Debug.Log(hit.transform.name);
            }
            delayTime++;
           if (Input.GetMouseButtonDown(0)&& delayTime >1)
            {
                if (enemy1Flag) { Instantiate(enemy1, receiveEvent.dammy.transform.position, Quaternion.identity); }
                else if (enemy2Flag) { Instantiate(enemy2, receiveEvent.dammy.transform.position, Quaternion.identity); }
                else if (INNFlag) { Instantiate(INN, receiveEvent.dammy.transform.position, Quaternion.identity); oracle.faithPoint -= oracle.INNGeneratePoint; worldController.architectsNumber--; }
                else if( houseFlag) { Instantiate(house, receiveEvent.dammy.transform.position, Quaternion.identity); oracle.faithPoint -= oracle.HouseGeneratePoint; worldController.architectsNumber--; }
                else if (ChurchFlag) { Instantiate(church, receiveEvent.dammy.transform.position, Quaternion.identity); oracle.faithPoint -= oracle.ChurchGeneratePoint; worldController.architectsNumber--; }
                delayTime = 0;
                generateStart = false;
               // Debug.Log("ダミーの座標" + receiveEvent.dammy.transform.position);
               // Debug.Log("本当の座標" + raycastHitList.First().point);

                raycastHitList.Clear();
                receiveEvent.dammy.SetActive(false);
                enemy1Flag = false;
                enemy1Flag = false;
                INNFlag = false;
                houseFlag = false;

            }

        }
        else {
            enemy1Flag = false; INNFlag = false;   houseFlag = false;
            }

    }
  
   
}
