using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemData{
    int value=0;
    public delegate void valueChangeTrigger(int _value);
    //基本Awakeでこのデリゲートに追加
    public valueChangeTrigger valueChanged;
    public int Value
    {
        set
        {
            this.value = value;
            valueChanged(this.value);
        }
        get
        {
            return this.value;
        }
    }
}
