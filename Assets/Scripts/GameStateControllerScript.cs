using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.EventSystems;


public class GameStateControllerScript : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject playCanvas;
    public GameObject gameOverCanvas;

    public TextMeshProUGUI playScore;
    public TextMeshProUGUI gameOverScore;
    public GameObject topScore;
    public GameObject ScoreListItemView;
    public TextMeshProUGUI playerName;

    public int score, top;

    private GameObject currentCanvas;
    private string state;

    public string filename = "top.txt";
    public TimeMode currentTimeMode;
    private string filePath;
    public int PowerUpLifeCount = 0;
    public int PowerUpLifeCountLimit = 3;
    [Header("Revive Settings")]
    public GameObject RevivePanel;
    public TextMeshProUGUI LifeText;
    private PlayerMovementScript playerMovementScript;
    public ScoresList data;
    public Healthbar healthbar;
    public TMP_InputField usernameInputField;
    



    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "score.json");
    }
    public void Start()
    {
        currentCanvas = null;
        if (playerMovementScript == null)
        {
            playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
        }

        healthbar.UpdateHearts(PowerUpLifeCount);// Initialize health UI

        MainMenu();
    }

    public void Update()
    {
        if (PowerUpLifeCount > PowerUpLifeCountLimit)
        {
            PowerUpLifeCount = PowerUpLifeCountLimit;
        }
        if (playerMovementScript == null)
        {
            playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
        }
        if (state == "play")
        {
            playScore.text = score.ToString();
            playerName.text = LoadData().playerName;
        }
        else if (state == "mainmenu")
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Application.Quit();
            }
            else if (Input.anyKeyDown)
            {
                Play();
            }
        }
        else if (state == "gameover")
        {
            Time.timeScale = 0;
            if (Input.anyKeyDown)
            {
                // Application.LoadLevel("Menu");
            }
        }
        else if (state == "revive")
        {
            Time.timeScale = 0;
        }

    }

        public void SubmitUsername()
    {
        if (usernameInputField == null) return;// avoid null crash 
        string username = usernameInputField.text;

        if (!string.IsNullOrEmpty(username))
        {
            PlayerPrefs.SetString("username", username);
            //Save for leaderboard system
            PlayerData data = new PlayerData { playerName = username };
            string json = JsonUtility.ToJson(data);
            string filePathData = Path.Combine(Application.persistentDataPath, "playerData.json");
            File.WriteAllText(filePathData, json);

            Debug.Log("Username saved: " + username);
        }
        else
        {
            Debug.LogWarning("Username is empty, not saved.");
        }
    }
    public void Revive()
    {
        if (PowerUpLifeCount > 0)
        {
            PowerUpLifeCount -= 1;
            playerMovementScript.setVulnerable();
            state = "play";
            RevivePanel.SetActive(false);
            playerMovementScript.canMove = true;
            Time.timeScale = 1;
            // playerMovementScript.transform.localScale = new Vector3(1, 1, 1);

            healthbar.UpdateHearts(PowerUpLifeCount);

        }
    }

    public void AddLifePoint(int val)
    {
        PowerUpLifeCount += val;
        //reviveText.text = PowerUpLifeCount + "";
        healthbar.UpdateHearts(PowerUpLifeCount);//Updates the puso hearts display

    }
    public void MainMenu()
    {
        CurrentCanvas = mainMenuCanvas;
        state = "mainmenu";

        GameObject.Find("LevelController").SendMessage("Reset");
        GameObject.FindGameObjectWithTag("Player").SendMessage("Reset");
        GameObject.FindGameObjectWithTag("MainCamera").SendMessage("Reset");


    }

    public void Play()
    {
        CurrentCanvas = playCanvas;
        state = "play";
        score = 0;
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>().canMove = true;
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>().moving = true;
    }

    public void GameOver()
    {
        if (playerMovementScript == null || playerMovementScript.isVulnerable())//(playerMovementScript.isVulnerable())  
        {
            return;
        }
        else if (PowerUpLifeCount > 0)
        {
            RevivePanel.SetActive(true);
            state = "revive";
        }
        else
        {

            state = "gameover";
            Debug.LogWarning("gameover");
            CurrentCanvas = gameOverCanvas;

            gameOverScore.text = score + "";

            topScore.SetActive(true); //always show leaderboard

            // Always show the username Input Field
            usernameInputField.gameObject.SetActive(true);
            usernameInputField.text = ""; // Clear previous input

            // Remove old listeners to avoid duplicates
            usernameInputField.onEndEdit.RemoveAllListeners();

            // Add new listener
            usernameInputField.onEndEdit.AddListener((value) =>
            {
                SubmitUsername();   // Save immediately
                SaveScore();        // Then save to leaderboard
                
            });

            if (score > top)
            {
                PlayerPrefs.SetInt("Top", score);
            }
            // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>().moving = false;
        }
    }
    public PlayerData LoadData()
    {
        string filePathData = Path.Combine(Application.persistentDataPath, "playerData.json");
        if (File.Exists(filePathData))
        {


            string json = File.ReadAllText(filePathData);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            return null;
        }
    }
    public ScoresList LoadScore()
    {
        if (!File.Exists(filePath))
        {
            return new ScoresList
            {
                scores = new List<Score>()
            };
        }

        string json = File.ReadAllText(filePath);
        ScoresList data = JsonUtility.FromJson<ScoresList>(json);

        

        // Deduplicate by keeping highest score per name
        var highestScores = data.scores.GroupBy(s => s.name)
            .Select(g => g.OrderByDescending(s => s.score).First())
            .ToList();

        // Optional: Log the cleaned-up scores 
        // Return cleaned list
        return new ScoresList { scores = highestScores };
    }

    public void SaveScore()
    {
        try
        {
            Debug.LogWarning("Saving to " + filePath);
            data = LoadScore();
            data.scores.Add(new Score { name = LoadData()?.playerName ?? "Player", score = score });

            data.scores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort descending by score

            string json = JsonUtility.ToJson(data, true); // pretty print
            Debug.LogWarning(json);
            File.WriteAllText(filePath, json);
            Debug.LogWarning("Data saved to " + filePath);
            int x = 0;

            Debug.LogWarning(data);
            if (data.scores.Count > 0)
            {

                foreach (var scoreItem in data.scores)
                {
                    GameObject scoreOBJ = Instantiate(ScoreListItemView, topScore.transform);
                    ScoreListView scoreListView = scoreOBJ.GetComponent<ScoreListView>();
                    scoreListView.score.text = scoreItem.score + "";
                    scoreListView.PlayerName.text = scoreItem.name;
                    scoreListView.rankingTxt.text = (x + 1) + "";

                    Debug.LogWarning($"score:{scoreItem.score}, name:{scoreItem.name}, rank:{x + 1}");
                    x++;
                }
            }
            else
            {

                Debug.LogWarning("Data score count:" + data.scores.Count);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save file: " + ex.Message);
        }
    }
    private GameObject CurrentCanvas
    {
        get
        {
            return currentCanvas;
        }
        set
        {
            if (currentCanvas != null)
            {
                currentCanvas.SetActive(false);
            }
            currentCanvas = value;
            currentCanvas.SetActive(true);
        }
    }
}
[System.Serializable]
public class ScoresList
{
    public List<Score> scores;
}
[System.Serializable]
public class Score
{
    public string name;
    public int score;
}
public enum TimeMode
{
    day,
    night
}


