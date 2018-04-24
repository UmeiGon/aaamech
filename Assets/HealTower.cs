using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : MonoBehaviour {
    UnitListCabinet unitLists;
    [SerializeField]
    int healValue = 20;
    private void Start()
    {
     unitLists=CompornentUtility.FindCompornentOnScene<UnitListCabinet>();
    }
    private void Update()
    {
        foreach(var i in unitLists.PlayerList)
        {
            if (Vector3.Distance(i.transform.position, transform.position) <= 100.0f)
            {
                i.Helth += healValue*Time.deltaTime;
            }
        }
    }
}
