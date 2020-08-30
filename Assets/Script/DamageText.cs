using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    public void Awake()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }
    public void Start()
    {
    
        transform.DOLocalMoveY(5f, 1f);
        textMesh.DOFade(0f, 1f).OnComplete(() => { Destroy(gameObject); });

    }
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    public void Number(float damage)
    {
       textMesh.text = damage.ToString();
    }
}
