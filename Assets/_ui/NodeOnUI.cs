using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class NodeOnUI :MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler{
    Vector3 offSet;
    public Vector3 startLimitPosition;
    public Vector3 endLimitPosition;
    public UICommandManager cManager;
    public Command commandNode = new Command();
    public void DeleteNode()
    {
        commandNode.DeleteMe();
    }
    private void Start()
    {
        commandNode.holder = transform;
    }
    public void SelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", true);
    }
    public void NonSelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", false);
    }
    public void OnBeginDrag(PointerEventData e)
    {
        offSet=transform.position - Input.mousePosition;
    }
    public void OnDrag(PointerEventData e)
    {
        transform.position = Input.mousePosition + offSet;
        if (startLimitPosition.x > transform.localPosition.x)
        {
            transform.localPosition = new Vector2(startLimitPosition.x, transform.localPosition.y);
        }
        if (endLimitPosition.x < transform.localPosition.x)
        {
            transform.localPosition = new Vector2(endLimitPosition.x, transform.localPosition.y);
        }
        if (startLimitPosition.y < transform.localPosition.y)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, startLimitPosition.y);
        }
        if (endLimitPosition.y > transform.localPosition.y)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, endLimitPosition.y);
        }
        //if (Input.mousePosition.x<startLimitPosition.x|| Input.mousePosition.y < startLimitPosition.y|| Input.mousePosition.x > endLimitPosition.x || Input.mousePosition.y > endLimitPosition.y)
        //{

        //}
      
    }
    public void OnEndDrag(PointerEventData e)
    {
      
    }
    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetMouseButtonDown(1))
        {
            cManager.NodeRightClick(this);
        }
        if (Input.GetMouseButtonDown(0))
        {
            cManager.NodeLeftClick(this);
        }
    }
}
