using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MechSoriteController : MonoBehaviour {
    [SerializeField]
    Button sortieButton;
    [SerializeField]
    Text ironValueText;
    [SerializeField]
    Text ironConsumeText;
    ItemManager itemManager;
    AITreeGenerator aITreeGenerator;
    MechGenerator mechGenerator;
    private void Start()
    {
        itemManager = CompornentUtility.FindCompornentOnScene<ItemManager>();
        aITreeGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
        mechGenerator = CompornentUtility.FindCompornentOnScene<MechGenerator>();
        sortieButton.interactable = false;
        sortieButton.onClick.AddListener(aITreeGenerator.GenerateMech);
        ironValueText.text = itemManager.itemDataTable[(int)ItemID.Iron].Value.ToString();
        ironConsumeText.text = mechGenerator.ConsumeIronValue + "を消費して出撃";
        itemManager.itemDataTable[(int)ItemID.Iron].AddValueChangedTrigger(ChangeSortieButton);
        itemManager.itemDataTable[(int)ItemID.Iron].Value= itemManager.itemDataTable[(int)ItemID.Iron].Value;
    }
    public void ChangeSortieButton(int _value)
    {
        ironValueText.text = _value.ToString();
        sortieButton.interactable = (_value >= mechGenerator.ConsumeIronValue);
    }
}
