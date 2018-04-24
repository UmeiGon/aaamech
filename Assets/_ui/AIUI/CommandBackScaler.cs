using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CommandBackScaler : MonoBehaviour, IScrollHandler,IPointerDownHandler
{
    RectTransform backRect;
    RectTransform myRect;
    Vector3 startPos;
   [SerializeField]
    Transform backObj;
    private void Start()
    {
        backRect = backObj.GetComponent<RectTransform>();
        myRect = GetComponent<RectTransform>();
        startPos = backObj.position;
    }
    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(ScaleMoveing());
        }
    }
    IEnumerator ScaleMoveing()
    {
        
        Vector3 prePos=Input.mousePosition;
        while (!Input.GetKeyUp(KeyCode.Mouse1))
        {
            var nowPos = Input.mousePosition;
            var diff = nowPos - prePos;
            backRect.Translate(diff);
            prePos = nowPos;
            BackPositionFix();                
            yield return null;
        }
        yield return null;
    }
    public void OnScroll(PointerEventData e)
    {
        var _value = Input.GetAxis("Mouse ScrollWheel");
        if ((backObj.localScale.x <= 0.33f && _value < 0) || backObj.localScale.x >= 1.0f && _value > 0) return;
        var dist = (backObj.position - Input.mousePosition);
        var _scale = backObj.localScale.x;
        var preScale = _scale;
        _value *= Time.unscaledDeltaTime * 10.0f;
        _scale += _value;
        _scale = Mathf.Clamp(_scale, 0.33f, 1.0f);
        backObj.localScale = new Vector3(_scale, _scale);
        backObj.position = Input.mousePosition + ((Vector3)dist * (_scale) / (preScale));
        BackPositionFix();
       
    }
    void BackPositionFix()
    {
        //position押し戻し
        if (backObj.position.y < transform.position.y)
        {
            backObj.position = new Vector3(backObj.position.x, transform.position.y, 0);
        }
        if (backObj.position.x > transform.position.x)
        {

            backObj.position = new Vector3(transform.position.x, backObj.position.y, 0);
        }
        //右下のポジション
        var scaleRightDownPos = new Vector2(backObj.position.x + backRect.sizeDelta.x * backRect.lossyScale.x, backObj.position.y - backRect.sizeDelta.y * backRect.lossyScale.y);
        var myRightDownPos = new Vector2(transform.position.x + myRect.sizeDelta.x * myRect.lossyScale.x, transform.position.y - myRect.sizeDelta.y * myRect.lossyScale.y);
        var diff = myRightDownPos - scaleRightDownPos;
        if (scaleRightDownPos.y > myRightDownPos.y)
        {
            backObj.Translate(0, diff.y, 0);
        }
        if (scaleRightDownPos.x < myRightDownPos.x)
        {
            backObj.Translate(diff.x, 0, 0);
        }
    }
}
