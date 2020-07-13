using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;

public class Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            Drag.dragginItem.transform.SetParent(transform);

            Item item = Drag.dragginItem.GetComponent<ItemInfo>().itemInfo;
            GameManager.instance.AddItem(item);
        }
    }
}
