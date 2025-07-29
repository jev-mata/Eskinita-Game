using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject LevelStoryPanel;
    public TextMeshProUGUI characterTMP;
    public TextMeshProUGUI messageTMP;
    public RawImage bgImageToPlay;
    public List<StoryLineWithImagae> storyLineWithImagaes;
    public int currentPage = 0;
    public int currentLine = 0;

    void Start()
    {

        Time.timeScale = 0;
    }

    // Update is called once per frames
    void Update()
    {
        if (storyLineWithImagaes[currentPage].storyImage)
        {

            bgImageToPlay.texture = storyLineWithImagaes[currentPage].storyImage;
        }
        if (storyLineWithImagaes[currentPage].storyLine[currentLine] != null)
        {
            characterTMP.text = storyLineWithImagaes[currentPage].storyLine[currentLine].characterName;
            messageTMP.text = storyLineWithImagaes[currentPage].storyLine[currentLine].speakLine;
        }
    }
    public void next()
    {
        if (storyLineWithImagaes[currentPage].storyLine.Count-1 > currentLine)
        {
            currentLine++;
        }
        else if (storyLineWithImagaes.Count-1 > currentPage)
        {
            currentPage++;
        }
        else
        {
            Time.timeScale = 1;
            LevelStoryPanel.SetActive(false);
        }
    }
}
[System.Serializable]
public class StoryLineWithImagae
{

    public Texture storyImage;
    public List<StoryLine> storyLine;
}
[System.Serializable]
public class StoryLine
{

    public string characterName;
    public string speakLine;
}
