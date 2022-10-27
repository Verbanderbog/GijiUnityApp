using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeboxList : MonoBehaviour
{
    public Playlist playlist;
    public GameObject trackPrefab;
    public GameObject sectionPrefab;
    public VideoPlayerController playerController;
    public GameObject videoPlayerPanel;
    public GameObject jukeboxMenu;
    // Start is called before the first frame update
    void Start()
    {
        GameObject videoPanel = Instantiate(sectionPrefab, this.transform, false) as GameObject;
        var videoUI = videoPanel.GetComponent<SectionPanelUI>();
        videoUI.sectionName.SetText("Videos");
        GameObject musicPanel = Instantiate(sectionPrefab, this.transform, false) as GameObject;
        var musicUI = musicPanel.GetComponent<SectionPanelUI>();
        musicUI.sectionName.SetText("Music");

        for (int i=0;i < playlist.tracks.Length;i++)
        {
            Transform t = (playlist.tracks[i].trackType == Track.TrackType.Video) ? videoUI.contents.transform : musicUI.contents.transform;
            GameObject trackPanel = Instantiate(trackPrefab, t, false) as GameObject;
            var trackUI = trackPanel.GetComponent<TrackPanelUI>();
            trackUI.track = playlist.tracks[i];
            trackUI.trackIndex = i;
            trackUI.playerController = playerController;
            trackUI.videoPlayerPanel = videoPlayerPanel;
            trackUI.jukeboxMenu = jukeboxMenu;
            trackUI.Initialize();
        }
        /*foreach (Album album in playlist.AlbumContents.Keys)
        {
            foreach (Track track in playlist.AlbumContents[album][Track.TrackType.Video])
            {
                GameObject trackPanel = Instantiate(trackPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                var trackUI = trackPanel.GetComponent<TrackPanelUI>();
                trackUI.track = track;
                trackUI.trackIndex = playlist.tracks.
                trackPanel.transform.parent = videoUI.contents.transform;
            }
            foreach (Track track in playlist.AlbumContents[album][Track.TrackType.Music])
            {
                GameObject trackPanel = Instantiate(trackPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                var trackUI = trackPanel.GetComponent<TrackPanelUI>();
                trackUI.track = track;
                trackPanel.transform.parent = musicUI.contents.transform;
            }
        }*/
        
    }

    // Update is called once per frame

}
