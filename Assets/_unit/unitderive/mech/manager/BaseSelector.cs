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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    BaseUnit baseUnit = hit.transform.GetComponent<BaseUnit>();
                    if (baseUnit)
                    {

                        mechGenerator.SelectBase = baseUnit;
                    }
                }
            }
            yield return null;
        }   
    }
}
