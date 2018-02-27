using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CheckerInput : MonoBehaviour {
    [SerializeField]
    protected InputDataEdge ide;
    [SerializeField]
     InputField nameInputField;
    protected void Awake()
    {
        if(nameInputField)nameInputField.onEndEdit.AddListener(NameInputChanged);
    }
    protected void OnEnable()
    {
        if (ide.SelectChecker!=null&& nameInputField) nameInputField.text = ide.SelectChecker.edgeName;
       
    }
    void NameInputChanged(string _text)
    {
        if(ide.SelectChecker != null) ide.SelectChecker.edgeName=_text;
    }
}
