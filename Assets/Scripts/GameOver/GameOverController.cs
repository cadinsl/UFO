using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public void TryAgain()
    {
        SceneController.Instance.LoadAdventureScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
