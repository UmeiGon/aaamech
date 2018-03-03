using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum BuildID
{
    NONE = -1,
    Bridge = 0,
    Tower,
}
public  class BuildData
{
    public BuildID id;
    public GameObject objPre=null;
    //key...itemID,value...個数
    public Dictionary<int, int> useItems=null;
    public BuildData(BuildID id,string resources_name, Dictionary<int, int> use_items)
    {
        this.id = id;
        if (id != BuildID.NONE)
        {
            objPre = Resources.Load(resources_name) as GameObject;
            useItems = use_items;
        }
       
    }
}

//建造物を設置する時などに使うマネージャー
public class BuildManager : MonoBehaviour
{

    BuildID buildNum=BuildID.NONE;
    public BuildID BuildNum
    {
        set
        {
            buildUnit=null;
            buildNum = value;
            buildUnit = Instantiate(buildDataTable[(int)buildNum].objPre,GameObject.Find("Parent").transform);
        }
        get {return buildNum; }
    }
    GameObject buildUnit = null;
    public Dictionary<int, BuildData> buildDataTable;
    PlayerItemManager pItemManager;
    private void Start()
    {
        pItemManager = GameObject.Find("Parent").GetComponentInChildren<PlayerItemManager>();
        var buildDataList = new List<BuildData>
        {
            new BuildData(BuildID.NONE, "null", null),
            new BuildData(BuildID.Bridge, "BridgeUnit", new Dictionary<int, int>() { { (int)ItemID.Wood, 20 } }),
            new BuildData(BuildID.Tower, "HealTower", new Dictionary<int, int>() { { (int)ItemID.Stone, 30 } }),
        };
        //todictionaryでidをkeyにしたdictionaryを作る
        buildDataTable = buildDataList.ToDictionary(d=>(int)d.id);
        StartCoroutine(Building());
    }
   public bool CanBuildCheck(BuildID num)
    {
        if (BuildID.NONE == num) return false;
        foreach (var i in buildDataTable[(int)num].useItems)
        {
            if (pItemManager.itemDataTable[i.Key].Value < i.Value)
            {
                return false;
            }
        }
        return true;
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
                        pItemManager.UsingItem(buildDataTable[(int)buildNum].useItems);
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
