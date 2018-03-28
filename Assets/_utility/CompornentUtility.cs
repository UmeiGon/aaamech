using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompornentUtility : MonoBehaviour {
    static GameObject topParent;
    public static GameObject GetTopParent
    {
        get { return topParent; }
    }
    //scene上に一つしかないcompornentを返す
    public static type FindCompornentOnScene<type>()
        where type :MonoBehaviour
    {
        if (!topParent)
        {
            topParent = GameObject.Find("Parent");
        }
        if (topParent == null) return null;
        return topParent.GetComponentInChildren<type>();
    }
}
