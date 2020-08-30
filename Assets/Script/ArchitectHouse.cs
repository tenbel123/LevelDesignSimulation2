using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectHouse : MonoBehaviour
{
    WorldController worldController;
    // Start is called before the first frame update
    void Start()
    {
        worldController = GameObject.Find("世界の仕組みそのもの").GetComponent<WorldController>();
        worldController.MaxArchitectsNumber++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        worldController.MaxArchitectsNumber--;
    }
}
