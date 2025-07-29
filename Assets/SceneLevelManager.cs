using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
[System.Serializable]
public class SceneLevelManager : MonoBehaviour
{
    public int CurrentLevel = 1;
    public MissionViewer missionViewer;
    public SceneLevel sceneLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<RectTransform>().position = sceneLevel.position;
        sceneLevel.levelTxT.text = sceneLevel.level + "";
    }
    public void StartLevel()
    {
        string sceneName = sceneLevel.scene.name;
        Debug.LogError(sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void ViewMission()
    {
        if (CurrentLevel >= sceneLevel.level)
            missionViewer.gameObject.SetActive(true);
        missionViewer.selectedLevel = this;
        missionViewer.ViewMission();
    }
}
[System.Serializable]
public class SceneLevel
{
    [SerializeField]
    public int level;
    [SerializeField]
    public Vector3 position;
    public TextMeshProUGUI levelTxT;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public SceneAsset scene;
    public Item[] items;
    public string[] mission;
    public float highScore_Time;
    public bool isLocked;
}