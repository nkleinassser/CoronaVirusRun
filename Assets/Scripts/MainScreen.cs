using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    public static bool loadingGame = false;
    public static void PlayButtonClicked() {
        if (loadingGame == false) {
            loadingGame = true;

            SceneManager.LoadSceneAsync("GameScreen");

            loadingGame = false;
        }
    }

    void Start() {
        GameObject.FindGameObjectWithTag("ScoreLabel").GetComponent<Text>().text = GlobalData.highScore.ToString();
    }
}
