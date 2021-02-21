using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreenController : MonoBehaviour
{
    GameObject gameHolder = null;
    GameObject playerCharacter = null;

    Vector3 startPosition = new Vector3(0, 0, 0);
    public static Vector3 transformPosition = new Vector3(0, 0, 0);

    public void getGameHolder() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("MainGameFrame")) {
            if (gameObject.name == "GameHolder") {
                gameHolder = gameObject;
            }
        }
    }

    void Awake() {
        FindObjectOfType<Text>().text = "Score: " + GlobalData.currentScore.ToString();
    }

    public static bool runningCollisionData = false;
    public static bool movingPlayer = false;

    void OnTriggerEnter2D(Collider2D collider) {
        if (runningCollisionData == false) { 
            runningCollisionData = true;

            if (collider.name == "EndZone") {
                movingPlayer = true;
                GlobalData.currentScore += 1;
                FindObjectOfType<Text>().text = "Score: " + GlobalData.currentScore.ToString();
                playerCharacter = null;
                SceneManager.LoadSceneAsync("GameScreen");
            } else if (collider.name == "Enemy") {
                movingPlayer = true;
                if (GlobalData.currentScore >= GlobalData.highScore) {
                    GlobalData.highScore = GlobalData.currentScore;
                }

                GlobalData.currentScore = 0;
                playerCharacter = null;
                SceneManager.LoadSceneAsync("MainScreen");
            }

            runningCollisionData = false;
            movingPlayer = false;
        }
    }

    public static bool alreadyRunning = false;
    public static float playerSpeed = 250f;
    public static double lastRuntime = 0.0;

    void FixedUpdate() {
        bool ranFirst = false;
        //bool cooldownCheckPassed = false;
        bool cooldownCheckPassed = true;

        //if (Time.time >= lastRuntime) {
            //lastRuntime = Time.time;
            //cooldownCheckPassed = true;
        //}

        if (alreadyRunning == false && cooldownCheckPassed == true) {
            alreadyRunning = true;
            ranFirst = true;

            if (gameHolder != null) {
                //double mapHeight = gameHolder.GetComponent<RectTransform>().sizeDelta.y;
                //double mapWidth = gameHolder.GetComponent<RectTransform>().sizeDelta.x;

                //double aspectRatio = mapWidth / mapHeight;

                //FindObjectOfType<Text>().text = aspectRatio.ToString();
            } else {
                getGameHolder();
            }
        }

        if (playerCharacter && cooldownCheckPassed == true && (alreadyRunning == false || ranFirst == true)) {
            bool detectedKey = false;
            alreadyRunning = true;

            //playerCharacter.GetComponent<BoxCollider2D>().size.Set(playerCharacter.GetComponent<RectTransform>().sizeDelta.x, playerCharacter.GetComponent<RectTransform>().sizeDelta.y);

            //Debug.Log(playerCharacter.GetComponent<RectTransform>().sizeDelta.x);
            //Debug.Log(playerCharacter.GetComponent<RectTransform>().sizeDelta.y);

            //GameObject.FindObjectOfType<Text>().text = GameObject.FindGameObjectWithTag("Player") + " , " + playerCharacter.GetComponent<RectTransform>().sizeDelta.y;

            //var transformPosition = playerCharacter.transform.position;

            transformPosition.x = Mathf.Abs(transformPosition.x);
            transformPosition.y = Mathf.Abs(transformPosition.y);

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                detectedKey = true;
                playerCharacter.GetComponent<Rigidbody2D>().simulated = true;
                transformPosition += (new Vector3(0, 1, 0) * Time.fixedDeltaTime * playerSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                detectedKey = true;
                playerCharacter.GetComponent<Rigidbody2D>().simulated = true;
                transformPosition += (new Vector3(-1, 0, 0) * Time.fixedDeltaTime * playerSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                detectedKey = true;
                playerCharacter.GetComponent<Rigidbody2D>().simulated = true;
                transformPosition += (new Vector3(0, -1, 0) * Time.fixedDeltaTime * playerSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                detectedKey = true;
                playerCharacter.GetComponent<Rigidbody2D>().simulated = true;
                transformPosition += (new Vector3(1, 0, 0) * Time.fixedDeltaTime * playerSpeed);
            }
            if (detectedKey == false) {
                playerCharacter.GetComponent<Rigidbody2D>().simulated = false;
            } else if (movingPlayer == false) {
                playerCharacter.GetComponent<Rigidbody2D>().MovePosition(transformPosition);
            }
        } else {
            playerCharacter = GameObject.FindGameObjectWithTag("Player");
            transformPosition = playerCharacter.transform.position;
        }

        alreadyRunning = false;
    }
}
