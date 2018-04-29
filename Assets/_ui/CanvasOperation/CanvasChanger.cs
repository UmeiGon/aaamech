using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CanvasChanger : MonoBehaviour
{
    [SerializeField]
    int DefaultCanvasNum;
    [SerializeField]
    List<Canvas> canvasList;
    Action<ChangeCanvasButton> changeTrigger;
    List<ChangeCanvasButton> changeButtonList = new List<ChangeCanvasButton>();
    public void AddChangedCanvasTrigger(Action<ChangeCanvasButton> change_trigger)
    {
        changeTrigger += change_trigger;
    }
    private void Start()
    {
        if (changeButtonList.Count > 0)
        {
            if (changeButtonList.Count > DefaultCanvasNum)
            {
                ChangeCanvas(changeButtonList[DefaultCanvasNum]);
            }
            else
            {
                ChangeCanvas(changeButtonList[0]);
            }
        }
    }
    public void AddChangeCanvasButton(ChangeCanvasButton change_button)
    {
        changeButtonList.Add(change_button);
    }
    public void ChangeCanvas(ChangeCanvasButton change_button)
    {
        ChangeCanvas(change_button.CanvasNum);
        ActiveChange(change_button);
    }
    void ActiveChange(ChangeCanvasButton change_button)
    {
        foreach (var i in changeButtonList)
        {
            bool isActive = (change_button == i);
            i.SelectEffect.SetActive(isActive);
        }
        changeTrigger(change_button);
    }
    void ChangeCanvas(int canvas_num)
    {
        for (int i = 0; i < canvasList.Count; i++)
        {
            bool isActive = (i == canvas_num);
            canvasList[i].enabled = isActive;
        }
    }
}
