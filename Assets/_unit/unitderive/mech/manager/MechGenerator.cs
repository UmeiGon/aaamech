using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechGenerator : MonoBehaviour {
    [SerializeField]
    GameObject mechPre;
    public BaseUnit selectBase;
    ItemManager itemManager;
    [SerializeField]
    int consumeIronValue;
    public int ConsumeIronValue
    {
        get { return consumeIronValue; }
    }
    private void Start()
    {
        if (selectBase == null)
        {
            selectBase = GameObject.Find("FirstBase").GetComponent<BaseUnit>();
        }
        itemManager = CompornentUtility.FindCompornentOnScene<ItemManager>();
    }
    public void GenerateMech(MechAITree ai_tree)
    {
        if (!selectBase) return;
        itemManager.itemDataTable[(int)ItemID.Iron].Value-= consumeIronValue;
        var mech=Instantiate(mechPre,selectBase.transform.position,Quaternion.identity,CompornentUtility.TopParent.transform);
        mech.GetComponent<MechController>().SetAITree(ai_tree);
    }
}
