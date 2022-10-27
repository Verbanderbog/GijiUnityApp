using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System;
using UnityEngine.UI;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private AudioSource audioPlayer;
    [SerializeField] private Image audioImage;
    [SerializeField] private RawImage videoImage;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI timestamp;
    [SerializeField] private Slider volume;
    [SerializeField] private Slider progress;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private JukeboxList JukeboxList;
    public Playlist playlist;
    [SerializeField] private Toggle autoplay;
    public int playlistIndex;
    private VideoProgressBar progressScript;
    private bool restart;

    // Start is called before the first frame update
    void Start()
    {
        
        SetTrack(playlistIndex);
    }

    private void OnValidate()
    {
        if (JukeboxList != null)
            playlist = JukeboxList.playlist;
    }

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioPlayer = GetComponent<AudioSource>();
        progressScript = progress.GetComponent<VideoProgressBar>();
        
        SetTrack(playlistIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (playlist.tracks[playlistIndex].trackType == Track.TrackType.Video)
        {
            if (videoPlayer.time >= videoPlayer.length - 0.05F)
            {
                if (autoplay.isOn) 
                {
                    Skip();
                    return;
                }
                
            }
            timestamp.SetText(GetStamp(videoPlayer.time) + " / " + GetStamp(videoPlayer.length));
            if (!progressScript.InRect)
            {
                if (videoPlayer.isPlaying)
                {
                    if (playButton.activeSelf)
                    {
                        playButton.SetActive(false);
                        pauseButton.SetActive(true);
                    }
                }
                else
                {
                    if (pauseButton.activeSelf)
                    {
                        playButton.SetActive(true);
                        pauseButton.SetActive(false);
                    }
                }
            }
        } else
        {
            if (audioPlayer.time >= audioPlayer.clip.length - 0.05F)
            {
                if (autoplay.isOn)
                {
                    Skip();
                    return;
                } else
                {
                    audioPlayer.Pause();
                    audioPlayer.time = audioPlayer.clip.length - 0.05F;
                    restart = true;
                }
                
            }

            timestamp.SetText(GetStamp(audioPlayer.time) + " / " + GetStamp(audioPlayer.clip.length));
            if (!progressScript.InRect)
            {
                if (audioPlayer.isPlaying)
                {
                    if (playButton.activeSelf)
                    {
                        playButton.SetActive(false);
                        pauseButton.SetActive(true);
                    }

                }
                else
                {
                    if (pauseButton.activeSelf)
                    {
                        playButton.SetActive(true);
                        pauseButton.SetActive(false);
                    }
                }
            }
        }
    }


    private string GetStamp(double seconds)
    {

        return String.Format("{0:D}:{1:D2}", (int)(seconds / 60), (int)(seconds % 60));
    }

    private void OnEnable()
    {
        if (JukeboxList != null)
            playlist = JukeboxList.playlist;
        title.SetText(playlist.tracks[playlistIndex].name);
        volume.value = PlayerPrefs.GetFloat(volume.name);

    }

    public void ChangeTime(int delta)
    {
        if (playlist.tracks[playlistIndex].trackType == Track.TrackType.Video)
        {
            var output = videoPlayer.time + delta;
            if (output >= videoPlayer.length - 0.05F)
            {
                if (autoplay.isOn)
                {
                    Skip();
                    return;
                }
                    videoPlayer.time = videoPlayer.length-0.05F;

            }
            else if (output < 0)
            {
                videoPlayer.time = 0;
            }
            else
            {
                videoPlayer.time = output;
            }
        } else
        {
            var output = audioPlayer.time + delta;
            if (output >= audioPlayer.clip.length - 0.05F)
            {
                if (autoplay.isOn)
                {
                    Skip();
                    return;
                }
                audioPlayer.time = audioPlayer.clip.length - 0.05F;
                audioPlayer.Pause();
            }
            else if (output < 0)
            {
                audioPlayer.time = 0;
            }
            else
            {
                audioPlayer.time = output;
            }
        }

    }

    public void Skip()
    {
        if (++playlistIndex > playlist.tracks.Length - 1)
            playlistIndex = 0;
        SetTrack(playlistIndex);
    }

    public void Back()
    {
        if (--playlistIndex < 0)
            playlistIndex = playlist.tracks.Length - 1;
        SetTrack(playlistIndex);
    }

    public void SetTrack(int index)
    {
        restart = false;
        bool isPlaying = (videoPlayer.clip!=null) ? videoPlayer.isPlaying : audioPlayer.isPlaying;
        Track curr = playlist.tracks[index];
        title.SetText(curr.name);
        if (curr.trackType == Track.TrackType.Video)
        {

            videoImage.gameObject.SetActive(true);
            videoPlayer.clip = curr.videoClip;
            audioPlayer.clip = null;
            audioImage.sprite = null;
            audioImage.enabled = false;

            videoPlayer.Prepare();
            videoPlayer.time = 0;
            if (isPlaying)
                videoPlayer.Play();
            else
                videoPlayer.Pause();
        }
        else
        {
            videoPlayer.clip = null;
            videoImage.gameObject.SetActive(false);
            audioPlayer.clip = curr.audioClip;
            audioImage.sprite = curr.audioImage;
            audioImage.preserveAspect = true;
            audioImage.enabled = true;
            audioPlayer.time = 0;

            if (isPlaying)
                audioPlayer.Play();
            else
                audioPlayer.Pause();
        }
        progress.value = 0;

    }

    public void Play()
    {
        if (playlist.tracks[playlistIndex].trackType == Track.TrackType.Video)
        {
            if (videoPlayer.frame == (long)videoPlayer.frameCount)
            {
                videoPlayer.frame = 0;
            }
            videoPlayer.Play();
        }
        else
        {
            if (restart || audioPlayer.time >= audioPlayer.clip.length - 0.05F )
            {
                audioPlayer.time = 0;
                restart = false;
            }
            audioPlayer.Play();
        }
    }

    public void Pause()
    {
        if (playlist.tracks[playlistIndex].trackType == Track.TrackType.Video)
        {
            videoPlayer.Pause();
        } 
        else
        {
            audioPlayer.Pause();
        }
    }
}
