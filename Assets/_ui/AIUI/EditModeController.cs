using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeController : MonoBehaviour {
    CanvasChanger canvasChanger;
    AITreeGenerator aITreeGenerator;
    [SerializeField]
    ChangeCanvasButton editButton;
    private void Awake()
    {
        canvasChanger = CompornentUtility.FindCompornentOnScene<CanvasChanger>();
        aITreeGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
        canvasChanger.AddChangedCanvasTrigger(ClickedEditButton);
    }
    void ClickedEditButton(ChangeCanvasButton _button)
    {
        if (!editButton) return;
        bool isEditMode = (editButton == _button);
        aITreeGenerator.SelectEdge = null;
        aITreeGenerator.SelectNode = null;
        aITreeGenerator.CanEdit = isEditMode;
    }
}
