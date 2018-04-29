using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//建造物を設置する時などに使うマネージャー
public class BuildManager : MonoBehaviour
{ 
    ItemManager itemManager;
    IEnumerator currentBuildRoutine;
    GameObject buildUnitObj;
    private void Start()
    {
        itemManager = CompornentUtility.FindCompornentOnScene<ItemManager>();
    }
    public bool CheckIfBuildable(BuildID num)
    {
       
        foreach (var i in BuildDataCabinet.Instance.buildDataTable[(int)num].consumptionItemData)
        {
            if (itemManager.itemDataTable[i.Key].Value < i.Value)
            {
                return false;
            }
        }
        return true;
    }
    public void BuildStart(BuildID build_id)
    {
        CancelBuild();
        currentBuildRoutine = BuildRoutine(build_id);
        StartCoroutine(currentBuildRoutine);
    }
    public void CancelBuild()
    {
        if (currentBuildRoutine != null) StopCoroutine(currentBuildRoutine);
        Destroy(buildUnitObj);
        currentBuildRoutine = null;
    }
    IEnumerator BuildRoutine(BuildID build_id)
    {
        buildUnitObj = Instantiate(BuildDataCabinet.Instance.buildDataTable[(int)build_id].ObjPre,CompornentUtility.TopParent.transform);
        var buiderCon= BuildDataCabinet.Instance.buildDataTable[(int)build_id].CreateBuilderControllerInstance();
        buiderCon.SetControlObject(buildUnitObj);
        while (true)
        {

            //positionのupdate設置可能ならtrueが入る
            bool canInstallation = buiderCon.WrappedBuilderMoveUpdate();

            //設置処理
            if (Input.GetKeyDown(KeyCode.Mouse0)&& canInstallation)
            {
                buildUnitObj.GetComponent<BuilderUnit>().enabled = true;
                itemManager.UsingItem(BuildDataCabinet.Instance.buildDataTable[(int)build_id].consumptionItemData);
                buildUnitObj = null;

                buiderCon.InstalledBuilderObject();
                break;
            }
            else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                CancelBuild();
            }
            yield return null;
        }
    }
}
