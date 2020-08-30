using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckParticleScript : MonoBehaviour
{
    Vector3 startPosition;
    EnemyPrefab Parent;
    float rate;
    // Start is called before the first frame update
    void Start()
    {
        
        Parent = transform.parent.GetComponent<EnemyPrefab>();
        startPosition = Parent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rate++;
        transform.position   = Vector3.Lerp(startPosition, Parent.target.position,rate/100 );
        if(rate > 150) { Parent.heroPrefab.damage(Parent.ATK); Destroy(gameObject); }
        
    }
}
