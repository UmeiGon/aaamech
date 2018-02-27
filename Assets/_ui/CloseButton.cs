using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour {
    [SerializeField]
    GameObject closeObject;
	public void CloseOnClick()
    {
        closeObject.SetActive(false);
    }
}
