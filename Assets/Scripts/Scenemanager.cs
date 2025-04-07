using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene(0);
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }
    
    public void OnHTPClick()
    {
        SceneManager.LoadScene(2);
    }
    
    public void OnContinueClick()
    {
        SceneManager.LoadScene(3);
    }
    public void OnContinueClick2()
    {
        SceneManager.LoadScene(4);
    }
}
