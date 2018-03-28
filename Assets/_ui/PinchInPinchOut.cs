using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PinchInPinchOut : MonoBehaviour,IPointerUpHandler {
    RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    public void OnPointerUp(PointerEventData e)
    {
       
    }
}
