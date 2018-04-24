using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class CheckerInput : MonoBehaviour
{
    protected InputDataToEdge inputDataToEdge;
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
    string edgeName = "";
    abstract protected void ActiveTrigger();
    public string EdgeName
    {
        get { return edgeName; }
        protected set { edgeName = value; }
    }
    //継承したクラスでawakeを使う場合はinitを使用して下さいedgenameはここで定義
    protected abstract void Init();
    void BaseInit()
    {
        inputDataToEdge = CompornentUtility.FindCompornentOnScene<InputDataToEdge>();
    }
    private void Awake()
    {
        BaseInit();
        Init();
        inputDataToEdge.AddEdgeLayout(edgeName, this);
    }
}
