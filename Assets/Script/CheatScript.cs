using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class CheatScript : MonoBehaviour
{
    [SerializeField] GameObject Hero;
    [SerializeField] Vector3[] positions;
    [SerializeField] GlobalClock globalClock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { Debug.Log("ti-to"); Hero.SetActive(true); }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Hero.transform.position = positions[0]; Hero.GetComponent<HeroController>().GotoNextPoint(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Hero.transform.position = positions[1]; Hero.GetComponent<HeroController>().GotoNextPoint(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Hero.transform.position = positions[2]; Hero.GetComponent<HeroController>().GotoNextPoint(); }
        if (Input.GetKeyDown(KeyCode.T)) { globalClock.localTimeScale = 10; }
        if (Input.GetKeyDown(KeyCode.Y)) { globalClock.localTimeScale = 0.1f; }


    }
}
