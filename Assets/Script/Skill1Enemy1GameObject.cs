using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Enemy1GameObject : MonoBehaviour
{
    Rigidbody rb;
    GameObject target;
    float time;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    { rb = GetComponent<Rigidbody>();
        rb.mass = 2;
       target = GameObject.Find("Hero");
    //    rb = GetComponent<Rigidbody>();
        Debug.Log(rb);
        rb.AddForce(0, 2000, 0);
        time = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xSpeed = target.transform.position.x - transform.position.x;
        float zSpeed = target.transform.position.z - transform.position.z;
        time += Time.deltaTime;
        //rb.transform.LookAt(target.transform.position);
        if (time > 1.5f)
        {
            rb.AddForce(new Vector3(xSpeed, 0, zSpeed) * 15 * Time.deltaTime);
        } 
    }
}
