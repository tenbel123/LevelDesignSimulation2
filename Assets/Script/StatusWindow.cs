using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{
    [SerializeField] HeroPrefab hero1;
    Animator animator;
    Text HPtext;
    Text ATKtext;
    Text FeeringText;
    GameObject aaa;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HPtext = this.transform.Find("ステータステキスト親/HP").GetComponent<Text>();
        ATKtext = this.transform.Find("ステータステキスト親/ATK").GetComponent<Text>();
        FeeringText = this.transform.Find("ステータステキスト親/Feering").GetComponent<Text>();
    }

    public void OnClick()
    {
        HPtext.text = "HP:"+hero1.HP.ToString();
        ATKtext.text = "ATK:" + hero1.ATK.ToString();
        Debug.Log("yobareta");
        if (animator.GetBool("Open") == false) { animator.SetBool("Open", true); } else { animator.SetBool("Open", false); }
    }
    // Update is called once per frame
    void Update()
    {
        FeeringText.text = "気分:" + hero1.Feeling.ToString();
    }
}
