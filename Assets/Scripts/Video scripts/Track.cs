using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TrackScriptableObject", menuName ="ScriptableObjects/Track")]
public class Track : ScriptableObject
{
    public Author author;
    public Album album;
    public Author featuring;

    public AudioClip audioClip;
    public Sprite audioImage;
    public VideoClip videoClip;
    public TrackType trackType;

    public enum TrackType
    {
        Video,
        Music
    }

    private void OnValidate()
    {
        if (trackType == TrackType.Music)
        {
            videoClip = null;
            if (audioImage == null && album != null)
            {
                audioImage = album.albumArt;
            }
        } else
        {
            audioClip = null;
            audioImage = null;
        }
        
    }


}
