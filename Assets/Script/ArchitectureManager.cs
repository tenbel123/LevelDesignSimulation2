using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class ArchitectureManager : MonoBehaviour
{
    public Oracle oracle;
    public bool generateStart = false;
    public Camera mainCamera;
    [SerializeField] ReceiveEvent receiveEvent;
    public GameObject INN;
    public GameObject house;
    public GameObject church;
    public GameObject blacksmith;
    [SerializeField] WorldController worldController;
    float delayTime = 0;//クリックと同時に敵が生成されるのを防ぐ
    [SerializeField] Button[] buttons;

    public float houseGeneratePoint;
    public float INNGeneratePoint;
    public float churchGeneratePoint;
    public float blacksmithGeneratePoint;

    public Material makkuro;//建築前の真っ黒マテリアル。建築完了したら普通に戻る。
    public enum MODE_TYPE//今何の建築をしようとしている？
    {
        Neutral,//何も無し。デフォルト
        house,
        INN,
        church,
        blacksmith,
    }
    public MODE_TYPE type = MODE_TYPE.Neutral;

    // Start is called before the first frame update
    void Start()
    {
        type = MODE_TYPE.Neutral;
    }

 

    void Update()
    {

        CheckPossible();

        if (type == MODE_TYPE.Neutral) { generateStart = false; } else { generateStart = true; }
        if (generateStart == true)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHitList = Physics.RaycastAll(ray).ToList();


            if (raycastHitList.Any())//虚空をクリックしていない時のみ。
            {
                var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point);
                var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

                float duration = 3; // 光線の出ている時間
                receiveEvent.dammy.transform.position = raycastHitList.First().point;

                Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
            }


            delayTime++;//一回目のクリックで生成されてしまわないように。
            if (Input.GetMouseButtonDown(0) && delayTime > 1)
            {
               GameObject obj;
                switch (type)
                {
                    
                    case MODE_TYPE.Neutral:

                        break;
                    case MODE_TYPE.house:
                       obj =  Instantiate(house, receiveEvent.dammy.transform.position, Quaternion.identity);
                     
                        obj.GetComponent<MeshRenderer>().material = makkuro;
                        oracle.DecreaseFaith(houseGeneratePoint);
                        worldController.architectsNumber--;
                        break;
                    case MODE_TYPE.INN:
                       obj= Instantiate(INN, receiveEvent.dammy.transform.position, Quaternion.identity);
                        obj.GetComponent<MeshRenderer>().material = makkuro;

                        oracle.DecreaseFaith(INNGeneratePoint);
                        worldController.architectsNumber--;
                        break;
                    case MODE_TYPE.church:
                        obj =Instantiate(church, receiveEvent.dammy.transform.position, Quaternion.identity);
                        obj.GetComponent<MeshRenderer>().material = makkuro;

                        oracle.DecreaseFaith(churchGeneratePoint);
                        worldController.architectsNumber--;
                        break;
                    case MODE_TYPE.blacksmith:
                        obj = Instantiate(blacksmith, receiveEvent.dammy.transform.position, Quaternion.identity);
                        obj.GetComponent<MeshRenderer>().material = makkuro;
                        oracle.DecreaseFaith(blacksmithGeneratePoint);
                        worldController.architectsNumber--;
                        break;
                }
               

                delayTime = 0;
                generateStart = false;
                type = MODE_TYPE.Neutral;
                // Debug.Log("ダミーの座標" + receiveEvent.dammy.transform.position);
                // Debug.Log("本当の座標" + raycastHitList.First().point);

                raycastHitList.Clear();
                receiveEvent.dammy.SetActive(false);
                
            }
            else 
            { //type = MODE_TYPE.Neutral;
            }
        }
    }
    private void CheckPossible()//建物ごとの建築条件を満たしているかのチェック。条件を満たしていないとき、ボタンをオフにする。Updateで呼ぶ。
    {
        if(oracle.faithPoint >= houseGeneratePoint) { buttons[0].interactable = true; } else { buttons[0].interactable = false; }
        if(oracle.faithPoint >= INNGeneratePoint) { buttons[1].interactable = true; } else { buttons[1].interactable = false; }
        if (oracle.faithPoint >= churchGeneratePoint) { buttons[2].interactable = true; } else { buttons[2].interactable = false; }
        if (oracle.faithPoint >= blacksmithGeneratePoint) { buttons[3].interactable = true; } else { buttons[3].interactable = false; }

    }
    public void ButtonClick(int aaa)//建築ウィンドウの建物ボタンをクリック。その建物を建てるモードに切り替わる。
    {
        Debug.Log("aaa");
        switch (aaa)
        {
            case 0:
                type = MODE_TYPE.house;
                break;
            case 1:
                type = MODE_TYPE.INN;
                break;
            case 2:
                type = MODE_TYPE.church;
                break;
            case 3:
                type = MODE_TYPE.blacksmith;
                break;
           
        }
    }
    public void test()
    {

    }
}    

