using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class NodeCreator:MonoBehaviour,IPointerDownHandler {
    AITreeGenerator aITreeGenerator;
    private void Start()
    {
        aITreeGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
    }
    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetKeyDown(KeyCode.Mouse2)&&aITreeGenerator.CanEdit)
        {
            aITreeGenerator.CreateDefaultNode(e.position);
        }
    }
	
}
