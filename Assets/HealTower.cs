using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : MonoBehaviour {
    UnitLists unitLists;
    [SerializeField]
    int healValue = 20;
    private void Start()
    {
     unitLists= GameObject.Find("Parent").GetComponentInChildren<UnitLists>();
    }
    private void Update()
    {
        foreach(var i in unitLists.playerList)
        {
            if (Vector3.Distance(i.transform.position, transform.position) <= 100.0f)
            {
                i.Helth += healValue*Time.deltaTime;
            }
        }
    }
}
