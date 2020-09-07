
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotator : MonoBehaviour
{
    // カメラオブジェクトを格納する変数
    public Camera mainCamera;
    // カメラの回転速度を格納する変数
    public Vector2 rotationSpeed;
    public Vector2 movingSpeed;

    // マウス移動方向とカメラ回転方向を反転する判定フラグ
    public bool reverse;
    // マウス座標を格納する変数
    private Vector2 lastMousePosition;
    // カメラの角度を格納する変数（初期値に0,0を代入）
    private Vector2 newAngle = new Vector2(0, 0);
    private Vector3 newPosition = new Vector3(0, 0,0);
    private Vector3 newPositionFlont = new Vector3(0, 0, 0);

    private Vector2 lastMousePosition2;

    float x;
    float z;
    private Vector3 ParentPosition ;
   
    [SerializeField] float scroolSpeed;


    float a;
    float b;
    float newAngle2;
    public GameObject test;
    private Vector2 ParentAngle = new Vector2(0, 0);
    [SerializeField] Slider slider;　//カメラの高さ。地面に近かったり遠かったり。
    float depth; //上のバリュー
    [SerializeField] Vector3 StartPosition;


    private void Awake()
    {
        slider.value = 13;
        ParentPosition = StartPosition;
    }
    void Update()
    {
      

        if (Input.GetMouseButtonDown(1))
        {
            // カメラの角度を変数"newAngle"に格納
            newAngle = mainCamera.transform.localEulerAngles;
            ParentAngle = test.transform.localEulerAngles;

            // マウス座標を変数"lastMousePosition"に格納
            lastMousePosition = Input.mousePosition;
        }
        // 左ドラッグしている間
        else if (Input.GetMouseButton(1))
        {
            //カメラ回転方向の判定フラグが"true"の場合
            if (!reverse)
            {
                // Y軸の回転：マウスドラッグ方向に視点回転
                // マウスの水平移動値に変数"rotationSpeed"を掛ける
                //（クリック時の座標とマウス座標の現在値の差分値）
                ParentAngle.y -= (lastMousePosition.x - Input.mousePosition.x) * rotationSpeed.y;
                // X軸の回転：マウスドラッグ方向に視点回転
                // マウスの垂直移動値に変数"rotationSpeed"を掛ける
                //（クリック時の座標とマウス座標の現在値の差分値）
                
               newAngle.x =  Mathf.Clamp( newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * rotationSpeed.x, 1, 89);//角度に制限を付けるよ。バグを防ぐため
                // "newAngle"の角度をカメラ角度に格納
                mainCamera.transform.localEulerAngles =  newAngle;
                test.transform.localEulerAngles = ParentAngle;
                // マウス座標を変数"lastMousePosition"に格納
                lastMousePosition = Input.mousePosition;
            }
            // カメラ回転方向の判定フラグが"reverse"の場合
            else if (reverse)
            {
                // Y軸の回転：マウスドラッグと逆方向に視点回転
                newAngle.y -= (Input.mousePosition.x - lastMousePosition.x) * rotationSpeed.y;
                // X軸の回転：マウスドラッグと逆方向に視点回転
                newAngle.x -= (lastMousePosition.y - Input.mousePosition.y) * rotationSpeed.x;
                // "newAngle"の角度をカメラ角度に格納
                mainCamera.transform.localEulerAngles = newAngle;
                // マウス座標を変数"lastMousePosition"に格納
                lastMousePosition = Input.mousePosition;
            }
        }

       if (Input.GetMouseButtonDown(2))
        {
            ParentPosition = test.transform.position;
            newPosition = test.transform.position;
            newPositionFlont = test.transform.position;
            lastMousePosition2 = Input.mousePosition;
           
        }
       else if (Input.GetMouseButton(2))
        {
            /*
            float aaa = Mathf.InverseLerp(75,0,transform.eulerAngles.x);//角度が0に近ければ1になる。４５以上で０になる
            float bbb = Mathf.InverseLerp(65, 90, transform.eulerAngles.x);//９０に近ければ１になる。
            Debug.Log(aaa+bbb);
            
            

            newPosition =(lastMousePosition2.x - Input.mousePosition.x) * mainCamera.transform.right*movingSpeed.x;
            newPositionFlont = (lastMousePosition2.y - Input.mousePosition.y) * mainCamera.transform.forward * movingSpeed.y *(aaa+bbb)*transform.eulerAngles.x/5;//問題点。カメラの前方向を取得して、それをz軸だけで移動しているから真下を向いているときに前に進まない（前は地面なので）。だからオイラー角のｘをかける。


            //mainCamera.transform.position += newPositionFlont;
            //mainCamera.transform.position += newPosition;

            CameraPosition.x += (newPosition.x + newPositionFlont.x);
              CameraPosition.z += (newPosition.z + newPositionFlont.z);

         
     


            // mainCamera.transform.position += newPositionFlont;
            lastMousePosition2 = Input.mousePosition;
           mainCamera.transform.position = CameraPosition;
        */
            newPosition = (lastMousePosition2.x - Input.mousePosition.x) * test.transform.right * (movingSpeed.x/100)*slider.value;
            newPositionFlont = (lastMousePosition2.y - Input.mousePosition.y) * test.transform.forward * (movingSpeed.y/100 )* slider.value ;
            ParentPosition.x += (newPosition.x + newPositionFlont.x);
            ParentPosition.z += (newPosition.z + newPositionFlont.z);
          //  test.transform.position = ParentPosition;
            //test. transform.Translate((lastMousePosition2.x - Input.mousePosition.x)*movingSpeed.x, 0, (lastMousePosition2.y - Input.mousePosition.y)*movingSpeed.y);
            lastMousePosition2 = Input.mousePosition;

        }



        float scroll = Input.GetAxis("Mouse ScrollWheel");
       
        slider.value -= scroll * scroolSpeed;     
        ParentPosition.y = slider.value;
        test.transform.position = ParentPosition;
       

    }

    // マウスドラッグ方向と視点回転方向を反転する処理
    public void DirectionChange()
    {
        // 判定フラグ変数"reverse"が"false"であれば
        if (!reverse)
        {
            // 判定フラグ変数"reverse"に"true"を代入
            reverse = true;
        }
        // でなければ（判定フラグ変数"reverse"が"true"であれば）
        else
        {
            // 判定フラグ変数"reverse"に"false"を代入
            reverse = false;
        }
    }
}
 