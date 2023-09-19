
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollviewControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect scrollView;

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollView.OnBeginDrag(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        scrollView.OnDrag(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        scrollView.OnEndDrag(eventData);
    }
}