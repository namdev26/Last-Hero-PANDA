using System;
using UnityEngine;

[Serializable]
public struct MusicTrack
{
    public string groupID;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] musicTracks;

    public AudioClip GetMusicFromName(string name)
    {
        foreach (var musicTrack in musicTracks)
        {
            if (musicTrack.groupID == name)
            {
                return musicTrack.clip;
            }
        }
        return null;
    }
}
