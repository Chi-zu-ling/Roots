using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    private Scene currentScene;

    public string menuName = "MainMenuScene";
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartCall(InputAction.CallbackContext context)
    {
        RestartScene();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menuName);
    }
}
