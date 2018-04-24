using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeOnUI : MonoBehaviour, IPointerDownHandler
{
    public CommandEdge commandEdge;
    public AITreeGenerator aITreeGenerator;
    bool selectFlag = false;
    EdgeOnUI()
    {
        commandEdge = new CommandEdge
        {
            holder = this
        };
    }
    public void SelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", true);
        selectFlag = true;
    }
    public void DeleteEdge()
    {
        commandEdge.DeleteMe();
    }
    public void UnSelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", false);
        selectFlag = false;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetMouseButtonDown(1))
        {
            aITreeGenerator.SelectEdge = this;
        }
    }
    void Update()
    {
        float widthArrow = 25.0f;
        if (commandEdge.next != null)
        {
            //preがあれば
            if (commandEdge.pre != null)
            {

                var dis = Vector3.Distance(commandEdge.next.holder.transform.localPosition, commandEdge.pre.holder.transform.localPosition);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(widthArrow, dis * (1.0f / transform.localScale.y));
                //回す
                var diff = (commandEdge.next.holder.transform.position - commandEdge.pre.holder.transform.position).normalized;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                transform.localPosition = commandEdge.pre.holder.transform.localPosition + transform.right * 15.0f;
            }
            else if (selectFlag)
            {
                transform.position = Input.mousePosition;
                var dis = Vector3.Distance(commandEdge.next.holder.transform.localPosition, transform.localPosition);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(widthArrow, dis * (1.0f / transform.localScale.y));
                //回す
                var diff = (commandEdge.next.holder.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
            }
        }
        else
        {
            //mousepos追従
            if (selectFlag)
            {
                var m_pos = Input.mousePosition;
                var A = (m_pos - transform.position);
                A.x *= 1.0f / transform.lossyScale.x;
                A.y *= 1.0f / transform.lossyScale.y;
                var dis = A.magnitude;
                var diff = (m_pos - transform.position).normalized;
                //diff=Vector3.Normalize(diff);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(widthArrow, dis - 1.0f);
                diff.z = 0;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                if (commandEdge.pre != null)
                {
                    transform.localPosition = commandEdge.pre.holder.transform.localPosition;
                }
                else
                {
                    transform.position = Input.mousePosition;
                }

            }
        }

    }

}
