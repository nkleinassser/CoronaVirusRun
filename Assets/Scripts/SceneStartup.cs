using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartup : MonoBehaviour
{
    public void Awake() {
        if (GlobalData.mainSceneLoaded == false) {
            if (SceneManager.GetActiveScene().name != "MainScreen") {
                SceneManager.LoadSceneAsync("MainScreen");
            }

            GlobalData.mainSceneLoaded = true;
        }
    }

    void Update() {
        #if !UNITY_EDITOR
            if (Screen.width < 600 || Screen.height < 300) {
                Screen.SetResolution(672, 378, false);
            }
        #endif
    }
}
