using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject dragginItem = null;

    private Transform itemListTr;
    private Transform inventoryTr;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        inventoryTr = GameObject.Find("Inventory").transform;
        itemListTr = GameObject.Find("ItemList").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(inventoryTr);
        dragginItem = gameObject;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragginItem = null;
        canvasGroup.blocksRaycasts = true;

        if(transform.parent == inventoryTr)
        {
            transform.SetParent(itemListTr);

            GameManager.instance.RemoveItem(GetComponent<ItemInfo>().itemInfo);
        }
    }
}
