using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIController : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] Text tekiText;
    public GameObject teki;
    [SerializeField] EnemyDragGenerate enemyDragGenerate;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Instantiate(teki);
        enemyDragGenerate.RecognitionObject(teki);
       // teki.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("dorag");
       // enemyDragGenerate.enemyGenerateController();
       // teki.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

}
