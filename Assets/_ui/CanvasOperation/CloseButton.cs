using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour {
    [SerializeField]
    Canvas closeCanvas;
	public void CloseOnClick()
    {
        closeCanvas.enabled=false;
    }
}
