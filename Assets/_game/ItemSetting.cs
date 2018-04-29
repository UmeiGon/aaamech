using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour {
    [SerializeField]
    int ironValue;
    private void Start()
    {
        CompornentUtility.FindCompornentOnScene<ItemManager>().itemDataTable[(int)ItemID.Iron].Value = ironValue;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
