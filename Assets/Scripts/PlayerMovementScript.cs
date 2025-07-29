using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerMovementScript : MonoBehaviour
{
    public bool canMove = false;
    public float timeForMove = 0.2f;
    public float jumpHeight = 1.0f;

    public int minX = -4;
    public int maxX = 4;

    public GameObject[] leftSide;
    public GameObject[] rightSide;

    public float leftRotation = -45.0f;
    public float rightRotation = 90.0f;
    public int MoveGridSpeed = 2;

    private bool moving;
    private float elapsedTime;

    private Vector3 current;
    private Vector3 target;
    private float startY;

    private Rigidbody body;
    public GameObject mesh;

    private GameStateControllerScript gameStateController;
    private int score;

    [SerializeField]
    [Range(1, 2)]
    public float speedMultipier = 1.2f;
    [SerializeField]
    bool isMouseMovementEnable = false;

    public GameObject headlamp;
    public GameObject headlampLight;
    public Light sunLight;
    private float timeCurrentStat = 0;
    public Quaternion sunRotationNight;
    private Quaternion initSunRotationNight;
    public Color day;
    public Color night;
    private float timeCounter = 0f;
    public float switchInterval = 10f;
    private bool isNight = false;

    private int recordTime = 4;

    public float timeVulnerability = 3;
    private float currentTimeVulnerability = 0;
    public GameObject vulnerabilityFX;
    public bool isVulnerable()
    {
        return currentTimeVulnerability > 0;
    }
    public void setVulnerable()
    {
        currentTimeVulnerability = timeVulnerability;
    }
    public void Start()
    {
        current = transform.position;
        moving = false;
        startY = transform.position.y;

        body = GetComponentInChildren<Rigidbody>();
        sunLight = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();

        score = 0;
        gameStateController = GameObject.Find("GameStateController").GetComponent<GameStateControllerScript>();
    }
    public void Update()
    {
        timeCounter += Time.deltaTime;
        if (currentTimeVulnerability > 0)
        {
            vulnerabilityFX.SetActive(true);
            currentTimeVulnerability -= Time.deltaTime;
            if (transform.position.y < 1.2)
            {
                transform.position = new Vector3(transform.position.x, 1.335f, transform.position.z);
            }
        }
        else
        {
            vulnerabilityFX.SetActive(false);
        }
        // Switch every 10 seconds
        if (timeCounter >= switchInterval)
        {
            isNight = !isNight;
            timeCounter = 0f;
            Light[] allLights = GameObject.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (isNight)
            {
                foreach (Light light in allLights)
                {
                    if (light.type == LightType.Spot)
                    {
                        light.gameObject.SetActive(true);
                    }
                }

            }
            else
            {
                foreach (Light light in allLights)
                {
                    if (light.type == LightType.Spot)
                    {
                        light.gameObject.SetActive(false);
                    }
                }
            }
        }
        if (isNight)
        {
            gameStateController.currentTimeMode = TimeMode.night;
            sunLight.transform.rotation = Quaternion.Lerp(sunLight.transform.rotation, sunRotationNight, 0.5f * Time.deltaTime);
            sunLight.color = Color.Lerp(sunLight.color, night, 0.5f * Time.deltaTime);


        }
        else
        {
            gameStateController.currentTimeMode = TimeMode.day;
            sunLight.transform.rotation = Quaternion.Lerp(sunLight.transform.rotation, initSunRotationNight, 0.5f * Time.deltaTime);
            sunLight.color = Color.Lerp(sunLight.color, day, 0.5f * Time.deltaTime);
        }
        if (gameStateController.currentTimeMode == TimeMode.night)
        {
            headlamp.SetActive(true);
            headlampLight.SetActive(true);
        }
        else
        {

            headlamp.SetActive(false);
            headlampLight.SetActive(false);
        }
        // If player is moving, update the player position, else receive input from user.
        if (moving)
        {

            MovePlayer();
        }
        else
        {

            // Update current to match integer position (not fractional).
            current = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

            if (canMove)

                HandleInput();
        }

        score = Mathf.Max(score, (int)current.z);
        gameStateController.score = score;
    }

    private void HandleMouseClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            var direction = hit.point - transform.position;
            var x = direction.x;
            var z = direction.z;

            if (Mathf.Abs(z) > Mathf.Abs(x))
            {
                if (z > 0)
                    Move(new Vector3(0, 0, MoveGridSpeed));
                else
                    Move(new Vector3(0, 0, -MoveGridSpeed));
            }
            else
            { // (Mathf.Abs(z) < Mathf.Abs(x))
                if (x > 0)
                {
                    if (Mathf.RoundToInt(current.x) < maxX)
                        Move(new Vector3(MoveGridSpeed, 0, 0));
                }
                else
                { // (x < 0)
                    if (Mathf.RoundToInt(current.x) > minX)
                        Move(new Vector3(-MoveGridSpeed, 0, 0));
                }
            }
        }
    }

    private void HandleInput()
    {
        // Handle mouse click
        if (Input.GetMouseButtonDown(0) && isMouseMovementEnable)
        {
            HandleMouseClick();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(new Vector3(0, 0, MoveGridSpeed));
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(new Vector3(0, 0, -MoveGridSpeed));
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Mathf.RoundToInt(current.x) > minX)
                Move(new Vector3(-MoveGridSpeed, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Mathf.RoundToInt(current.x) < maxX)
                Move(new Vector3(MoveGridSpeed, 0, 0));
        }

    }

    private void Move(Vector3 distance)
    {

        var newPosition = current + distance;

        // Don't move if blocked by obstacle.
        if (Physics.CheckSphere(newPosition + new Vector3(0.0f, 0.5f, 0.0f), 0.1f))
            return;

        target = newPosition;

        moving = true;
        elapsedTime = 0;
        body.isKinematic = true; 

        switch (MoveDirection)
        {
            case "north":
                mesh.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "south":
                mesh.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "east":
                mesh.transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case "west":
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            default:
                break;
        }

        // Rotate arm and leg.
        foreach (var o in leftSide)
        {
            o.transform.Rotate(leftRotation, 0, 0);
        }

        foreach (var o in rightSide)
        {
            o.transform.Rotate(rightRotation, 0, 0);
        }
    }

    private void MovePlayer()
    {
        elapsedTime += Time.deltaTime;

        float weight = (elapsedTime < timeForMove) ? (elapsedTime / timeForMove) : 1;
        float x = Lerp(current.x, target.x, weight);
        float z = Lerp(current.z, target.z, weight);

        float y = Sinerp(current.y, startY + jumpHeight, weight);

        Vector3 result = new Vector3(x, y, z);
        transform.position = result; // note to self: why using transform produce better movement?
        // body.MovePosition(result);

        if (result == target)
        {
            moving = false;
            current = target;
            body.isKinematic = false;
            body.AddForce(0, -10, 0, ForceMode.VelocityChange);

            // Return arm and leg to original position.
            foreach (var o in leftSide)
            {
                o.transform.rotation = Quaternion.identity;
            }

            foreach (var o in rightSide)
            {
                o.transform.rotation = Quaternion.identity;
            }
        }
    }

    private float Lerp(float min, float max, float weight)
    {
        return min + (max - min) * weight;
    }

    private float Sinerp(float min, float max, float weight)
    {
        return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
    }

    public bool IsMoving
    {
        get { return moving; }
    }

    public string MoveDirection
    {
        get
        {
            if (moving)
            {
                float dx = target.x - current.x;
                float dz = target.z - current.z;
                if (dz > 0)
                    return "north";
                else if (dz < 0)
                    return "south";
                else if (dx > 0)
                    return "west";
                else
                    return "east";
            }
            else
                return null;
        }
    }

    public void GameOver()
    {
        // When game over, disable moving. 

        // Call GameOver at game state controller (instead of sending messages).
        gameStateController.GameOver();
    }

    public void Reset()
    {
        // TODO This kind of reset is dirty, refactor might be needed.
        transform.position = new Vector3(0, 1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.identity;
        score = 0;
    }
}