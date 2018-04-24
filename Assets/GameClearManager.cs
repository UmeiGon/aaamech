using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{

    public Unit ClearUnit;
    private void Start()
    {
        if (ClearUnit)
        {
            StartCoroutine(ClearCheckRoutine());
        }
    }
    IEnumerator ClearCheckRoutine()
    {
        while (true)
        {
            if (ClearUnit == null)
            {
                Clear();
            }
            yield return null;
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
