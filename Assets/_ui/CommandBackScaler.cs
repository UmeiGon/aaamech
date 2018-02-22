using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CommandBackScaler : MonoBehaviour, IScrollHandler {
   
    public void OnScroll(PointerEventData e)
    {
        //var m_pos= transform.InverseTransformPoint(e.position);
        //RectTransform _rect = GetComponent<RectTransform>();
        //_rect.pivot = new Vector2(0,0);
        //_rect.pivot = new Vector2(Mathf.Clamp(m_pos.x / _rect.sizeDelta.x,0f,1.0f),Mathf.Clamp(m_pos.y / _rect.sizeDelta.y, 0f,1.0f));
        var _value=Input.GetAxis("Mouse ScrollWheel");
        var _scale = transform.localScale.x;
        _scale += _value * Time.unscaledDeltaTime*10.0f;
        _scale= Mathf.Clamp(_scale,0.33f,1.0f);
        transform.localScale = new Vector3(_scale,_scale);
    }
   
}
