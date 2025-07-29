using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapLevelManager))]
public class MapLevelManagerEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapLevelManager script = (MapLevelManager)target;

        if (GUILayout.Button("Add New Level"))
        {
            GameObject child = Instantiate(script.sceneLevelManagerPrefabs, script.Map.transform);
            child.transform.SetParent(script.Map.transform);
            child.name = "level " + (script.sceneLevelManagers.Count + 1);
            SceneLevelManager sceneLevel = child.GetComponent<SceneLevelManager>();
            sceneLevel.sceneLevel.level = (script.sceneLevelManagers.Count + 1);
            script.sceneLevelManagers.Add(sceneLevel);
            script.sceneLevels.Add(sceneLevel.sceneLevel); 
        }
    }
#endif
}
