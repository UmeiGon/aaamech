using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInstaller : MonoBehaviour {
    [SerializeField]
    BuilderUnit myUnit;
    [SerializeField]
    GameObject buildObject;
    [SerializeField]
    GameObject EmptyBuiderObj;
    private Material mat;
    private void Awake()
    {
        myUnit.AddDeathAction(PopBuildObj);
        mat = EmptyBuiderObj.GetComponent<MeshRenderer>().material;
    }
    public void ChangeBuilderColor(Color _color)
    {
        mat.color = _color;
    }
    public void PopBuildObj()
    {
        buildObject.SetActive(true);
        buildObject.transform.parent = CompornentUtility.TopParent.transform;
    }
}
