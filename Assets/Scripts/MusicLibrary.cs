using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}

[System.Serializable]
public struct SFXTrack
{
    public string sfxName;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;
    public SFXTrack[] sfxTracks;

    public AudioClip GetClipFromName(string trackName)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)
            {
                Debug.Log("✅ Found music track: " + trackName);
                return track.clip;
            }
        }

        Debug.LogWarning("❌ Music track not found: " + trackName);
        return null;
    }


    public AudioClip GetSFXClip(string sfxName)
    {
        foreach (var sfx in sfxTracks)
        {
            if (sfx.sfxName == sfxName)
                return sfx.clip;
        }
        return null;
    }

}
