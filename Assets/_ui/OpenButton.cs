using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButton : MonoBehaviour {
    [SerializeField]
    Canvas openCanvas;
    public void Open()
    {
        openCanvas.enabled = true;
    }
}
