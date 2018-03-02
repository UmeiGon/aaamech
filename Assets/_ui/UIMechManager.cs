using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMechManager : MonoBehaviour
{
    [SerializeField]
    Dropdown mechsDropDown;
    [SerializeField]
    Dropdown mechCommandDropDown;
    MechManager mechManager;
    [SerializeField]
    Text attackText;
    [SerializeField]
    Text helthText;
    [SerializeField]
    Toggle allSelectToggle;
    [SerializeField]
    Transform createPos;
    [SerializeField]
    GameObject mechPre;
    bool allSelectFlag = false;
    bool blockSelectMode = false;
    
    private void Start()
    {
        var p = GameObject.Find("Parent");
        mechManager = p.GetComponentInChildren<MechManager>();
        mechCommandDropDown.ClearOptions();
       
        mechManager.selectTriggerFuncs = MechDropDownReload;
        //StartCoroutine(ScreenSelectBlock());
        AllSelectFlagChange();
    }
    private Vector3 startPos, endPos;
    public UnityEngine.UI.Image selectImage;
    IEnumerator ScreenSelectBlock()
    {
        while (true)
        {
            ClickTargetUnit();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //uiに触ってなかったら
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    startPos = Input.mousePosition;
                    selectImage.gameObject.SetActive(true);
                    selectImage.transform.position = startPos;
                    blockSelectMode = true;
                }

            }
            if (blockSelectMode)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    var sub_pos = Input.mousePosition - startPos;
                    selectImage.rectTransform.pivot = new Vector2(0, 0);
                    if (sub_pos.x < 0)
                    {
                        selectImage.rectTransform.pivot = new Vector2(1, selectImage.rectTransform.pivot.y);
                        sub_pos.x *= -1;
                    }
                    if (sub_pos.y < 0)
                    {
                        selectImage.rectTransform.pivot = new Vector2(selectImage.rectTransform.pivot.x, 1);
                        sub_pos.y *= -1;
                    }

                    selectImage.rectTransform.sizeDelta = sub_pos;
                }
                //選択終了
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    //startとendを正常な位置に
                    endPos = Input.mousePosition;
                    if (endPos.x < startPos.x)
                    {
                        float a = startPos.x;
                        startPos.x = endPos.x;
                        endPos.x = a;
                    }

                    if (endPos.y < startPos.y)
                    {
                        float a = startPos.y;
                        startPos.y = endPos.y;
                        endPos.y = a;
                    }

                    foreach (var i in mechManager.MechList)
                    {
                        i.selectEffect.SetActive(false);
                        i.selectBall.SetActive(false);
                    }
                    mechManager.selectMechList.Clear();
                    if (2.0f > Vector3.Distance(startPos, endPos))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 1000))
                        {
                            if (hit.transform.GetComponent<MechUnit>() != null)
                            {
                                MechUnit _unit = hit.transform.GetComponent<MechUnit>();
                                mechManager.selectMechList.Add(_unit);
                                _unit.selectEffect.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        foreach (var i in mechManager.MechList)
                        {
                            var m_pos = Camera.main.WorldToScreenPoint(i.transform.position);
                            if (startPos.x < m_pos.x && startPos.y < m_pos.y && endPos.x > m_pos.x && endPos.y > m_pos.y)
                            {
                                mechManager.selectMechList.Add(i);
                                i.selectEffect.SetActive(true);
                            }
                        }
                    }
                    selectImage.gameObject.SetActive(false);
                    selectImage.rectTransform.sizeDelta = new Vector2(0, 0);
                    mechManager.selectTriggerFuncs();
                    blockSelectMode = false;
                }
            }
            yield return null;
        }
    }
    //敵や建造物をクリックして選択しているmechのターゲットにする。
    void ClickTargetUnit()
    {
        if (mechManager.selectMechList.Count == 0) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse1) && Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.transform.GetComponent<Unit>() == null) return;
            Unit _unit = hit.transform.GetComponent<Unit>();
            if (_unit.GetType() != typeof(MechUnit))
            {
                if (mechManager.selectUnit != null) mechManager.selectUnit.selectEffect.SetActive(false);
                UnitIntoSelectMechsTarget(_unit);
            }
        }
    }
  
    public void CreateMechInstance(MechAITree mechAI)
    {
        var m =Instantiate(mechPre,GameObject.Find("Parent").transform);
        m.transform.position = createPos.position;
        m.GetComponent<MechController>().SetAITree(mechAI);
    }
    void UnitIntoSelectMechsTarget(Unit _unit)
    {
        if (allSelectFlag)
        {
            foreach (var i in mechManager.selectMechList)
            {
                i.mechCon.SetTarget(_unit);
            }
        }
        else
        {
            mechManager.selectMechList[mechsDropDown.value].mechCon.SetTarget(_unit);
        }
    }
    //commandを変更するときに選択した全てのmechを変更するのかを決める。
    public void AllSelectFlagChange()
    {
        allSelectFlag = allSelectToggle.isOn;
    }
    //mechのcommandを変更する
    public void MechCommmandChange()
    {
        if (allSelectFlag)
        {
            foreach (var i in mechManager.selectMechList)
            {
                i.mechCon.SetMode(mechCommandDropDown.value);
            }
        }
        else
        {
            mechManager.selectMechList[mechsDropDown.value].mechCon.SetMode(mechCommandDropDown.value);
        }

    }
    //selectMechListが更新されるたびに呼び出す
    void MechDropDownReload()
    {
        //dropdown初期化
        mechsDropDown.ClearOptions();
        mechCommandDropDown.ClearOptions();
        //mechsdropdownに反映
        List<string> slist = new List<string>();
        int num = 1;
        foreach (var i in mechManager.selectMechList)
        {
            slist.Add("" + num);
            num++;
        }
        mechsDropDown.AddOptions(slist);
        mechsDropDown.value = 0;
        //ステータス等に数値を反映
        if (mechManager.selectMechList.Count == 0)
        {
            attackText.text = "";
            helthText.text = "";
        }
        else
        {
            //mechListがきちんとある場合。
            List<string> sslist = new List<string>();
            foreach (MechController.Mode i in Enum.GetValues(typeof(MechController.Mode)))
            {
                sslist.Add(i.ToString());
            }
            mechCommandDropDown.AddOptions(sslist);
            mechCommandDropDown.value = (int)mechManager.selectMechList[0].mechCon.mode;
            mechManager.selectMechList[0].selectBall.SetActive(true);
            attackText.text = "ATK " + mechManager.selectMechList[0].attack;
            helthText.text = "HP " + mechManager.selectMechList[0].Helth;
        }
    }
    public void MechChange()
    {
        int n = 0;
        foreach (var i in mechManager.selectMechList)
        {
            if (n == mechsDropDown.value)
            {
                attackText.text = "ATK " + mechManager.selectMechList[n].attack;
                helthText.text = "HP " + mechManager.selectMechList[n].Helth;
                mechCommandDropDown.value = (int)mechManager.selectMechList[n].mechCon.mode;
                i.selectBall.SetActive(true);
            }
            else
            {
                i.selectBall.SetActive(false);
            }
            n++;
        }

    }
    //IEnumerator UiUpdate()
    //{
    //    while (true)
    //    {
    //        mechsDropDown.options.Clear();
    //        foreach
    //        yield return null;
    //    }
    //}
}
