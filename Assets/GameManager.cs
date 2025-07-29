using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public float timeIntevalSave;
    [SerializeField] private GameObject settingsMenu;
    public PlayerData playerData;

    private string filePath;
    private float curTime = 0;
    public TMP_InputField playerNameTXT;
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        if (!playerData.mapLevelManager)
        {
            playerData.mapLevelManager = gameObject.GetComponent<MapLevelManager>();
        }
    }

    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > timeIntevalSave)
        {
            curTime = 0;
            if (!playerData.mapLevelManager)
            {
                playerData.mapLevelManager = gameObject.GetComponent<MapLevelManager>();
            }
            SaveData();
        }
        playerData.mapLevelManager.CurrentLevel = playerData.level;
        foreach (var mapLevel in playerData.mapLevelManager.sceneLevelManagers)
        {
            mapLevel.CurrentLevel = playerData.level;
        }
    }
    public void SaveName()
    {
        playerData.playerName = playerNameTXT.text;
    }
    public void SaveData()
    {
        PlayerData data = new PlayerData
        {
            mapLevelManager = playerData.mapLevelManager,
            playerName = playerData.playerName,
            level = playerData.level,
            health = 75.5f
        };

        string json = JsonUtility.ToJson(data, true); // pretty print
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {


            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            playerData = data;
            if (!playerData.mapLevelManager)
            {
                playerData.mapLevelManager = gameObject.GetComponent<MapLevelManager>();
            }
            Debug.Log($"Loaded: Name={data.playerName}, Level={data.level}, Health={data.health}");
            playerData.mapLevelManager.CurrentLevel = data.level;
            playerNameTXT.text = data.playerName;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
        }
    }
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }


}
