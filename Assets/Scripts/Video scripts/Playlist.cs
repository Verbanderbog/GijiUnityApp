using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlaylistScriptableObject", menuName = "ScriptableObjects/Playlist")]
public class Playlist : ScriptableObject
{
    public Track[] tracks;
    public bool sorted;
    private SortedDictionary<Album, Dictionary<Track.TrackType, List<Track>>> albumContents = new();

    public SortedDictionary<Album, Dictionary<Track.TrackType, List<Track>>> AlbumContents { get => albumContents; }

    private void OnValidate()
    {
        foreach (Track t in tracks)
        {
            
            if (!albumContents.ContainsKey(t.album))
            {
                albumContents.Add(t.album, new Dictionary<Track.TrackType, List<Track>>() { { Track.TrackType.Music, new List<Track>() }, { Track.TrackType.Video, new List<Track>() } });
                
            }
            albumContents[t.album][t.trackType].Add(t);
        }
    }
}
