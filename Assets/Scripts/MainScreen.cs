using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    public static void PlayButtonClickedOld() {
        Text[] textLabels = FindObjectsOfType<Text>();

        foreach (Text textLabel in textLabels) {
            if (textLabel.name == "MainScreenText") {
                textLabel.text = "Hello World";
            }
        }
    }

    public static bool loadingGame = false;
    public static void PlayButtonClicked() {
        if (loadingGame == false) {
            loadingGame = true;

            SceneManager.LoadSceneAsync("GameScreen");

            loadingGame = false;
        }
    }

    void Update() {
        #if !UNITY_EDITOR
            if (Screen.width < 900 || Screen.height < 350) {
                Screen.SetResolution(1000, 450, false);
            }
        #endif
    }
}
