using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgressBar : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private FadeUI panel;
    private Slider progress;
    [SerializeField] private Image bar;
    private bool inRect;
    private bool playState;

    public bool InRect { get => inRect; set => inRect = value; }

    private void Awake()
    {
        InRect = false;
        progress = GetComponent<Slider>();
        //bar = this.transform.Find("Background").GetComponent<Image>();
    }

    private void Update()
    {
        if (videoPlayer.clip != null)
        {
            if (videoPlayer.frameCount > 0 && !InRect)
                progress.value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
        } else if (audioPlayer.clip != null)
        {
            if (audioPlayer.time > 0 && !InRect)
                progress.value = audioPlayer.time / audioPlayer.clip.length;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (InRect)
        {
            SkipToPercent(progress.value);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bar.rectTransform, eventData.position, null, out _))
        {
            InRect = true;
            if (videoPlayer.clip != null)
            {
                playState = videoPlayer.isPlaying;
                videoPlayer.Pause();
            }
            else
            {
                playState = audioPlayer.isPlaying;
                audioPlayer.Pause();
            }
            SkipToPercent(progress.value);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (InRect)
        {
            InRect = false;
            if (playState) {
                if (videoPlayer.clip != null)
                {
                    videoPlayer.Play();
                }
                else if (audioPlayer.clip != null)
                {
                    audioPlayer.Play();
                }
            }
                
        }
    }

    /*private void TrySkip(PointerEventData eventData)
    {

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bar.rectTransform, eventData.position, null, out Vector2 localPoint))
        {
            float pct = Mathf.InverseLerp(bar.rectTransform.rect.xMin, bar.rectTransform.rect.xMax, localPoint.x);
            SkipToPercent(progress.value);
        }
    }*/

    private void SkipToPercent(float pct)
    {
        
        if (videoPlayer.clip != null)
        {
            var frame = videoPlayer.frameCount * pct;
            videoPlayer.frame = (long)frame;
        }
        else if (audioPlayer.clip != null)
        {
            var adjust = 0F;
            if (pct >= 1)
            {
                adjust = 0.05F;
                pct = 1;
            }
            audioPlayer.time = (audioPlayer.clip.length * pct) - adjust;
        }
    }
    
}

