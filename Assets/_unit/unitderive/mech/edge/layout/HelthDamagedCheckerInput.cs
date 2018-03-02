using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HelthDamagedCheckerInput : CheckerInput {
    [SerializeField]
    InputField DamageParInput;
    [SerializeField]
    Toggle upperToggle;
    private void Awake()
    {
        DamageParInput.onValueChanged.AddListener(ItemValueInputChanged);
        upperToggle.onValueChanged.AddListener(ToggleChanged);
    }
    private void OnEnable()
    {
        if (ide.SelectChecker is HelthDamagedChecker)
        {
            var checkerInstance = ide.SelectChecker as HelthDamagedChecker;
            DamageParInput.text = checkerInstance.par.ToString();
            upperToggle.isOn = (checkerInstance.upper != 0);
        }
    }
    void ToggleChanged(bool _flag)
    {
        if (ide.SelectChecker is HelthDamagedChecker)
        {
            var checkerInstance = ide.SelectChecker as HelthDamagedChecker;
            if (_flag)
            {
                checkerInstance.upper = 1;
            }
            else
            {
                checkerInstance.upper = 0;
            }
        }

    }
    void ItemValueInputChanged(string _text)
    {
        if (ide.SelectChecker is HelthDamagedChecker)
        {
            var checkerInstance = ide.SelectChecker as HelthDamagedChecker;
            checkerInstance.Par = int.Parse(_text);
            DamageParInput.text = checkerInstance.Par.ToString();
        }
    }
}
