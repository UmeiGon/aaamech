using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialID
{
    Tree, Stone
}
public static class MaterialIDExt
{
    public static string NameMaterialID(this MaterialID value)
    {
        string[] values = { "木", "岩", };
        return values[(int)value];
    }
}
