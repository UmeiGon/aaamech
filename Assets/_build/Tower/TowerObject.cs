using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour {
    [SerializeField]
    CharacterUnit myUnit;
    [SerializeField]
    GameObject brokenTowerPre;
    void CraeteBrokenTowerObject()
    {
        var obj=Instantiate(brokenTowerPre,CompornentUtility.TopParent.transform);
        obj.transform.position = transform.position;
    }
    private void Start()
    {
        myUnit.AddDeathAction(CraeteBrokenTowerObject);
    }
}
