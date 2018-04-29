using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class EdgeOnUI : MonoBehaviour, IPointerDownHandler
{
    CommandEdge commandEdge;
    Vector3 preHeadPositon;
    public CommandEdge CommandEdge
    {
        get { return commandEdge; }
        set
        {
            commandEdge = value;
            if(commandEdge!=null)commandEdge.holder = this;
        }
    }
    public AITreeGenerator aITreeGenerator;
    bool isSelected = false;
    RectTransform rectTransform;
    public void SelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", true);
        isSelected = true;
    }
    public void Sucide()
    {
        CommandEdge.ReferenceRemoving();
        CommandEdge.holder = null;
        CommandEdge = null;
        Destroy(gameObject);
    }
    public void UnSelectTrigger()
    {
        GetComponent<Animator>().SetBool("selectFlag", false);
        isSelected = false;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetMouseButtonDown(1))
        {
            aITreeGenerator.SelectEdge = this;
        }
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void SettingPosition(Vector3 origin_pos, Vector3 head_pos)
    {
        float widthArrow = 25.0f;

        //2点間の距離を取り、UIのサイズを設定
        var dis = Vector3.Distance(head_pos, origin_pos);
        rectTransform.sizeDelta = new Vector2(widthArrow, dis * (1.0f / transform.lossyScale.y));

        //pre->nextのベクトルを取り、UIの角度を設定
        var diff = (head_pos - origin_pos).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);

        //エッジが被らないようにするために、UIを横にずらす
        transform.position = origin_pos + transform.right * 15.0f;

        preHeadPositon=head_pos;
    }
    void Update()
    {
        Vector3 origin=Vector3.zero,head=Vector3.zero;
        var mouse = Input.mousePosition;
        if (CommandEdge.next != null)
        {
            head = CommandEdge.next.holder.transform.position;
        }
        else
        {
            if (isSelected)
            {
                head = mouse;
            }
        }
        if (commandEdge.pre!=null)
        {
            origin = commandEdge.pre.holder.transform.position;
        }
        else
        {
            if (isSelected)
            {
                origin = mouse;
            }
        }
        SettingPosition(origin, head);
    }

}
