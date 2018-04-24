using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class NodeOnUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    Vector3 offSet;
    [SerializeField]
    UnityEngine.UI.Text nodeNameText;
    public Vector3 startLimitPosition;
    public Vector3 endLimitPosition;
    public AITreeGenerator aITreeGenerator;
    public CommandNode commandNode = new CommandNode();
    private Image NodeImage;
    public  Color NodeColor
    {
        get
        {
            SetImage();
            return NodeImage.color;
        }
        set
        {
            SetImage();
            NodeImage.color = value;
        }
    }
    void SetImage()
    {
        if (!NodeImage) NodeImage = GetComponent<Image>();
    }
    public void DeleteNode()
    {
        commandNode.DeleteMe();
    }
    void NameApplyText(string _name)
    {
        nodeNameText.text = _name;
    }
    private void Start()
    {
        StartCoroutine(TextApply());
    }
    IEnumerator TextApply()
    {
        while (true)
        {

            nodeNameText.text = "" + NodeDataBase.GetInstance().NodeDataList[NodeDataBase.GetInstance().FindTypeNumber(commandNode.activity)].NodeName;

            yield return null;
        }
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
        offSet = transform.position - Input.mousePosition;
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

    }
    public void OnEndDrag(PointerEventData e)
    {

    }
    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetMouseButtonDown(1))
        {
            aITreeGenerator.NodeRightClick(this);
        }
        if (Input.GetMouseButtonDown(0))
        {
            aITreeGenerator.NodeLeftClick(this);
        }
    }
}
