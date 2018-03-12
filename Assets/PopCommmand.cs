using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PopCommmand :MonoBehaviour,IPointerDownHandler {
    [SerializeField]
    UICommandManager uiCommandMane;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData e)
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            uiCommandMane.CreateCommandNode(e.position);
        }
    }
	
}
