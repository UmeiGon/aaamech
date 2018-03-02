using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIItemText : MonoBehaviour {
    
    public ItemID ID;
    [SerializeField]
    Text usetext;
    public void TextReload(int _value)
    {
        GetComponent<Text>().text = "x"+_value;
    }
    public void  UseTextInput(int num)
    {
        usetext.text = "-" + num;
    }
}
