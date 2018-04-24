using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemData{
    int value=0;
    //基本Awakeでこのデリゲートに追加
    Action<int> valueChanged;
    public void AddValueChangedTrigger(Action<int> func)
    {
        valueChanged += func;
    }
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
