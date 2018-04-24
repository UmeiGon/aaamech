using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveDataButtonManager : MonoBehaviour
{
    SaveDataButton SelectButton;
    Dictionary<int, SaveDataButton> saveDataButtons = new Dictionary<int, SaveDataButton>();
    AITreeGenerator aiTreeGenerator;
    LoadAIDataCabinet saveDataCabinet;
    GameNumbers gameNumbers;
    [SerializeField]
    Button SaveButton;
    //セーブデータの上に付ける名前
    string articleName = "";
    public void ChangeArticleName(string article_name)
    {
        articleName = article_name;
    }
    private void Start()
    {
        aiTreeGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
        saveDataCabinet = CompornentUtility.FindCompornentOnScene<LoadAIDataCabinet>();
        gameNumbers = CompornentUtility.FindCompornentOnScene<GameNumbers>();
        for (int i = 0; i < saveDataButtons.Count; i++)
        {
            saveDataButtons[i].SaveDataName = PlayerPrefs.GetString(GetFileName(i));
            bool isActive = (i < gameNumbers.maxSaveValue);
            saveDataButtons[i].UIButton.interactable = isActive;
        }
        SaveButton.onClick.AddListener(SaveSelectAIData);
        //予め最初に全てローディングしておく
        LoadAllSaveData();

        ButtonPressed(saveDataButtons[0]);
    }
    string GetFileName(int _num)
    {
        return articleName + "Save" + _num;
    }

    public void LoadAllSaveData()
    {
        foreach (var i in saveDataButtons)
        {
            LoadAIData(i.Value);
        }
    }
    public void AddSaveDataButton(SaveDataButton save_data_button)
    {
        saveDataButtons.Add(save_data_button.ButtonNum, save_data_button);
    }
    public void ButtonPressed(SaveDataButton save_data_button)
    {
        if (SelectButton) SelectButton.UnSelectTrigger();
        save_data_button.SelectTrigger();
        SelectButton = save_data_button;
        foreach (var i in saveDataButtons)
        {
            bool isAcitve = (i.Value == save_data_button);
            MechAITree loadedTree = saveDataCabinet.LoadedTrees[i.Value.ButtonNum];
            loadedTree.IsActive = isAcitve;
            if (isAcitve)
            {
                aiTreeGenerator.editingTree = loadedTree;
            }
        }
    }
    void SaveSelectAIData()
    {
        SaveAIData(SelectButton);
    }
    void SaveAIData(SaveDataButton save_data_button)
    {
        if (!save_data_button) return;

        string fileName = GetFileName(save_data_button.ButtonNum);
        if (save_data_button.SaveDataName.Length == 0)
        {
            save_data_button.SaveDataName = "Data" + save_data_button.ButtonNum;
        }
        PlayerPrefs.SetString(fileName, save_data_button.SaveDataName);
        PlayerPrefs.Save();
        aiTreeGenerator.SaveMechAI(saveDataCabinet.LoadedTrees[save_data_button.ButtonNum], fileName);
    }
    void LoadAIData(SaveDataButton save_data_button)
    {
        if (!save_data_button) return;

        string fileName = GetFileName(save_data_button.ButtonNum);
        var ai = aiTreeGenerator.LoadMechAI(fileName);
        saveDataCabinet.AddLoadTree(save_data_button.ButtonNum, ai);
        if (ai != null)
        {
            ai.IsActive = false;
        }
    }
}
