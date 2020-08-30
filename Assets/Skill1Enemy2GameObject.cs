using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Enemy2GameObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hero")
        {
            other.GetComponent<HeroPrefab>().damage(5);
        }
    }
}
