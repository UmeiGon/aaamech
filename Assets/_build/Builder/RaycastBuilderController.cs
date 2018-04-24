using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RaycastBuilderController : BuilderControllerSuperClass
{
    protected override bool BuilderMoveUpdate()
    {
        Vector3 obj_pos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            obj_pos = hit.point;
        }
        ControlObject.transform.position = obj_pos;
        return true;
    }
}
