using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { Tower, Mini,  Big }
public static class CharacterTypeExt
{
    public static string NameCharacterType(this CharacterType value)
    {
        string[] values = {"タワー","小型敵","大型敵", };
        return values[(int)value];
    }
}
