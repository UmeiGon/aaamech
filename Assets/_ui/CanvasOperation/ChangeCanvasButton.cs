using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ChangeCanvasButton : MonoBehaviour {
    [SerializeField]
    Button button;
    [SerializeField]
    GameObject selectEffect;
    public GameObject SelectEffect
    {
        get { return selectEffect; }
    }
    [SerializeField]
    int canvasNum;
    public int CanvasNum{
        get { return canvasNum; }
    }
    CanvasChanger canvasChanger;
    private void Awake()
    {
        canvasChanger = CompornentUtility.FindCompornentOnScene<CanvasChanger>();
        canvasChanger.AddChangeCanvasButton(this);
        button.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        canvasChanger.ChangeCanvas(this);
    }
}
