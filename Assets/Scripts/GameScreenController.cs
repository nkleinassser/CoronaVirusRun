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
            } else if (collider.name == "Enemy" || collider.tag == "Particle") {
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
    public static float playerSpeed = 500f;
    public static double lastRuntime = 0.0;
    public static int maxParticles = 10;
    public static int currentParticles = 0;
    public static double lastParticleCreatedTime = 0.0;

    public static List<GameObject> particlesInExistence = new List<GameObject>();

    public void Start() {
        particlesInExistence = new List<GameObject>();
        currentParticles = 0;
    }

    public static void particleHitWall(GameObject particle, Collider2D collider) {
        destroyParticle(particle, true);
    }

    public static void destroyParticle(GameObject particle, bool createNew) {
        particlesInExistence[particlesInExistence.IndexOf(particle)] = null;
        Destroy(particle);

        if (createNew == true) {
            createNewParticle();
        }
    }

    public static void createNewParticle() {
        GameObject newParticle = GameObject.Instantiate(GameObject.FindGameObjectWithTag("DefaultParticle"), GameObject.FindGameObjectWithTag("ParticleHolder").transform);
        newParticle.tag = "Particle";
        currentParticles ++;
        particlesInExistence.Add(newParticle);

        int[] randomNumbers = new int[2]; 

        for (int i = 0; i < 2; i++) {
            int signController = Random.Range(0, 1) * 2 - 1;

            randomNumbers[i] = Random.Range(150, 250) * signController;
        }

        newParticle.GetComponent<Rigidbody2D>().AddForce(Random.onUnitSphere * new Vector2(randomNumbers[0], randomNumbers[1]), ForceMode2D.Impulse);
    }

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

        if (currentParticles < maxParticles && Time.time - lastParticleCreatedTime >= 0.35)  {
            lastParticleCreatedTime = Time.time;
            createNewParticle();
        }
    }
}
