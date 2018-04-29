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
    [SerializeField]
    ChangeCanvasButton changeEditButton;
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
        bool isTutorial=TutorialSetter.IsTutorial;
        gameNumbers = CompornentUtility.FindCompornentOnScene<GameNumbers>();
       
        //予め最初に全てローディングしておく
        if (isTutorial)
        {
            Debug.Log("tuto");
            for (int i = 0; i < saveDataButtons.Count; i++)
            {
                saveDataButtons[i].SaveDataName = "Data"+i;
                bool isActive = (i < 1);
                saveDataButtons[i].UIButton.interactable = isActive;
            }
            NewAllSaveData();
        }
        else
        {
            //セーブデータの名前をセット
            for (int i = 0; i < saveDataButtons.Count; i++)
            {
                string name = PlayerPrefs.GetString(GetFileName(i));
                if (name == "")
                {
                    name = "Data" + i;
                }
                saveDataButtons[i].SaveDataName = name;
                bool isActive = (i < saveDataButtons.Count);
                saveDataButtons[i].UIButton.interactable = isActive;
            }
            LoadAllSaveData();
        }
        changeEditButton.UIButton.interactable = false;
    }
    string GetFileName(int _num)
    {
        return articleName + "Save" + _num;
    }
    //セーブデータを全て新しいaitreeに置き換える
    void NewAllSaveData()
    {
        for (int i=0;i< saveDataButtons.Count;i++)
        {
            saveDataCabinet.AddLoadTree(i, new MechAITree());
        }
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
    //出撃したらsaveDataCabinet.LoadedTrees[i.Value.ButtonNum]がeditingtreeと違うものになるので直す
    public void ButtonPressed(SaveDataButton save_data_button)
    {
        changeEditButton.UIButton.interactable = true;
        if (SelectButton) SelectButton.UnSelectTrigger();
        save_data_button.SelectTrigger();
        SelectButton = save_data_button;
        //AITreeのアクティブをチェンジ
        foreach (var i in saveDataButtons)
        {
            bool isAcitve = (i.Value == save_data_button);
            var loadedTree = saveDataCabinet.LoadedTrees[i.Value.ButtonNum];
            loadedTree.UIIsActive = isAcitve;
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
        MechAITree ai = new MechAITree();
        saveDataCabinet.AddLoadTree(save_data_button.ButtonNum, ai);
        if (ai != null)
        {
            ai.UIIsActive = false;
        }
    }
}
