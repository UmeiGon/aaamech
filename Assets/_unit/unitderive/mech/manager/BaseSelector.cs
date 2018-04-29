using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSelector : MonoBehaviour {
    MechGenerator mechGenerator;
    private void Start()
    {
        mechGenerator = CompornentUtility.FindCompornentOnScene<MechGenerator>();
        StartCoroutine(SelectRoutine());     
    }
    IEnumerator SelectRoutine()
    {
        while (true)
        {
            BaseUnit baseUnit;
            if ((baseUnit = RightClickRayShot.GetMouseRayHitObject<BaseUnit>(KeyCode.Mouse0)) != null)
            {
                Debug.Log(baseUnit);
                mechGenerator.SelectBase = baseUnit;
            }
            yield return null;
        }   
    }
}
