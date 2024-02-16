using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneController : MonoBehaviour
{
    private int currentScene;
    public static SceneController Instance { get; private set;}


    private void Awake() 
    { 
    // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(Instance);
        } 

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    private void loadScene(int sceneNumber, UnityAction actionAfterLoaded)
    {
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Additive);
        StartCoroutine(waitForSceneLoad(1, actionAfterLoaded));
    }

    public void LoadAdventureScene()
    {
        SceneManager.LoadScene("Adventure");
    }

     
IEnumerator waitForSceneLoad(int sceneNumber, UnityAction actionAfterLoaded)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            yield return null;
            
        }
 
        // Do anything after proper scene has been loaded
         if (SceneManager.GetActiveScene().buildIndex == sceneNumber)
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
        }
        actionAfterLoaded();
        currentScene = sceneNumber;
    }
}
