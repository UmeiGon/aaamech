using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//ビルドobjを配置するclass
public abstract class BuilderControllerSuperClass
{
    //建築できるかを真偽で返す
    protected abstract bool BuilderMoveUpdate();
    public virtual void InstalledBuilderObject() { }
    protected GameObject ControlObject;
    public void SetControlObject(GameObject _obj)
    {
        ControlObject = _obj;
    }
    bool IsGUIHitPointer()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        return (result.Count > 0);
    }
    public bool WrappedBuilderMoveUpdate()
    {
        return(BuilderMoveUpdate()&&!IsGUIHitPointer());
    }
}
