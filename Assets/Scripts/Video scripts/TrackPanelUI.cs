using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trackName;
    [SerializeField] private TextMeshProUGUI trackDuration;
    [SerializeField] private TextMeshProUGUI trackAuthorAlbum;
    [SerializeField] private TextMeshProUGUI trackFeaturing;
    public VideoPlayerController playerController;
    public GameObject videoPlayerPanel;
    public GameObject jukeboxMenu;
    public int trackIndex;
    public Track track;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (track != null)
        {
            var seconds = (track.trackType == Track.TrackType.Video) ? track.videoClip.length : track.audioClip.length;
            trackName.SetText(track.name);
            trackDuration.SetText(String.Format("{0:D}:{1:D2}", (int)(seconds / 60), (int)(seconds % 60)));
            var album = (track.album != null) ? " - " + track.album.name : "";
            trackAuthorAlbum.SetText(track.author.name + album);
            var featuring = (track.featuring != null) ? "(ft. " + track.featuring.name + ")" : "";
            trackFeaturing.SetText(featuring);
            this.gameObject.GetComponent<Button>().onClick.AddListener(Play);
        }
    }

    public void Play()
    {
        jukeboxMenu.SetActive(false);
        videoPlayerPanel.SetActive(true);
        playerController.playlistIndex = trackIndex;
        playerController.SetTrack(trackIndex);
        playerController.Play();

    }
}
