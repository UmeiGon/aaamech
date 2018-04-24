using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
public class SaveDataButton : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    Button button;
    [SerializeField]
    InputField nameInputField;
    [SerializeField]
    int buttonNum;
    public Button UIButton
    {
        get { return button;}
    }
    public string SaveDataName
    {
        get { return nameInputField.text; }
        set { nameInputField.text = value; }
    }
    public int ButtonNum
    {
        get { return buttonNum; }
    }
    SaveDataButtonManager manager;
    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        manager = CompornentUtility.FindCompornentOnScene<SaveDataButtonManager>();
        manager.AddSaveDataButton(this);
        nameInputField.enabled = false;
    }
    void OnClick()
    {
        manager.ButtonPressed(this);
    }
    public void SelectTrigger()
    {
        anim.SetBool("IsSelect", true);
        nameInputField.enabled = true;
        nameInputField.ActivateInputField();
    }
    public void UnSelectTrigger()
    {
        anim.SetBool("IsSelect", false);
        nameInputField.enabled = false;
    }
}
