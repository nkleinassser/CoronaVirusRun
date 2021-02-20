using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenController : MonoBehaviour
{
    public static int currentScore = 0;
    public static int livesLeft = 3;
    
    void Awake() {

    }

    void Update() {
        #if !UNITY_EDITOR
            if (Screen.width < 900 || Screen.height < 350) {
                Screen.SetResolution(1000, 450, false);
            }
        #endif
    }
}
