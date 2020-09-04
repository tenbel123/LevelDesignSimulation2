using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBabyDate : MonoBehaviour
{
    public int ID;
    public int LV;
    public float Gold;
    public int slot;//召喚する場所
    Text name;
    Text LVtext;

    // Start is called before the first frame update
    void Start()
    {
        ID = 3;//最初は謎の数値を入れておく。
      //  name = transform.Find("名前").GetComponent<Text>();
       // LVtext = transform.Find("レベル").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       
       // LVtext.text = "LV"+LV;
   
    }
    public void ResetDate()
    {
        ID = 3;
        LV = 0;
        Gold = 0;
        slot = 0;
    }
}
