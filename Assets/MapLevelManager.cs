using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode] 
[System.Serializable]
public class MapLevelManager : MonoBehaviour
{
    public int CurrentLevel=1;
    [SerializeField]
    public List<SceneLevel> sceneLevels;
    public List<SceneLevelManager> sceneLevelManagers;
    public GameObject sceneLevelManagerPrefabs;

    public GameObject Map;
    public MissionViewer missionViewer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // Update is called once per frame
#if UNITY_EDITOR
    void Update()
    {
        if (sceneLevelManagers.Count > 0 && sceneLevelManagers.Count != sceneLevels.Count)
        {
            foreach (var SceneLevelManager in sceneLevelManagers)
            {
                sceneLevels.Clear();
                sceneLevels.Add(SceneLevelManager.sceneLevel);
            }
        }
        int x = 0;
        foreach (var SceneLevelManager in sceneLevelManagers)
        {
            if (sceneLevels[x] != SceneLevelManager.sceneLevel)
            {
                sceneLevels[x] = SceneLevelManager.sceneLevel;
            }
            if (SceneLevelManager.missionViewer != missionViewer)
            {
                SceneLevelManager.missionViewer = missionViewer;
            }
            x++;
        }
    }
#endif
}
