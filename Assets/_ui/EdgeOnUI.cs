using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeOnUI : MonoBehaviour, IPointerDownHandler
{
    public CommandEdge commandEdge = new CommandEdge();
    public UICommandManager cManager;
    bool selectFlag = false;
    public void SelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", true);
        selectFlag = true;
    }
    public void DeleteEdge()
    {
        commandEdge.DeleteMe();
    }
    public void NonSelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", false);
        selectFlag = false;
    }
    private void Start()
    {
        commandEdge.holder = transform;
        StartCoroutine(EdgeUpdate());
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetMouseButtonDown(1))
        {
            cManager.SelectEdge= this;
        }
    }
    IEnumerator EdgeUpdate()
    {
        while (true)
        {
            if (commandEdge.pre != null)
            {
                transform.position = commandEdge.pre.holder.position;
            }
            else
            {
                if(selectFlag)transform.position = Input.mousePosition;
            }
            if (commandEdge.next != null)
            {

                //node
                var m_pos = commandEdge.next.holder.position;
                var dis = Vector3.Distance(commandEdge.next.holder.localPosition, transform.localPosition);
                var diff = (m_pos - transform.position).normalized;
                //diff=Vector3.Normalize(diff);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(35, dis * (1.0f / transform.localScale.y));
                transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);


            }
            else
            {
                //mousep追従
                if (selectFlag)
                {
                    var m_pos = Input.mousePosition;
                    var A = (m_pos - transform.position);
                    A.x *= 1.0f / transform.lossyScale.x;
                    A.y *= 1.0f / transform.lossyScale.y;
                    var dis = A.magnitude;
                    var diff = (m_pos - transform.position).normalized;
                    //diff=Vector3.Normalize(diff);
                    transform.GetComponent<RectTransform>().sizeDelta = new Vector2(35, dis-1.0f);
                    diff.z = 0;
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                }
            }
            yield return null;
        }
    }

}
