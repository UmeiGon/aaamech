using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedPositonBuilderController : BuilderControllerSuperClass
{
    BuildingInstaller bInstaller;
    BrokenTower hitBrokeTower;
    protected override bool BuilderMoveUpdate()
    {
        if (!bInstaller)
        {
            bInstaller = ControlObject.GetComponent<BuildingInstaller>();
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            BrokenTower bt = hit.transform.GetComponent<BrokenTower>();
            if (bt!=null)
            {
                hitBrokeTower = bt;
                //設置可能
                ControlObject.transform.position = hit.transform.position;
                bInstaller.ChangeBuilderColor(Color.blue);
                return true;
            }
            else
            {
                //設置不可
                ControlObject.transform.position = hit.point;
                bInstaller.ChangeBuilderColor(Color.red);
                return false;
            }
        }
        return false;
    }
    public override void InstalledBuilderObject()
    {
        hitBrokeTower.Sucide();
    }
}
