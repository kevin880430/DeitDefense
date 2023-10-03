using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool ControlSound;
   
    public void NextScene() {
        //画面遷移機能
        SceneManager.LoadScene("Game");
    }
    public void Quit() {
        //離脱機能
        Application.Quit();
    }
    public void Control_Sound() {
        //ミュート機能
        ControlSound = !ControlSound;
        AudioListener.pause = ControlSound;
    }
}
