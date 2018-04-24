using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompornentUtility 
{
    static GameObject subParent;
    public static GameObject SubParent
    {
        get
        {
            if (subParent == null)
            {
                FindsubParent();
            }
            return subParent;
        }
    }
    static void FindsubParent()
    {
        subParent = GameObject.Find("SubParent");
    }

    static GameObject topParent;
    public static GameObject TopParent
    {
        get
        {
            if (topParent == null)
            {
                FindTopParent();
            }
            return topParent;
        }
    }
    static void FindTopParent()
    {
        topParent = GameObject.Find("MainParent");
    }
    //scene上に一つしかないcompornentを返す
    public static type FindCompornentOnScene<type>()
        where type : MonoBehaviour
    {
        if (!topParent)
        {
            FindTopParent();
        }
        if (topParent == null) return null;
        return topParent.GetComponentInChildren<type>();
    }
}
