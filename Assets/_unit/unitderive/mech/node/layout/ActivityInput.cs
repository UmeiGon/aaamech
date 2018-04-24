using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class ActivityInput : MonoBehaviour
{
    //ここでacitivityNameの定義をしてください
    protected abstract void Init();
    protected InputDataToNode inputDataToNode;
    string acitivityName = "";
    [SerializeField]
    GameObject layoutObj;
    public void LayoutSetActive(bool _active)
    {
        if (layoutObj)
        {
            layoutObj.SetActive(_active);
        }
        if (_active)
        {
            ActiveTrigger();
        }
    }
    abstract protected void ActiveTrigger();
    public string AcitivityName
    {
        get { return acitivityName; }
        protected set { acitivityName = value; }
    }
    void BaseInit()
    {
        inputDataToNode = CompornentUtility.FindCompornentOnScene<InputDataToNode>();
    }
    private void Awake()
    {
        BaseInit();
        Init();
        inputDataToNode.AddNodeLayOut(acitivityName, this);
    }


}
