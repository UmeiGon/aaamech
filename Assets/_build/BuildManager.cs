using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//建造物を設置する時などに使うマネージャー
public class BuildManager : MonoBehaviour
{ 
    GameObject buildUnit = null;
    
    PlayerItemManager pItemManager;
    private void Start()
    {
        pItemManager = GameObject.Find("Parent").GetComponentInChildren<PlayerItemManager>();
  

        StartCoroutine(Building());
    }
    public bool CheckIfBuildable(BuildID num)
    {
        if (BuildID.NONE == num) return false;
        foreach (var i in BuildDataCabinet.Instance.buildDataTable[(int)num].consumptionItemData)
        {
            if (pItemManager.itemDataTable[i.Key].Value < i.Value)
            {
                return false;
            }
        }
        return true;
    }
    void BuildStart(BuildID build_id)
    {
        buildUnit = null;
        buildUnit = Instantiate(BuildDataCabinet.Instance.buildDataTable[(int)build_id].objPre, GameObject.Find("Parent").transform);
    }
    IEnumerator Building()
    {
        while (true)
        {
            Vector3 obj_pos = Vector3.zero;
            if (buildNum != BuildID.NONE)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    obj_pos = hit.point;
                }
            }
            switch (buildNum)
            {
                case BuildID.NONE:
                    break;
                case BuildID.Bridge:
                case BuildID.Tower:
                    buildUnit.transform.position = obj_pos;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        buildUnit.GetComponent<BuildUnit>().enabled = true;
                        pItemManager.UsingItem(buildDataTable[(int)buildNum].consumptionItemData);
                        buildNum = BuildID.NONE;
                    }
                    break;
                default:
                    break;
            }
            yield return null;
        }
    }
}
