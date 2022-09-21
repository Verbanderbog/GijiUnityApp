using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Video;

public class FadeUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private float fadeOutStartTimeSec;
    [SerializeField] private float fadeOutEndTimeSec;
    [SerializeField] private float fadeInTimeSec;
    private TimeSpan fadeOutStartTime;
    private TimeSpan fadeOutEndTime;
    private TimeSpan fadeInTime;
    private CanvasGroup canvasGroup;
    private DateTime activeTime;
    private DateTime startTime;
    private bool forceFade;
    public void OnDrag(PointerEventData eventData)
    {
        activeTimer();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        activeTimer();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        activeTimer();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        activeTimer();
    }

   
    void Awake()
    {
        forceFade = false;
        canvasGroup = GetComponent<CanvasGroup>();
        activeTimer();
        startTime = DateTime.Now;
    }

    void Start()
    {
        forceFade = false;
        startTime = DateTime.Now;
    }

    private void OnValidate()
    {
        fadeOutEndTime = TimeSpan.FromSeconds(fadeOutEndTimeSec);
        fadeOutStartTime = TimeSpan.FromSeconds(fadeOutStartTimeSec);
        fadeInTime = TimeSpan.FromSeconds(fadeInTimeSec);
    }

    private void OnEnable()
    {
        startTime = DateTime.Now;
    }


    void Update()
    {
        if (videoPlayer.isPlaying || audioPlayer.isPlaying)
        {
            TimeSpan timeElapsedFromStart = DateTime.Now - startTime;
            TimeSpan timeElapsedFromActive = DateTime.Now - activeTime;
            if (timeElapsedFromActive.CompareTo(fadeOutStartTime) > 0)
            {
                float percentComplete = (float)((timeElapsedFromActive - fadeOutStartTime) / (fadeOutEndTime - fadeOutStartTime));
                if (percentComplete >= 1)
                {
                    percentComplete = 1;
                    forceFade = false;
                    gameObject.SetActive(false);
                }
                canvasGroup.alpha = 1 - percentComplete;
            }
            else if (timeElapsedFromStart.CompareTo(fadeInTime) <= 0)
            {
                float percentComplete = (float)((timeElapsedFromStart) / (fadeInTime));

                canvasGroup.alpha = percentComplete;
            }
            else if (canvasGroup.alpha != 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        else if (canvasGroup.alpha != 1)
        {
            canvasGroup.alpha = 1;
        }
    }

    public void activeTimer()
    {
        if (!forceFade)
            activeTime = DateTime.Now;
    }

    public void fadeOut()
    {
        forceFade = true;
        activeTime = DateTime.Now - (fadeOutStartTime + (fadeOutEndTime-fadeOutStartTime)/2);
    }
}
