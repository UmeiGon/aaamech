using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgramInput:MonoBehaviour{
    [SerializeField]
    protected InputDataCommand idc;
    [SerializeField]
    InputField nameInputField;
    protected void Awake()
    {
        if(nameInputField)nameInputField.onEndEdit.AddListener(NameInputChanged);
    }
    protected void OnEnable()
    {
        if (idc.SelectProgram != null&& nameInputField) nameInputField.text = idc.SelectProgram.commandName;

    }
    void NameInputChanged(string _text)
    {
        if (idc.SelectProgram != null) idc.SelectProgram.commandName = _text;
    }
}
