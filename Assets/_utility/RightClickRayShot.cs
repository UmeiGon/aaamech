using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickRayShot  {

    //引数のキーをクリックした時、マウスの位置からrayを飛ばして指定のclassを返す。
	public static T GetMouseRayHitObject<T>(KeyCode key_code)
        where T:MonoBehaviour
    {
        if (Input.GetKeyDown(key_code))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                T retObj = hit.transform.GetComponent<T>();
                if (retObj)
                {
                    return retObj;
                }
            }
        }
        return null;
    }
}
