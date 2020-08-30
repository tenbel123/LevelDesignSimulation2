using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCllider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            Debug.Log(other.gameObject);
            other.gameObject.GetComponent<EnemyPrefab>().Damage(5);
        }
        if(other.transform.tag== "Hero")
        {
            other.gameObject.GetComponent<HeroPrefab>().damage(5);
        } 
    }
}
