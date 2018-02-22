using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemText : MonoBehaviour {
    
    public ItemID ID;

    public void TextReload(int _value)
    {
        GetComponent<UnityEngine.UI.Text>().text = "x"+_value;
    }
}
