using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;

    private RectTransform rectParent;
    private RectTransform rectHp;

    public Vector3 offset;
    public Transform target;    

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

        
        if (screenPos.z < 0.0f)
            screenPos *= -1.0f;

        /*
        Vector2 localPos = Vector2.zero;        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectParent, screenPos, uiCamera, out localPos
            );

        rectHp.localPosition = localPos;
        */
        
        rectHp.localPosition = screenPos;
        //print("ScreenPos : " + Input.mousePosition);
    }
}
