using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameExit : MonoBehaviour {
    [SerializeField]
    Button exitButton;
    private void Start()
    {
        exitButton.onClick.AddListener(QuitGame);
    }
    void QuitGame()
    {
        Application.Quit();
    }
}
