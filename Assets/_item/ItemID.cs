using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemID { Stone, Wood }
public static class ItemIDExt
{
    public static string NameItemID(this ItemID value)
    {
        string[] values = {"石","木",};
        return values[(int)value];
    }
}