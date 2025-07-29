using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionViewer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI goalTxt;
    public SceneLevelManager selectedLevel;
    public ItemsManager itemsManager;

    void Start()
    {

    }
    public void ViewMission()
    {
        List<Item> itemList = new List<Item>();
        foreach (var item in selectedLevel.sceneLevel.items)
        {
            itemList.Add(item);
        }
        itemsManager.items = itemList;
        itemsManager.onStart();
        string missionTxt = "";
        foreach (var ms in selectedLevel.sceneLevel.mission)
        {
            missionTxt += ms + "\n";
        }
        goalTxt.text = missionTxt;
    }
    public void StartLevel()
    {
        selectedLevel.StartLevel();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
