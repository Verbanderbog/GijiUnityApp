using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System;
using UnityEngine.UI;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer player;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI timestamp;
    [SerializeField] private Slider progress;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.Prepare();

    }

    // Update is called once per frame
    void Update()
    {
        timestamp.SetText(getStamp(player.time) + " / " + getStamp(player.length));
        
    }

    private string getStamp(double seconds)
    {
        return String.Format("{0:D}:{1:D2}", Convert.ToInt32(seconds / 60), Convert.ToInt32(seconds % 60));
    }

    public void fadeOutUI()
    {

    }
}
