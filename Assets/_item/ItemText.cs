using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemText : MonoBehaviour {
    
    public ItemID itemID;
    [SerializeField]
    Text consumptionText;
    Text quantityText;
    private void Start()
    {
        quantityText = GetComponent<Text>();
    }
    public void TextReload(int _value)
    {
        quantityText.text = "x"+_value;
    }
    public void SetConsumptionText(int num)
    {
        if (num == 0) consumptionText.text = "";
        else consumptionText.text = "-" + num;
    }
}
