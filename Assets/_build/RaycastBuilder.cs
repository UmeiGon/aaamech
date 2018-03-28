using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBuilder : BuilderSuperClass
{
    protected override bool BuilderUpdate()
    {
        Vector3 obj_pos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            obj_pos = hit.point;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            buildUnit.GetComponent<BuildUnit>().enabled = true;
            pItemManager.UsingItem(buildDataTable[(int)buildNum].consumptionItemData);
            buildNum = BuildID.NONE;
        }

    }
}
