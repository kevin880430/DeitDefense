using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool ControlSound;
   
    public void NextScene() {
        SceneManager.LoadScene("Game");
    }
    public void Quit() {
        Application.Quit();
    }
    public void Control_Sound() {
        ControlSound = !ControlSound;
        AudioListener.pause = ControlSound;
    }
}
