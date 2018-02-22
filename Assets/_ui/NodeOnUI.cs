using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class NodeOnUI :MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler{
    Vector3 offSet;
    public UICommandManager cManager;
    public Command commandNode = new Command();
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
        transform.position = Input.mousePosition+offSet;
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
