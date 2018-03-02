using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour {
   
   public Unit ClearUnit;
    private void Update()
    {
        if (ClearUnit == null)
        {
            Clear();
        }
    }
    void Clear()
    {
        SceneManager.LoadScene("ClearScene");
    }
    public void ResetButtonClick()
    {
        SceneManager.LoadScene("stage_1");
    }
}
