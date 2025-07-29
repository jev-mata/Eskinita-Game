using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelControllerScript : MonoBehaviour
{
    public int minZ = 3;
    public int lineAhead = 40;
    public int lineBehind = 20;
    public int Distance_spawn = 100;
    public int MaxSpawn = 3;
    private int CurrentPicked = 0;

    public GameObject[] linePrefabs;
    public GameObject coins;

    private Dictionary<int, GameObject> lines;
    private GameObject player;

    public GameObject gameOverPanel;
    public int lastLineIndex = -1;
    private int lineRepeatCount = 0;
    private int maxRepeat = 2;

    private RoadCarGeneratorManager roadCarGenerator;
    private MainMenuManager mainMenuManager;
    private int lastPlayerZ = 0;
    private float accumulatedZDistance = 0f;
    public void Picked()
    {
        CurrentPicked++;
    }
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lines = new Dictionary<int, GameObject>();
        mainMenuManager = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<MainMenuManager>();
        lastPlayerZ = (int)player.transform.position.z;
    }

    public void Update()
    {
        var playerZ = (int)player.transform.position.z;
        int deltaZ = playerZ - lastPlayerZ;
        accumulatedZDistance += deltaZ;
        lastPlayerZ = playerZ;
        for (int z = Mathf.Max(minZ, playerZ - lineBehind); z <= playerZ + lineAhead; z++)
        {
            if (!lines.ContainsKey(z))
            {

                // Select line prefab with streak limit
                int nextLineIndex = GetNextLineIndex();
                lastLineIndex = nextLineIndex;
                GameObject trainOBJ = linePrefabs[nextLineIndex];
                int gridSize = 1;
                if (trainOBJ.TryGetComponent<RoadCarGeneratorManager>(out roadCarGenerator))
                {
                    gridSize = roadCarGenerator.roadCarGeneratorLeftLane.gridSize + roadCarGenerator.roadCarGeneratorRightLane.gridSize;
                }
                float offsetZ = (gridSize > 1) ? (gridSize / 2f - 0.55f) : 0f;
                GameObject line = Instantiate(trainOBJ, new Vector3(0, 0, z + offsetZ), Quaternion.identity);

                if (accumulatedZDistance >= Distance_spawn - lineAhead && MaxSpawn > CurrentPicked)
                {
                    accumulatedZDistance = 0f;
                    var coin = Instantiate(coins);
                    int randX = Random.Range(-6, 6);
                    coin.transform.position = new Vector3(randX, 1, lastPlayerZ + 5);
                }

                line.transform.localScale = new Vector3(1, 1, 1);

                // Mark all occupied Z positions in the dictionary
                for (int i = 0; i < gridSize; i++)
                {
                    lines.Add(z + i, line);
                }
            }
        }
        int c = 0;
        // Cleanup old lines
        foreach (var line in new List<GameObject>(lines.Values))
        {
            if (line.gameObject)
            {
                float lineZ = line.transform.position.z;
                if (lineZ < playerZ - lineBehind)
                {
                    lines.Remove((int)lineZ);
                    Destroy(line);
                }
            }
            else
            {

                lines.Remove(c);
            }
            c++;
        }
    }

    // Helper to pick line index while enforcing max repeat limit
    private int GetNextLineIndex()
    {
        int index;

        if (lineRepeatCount >= maxRepeat)
        {
            // Must pick a different line
            do
            {
                index = Random.Range(0, linePrefabs.Length);
            } while (index == lastLineIndex);
            lineRepeatCount = 1;
        }
        else
        {
            index = Random.Range(0, linePrefabs.Length);
            if (index == lastLineIndex)
                lineRepeatCount++;
            else
                lineRepeatCount = 1;
        }

        lastLineIndex = index;
        return index;
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void Reset()
    {
        if (lines != null)
        {
            foreach (var line in new List<GameObject>(lines.Values))
            {
                Destroy(line);
            }
            lines.Clear();
            lastLineIndex = -1;
            lineRepeatCount = 0;
            Start();
        }
    }
    public void PlayAgain()
    {
        gameOverPanel.SetActive(false);
        ReloadScene();
    }
    public void MainMenu()
    {
        if (mainMenuManager)
        {
            gameOverPanel.SetActive(false);
            mainMenuManager.MainMenu();
        }
    }
}
