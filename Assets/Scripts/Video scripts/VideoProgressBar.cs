using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgressBar : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    private Slider progress;
    [SerializeField] private Image bar;
    private bool inRect;
    private bool playState;

    private void Awake()
    {
        inRect = false;
        progress = GetComponent<Slider>();
        //bar = this.transform.Find("Background").GetComponent<Image>();
    }

    private void Update()
    {
        if (videoPlayer.frameCount > 0 && videoPlayer.isPlaying)
            progress.value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (inRect)
        {
            SkipToPercent(progress.value);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bar.rectTransform, eventData.position, null, out localPoint))
        {
            inRect = true;
            playState = videoPlayer.isPlaying;
            videoPlayer.Pause();
            SkipToPercent(progress.value);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (inRect)
        {
            inRect = false;
            if (playState)
                videoPlayer.Play();
        }
    }

    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bar.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(bar.rectTransform.rect.xMin, bar.rectTransform.rect.xMax, localPoint.x);
            SkipToPercent(progress.value);
        }
    }

    private void SkipToPercent(float pct)
    {
        var frame = videoPlayer.frameCount * pct;
        videoPlayer.frame = (long)frame;
    }

    
}

