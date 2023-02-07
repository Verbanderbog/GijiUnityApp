using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    Image image;

    bool fading;
    bool fadeIn;
    bool blocking;
    float time;
    float maxTime;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void Update()
    {
        if (fading)
        {
            image.raycastTarget = blocking;
            float alpha;
            time += Time.deltaTime;
            if (time > maxTime)
                time = maxTime;
            if (fadeIn)
            {
                alpha = time / maxTime;
            }
            else
            {
                alpha = (maxTime - time) / maxTime;
            }
            
            image.color = new Color(0, 0, 0, alpha);
            if (time == maxTime)
            {
                if (!fadeIn)
                    image.raycastTarget = false;
                fading = false;
            }
        }
    }

    public IEnumerator FadeOut(float time, bool blocking = true)
    {
        
        fadeIn = false;
        this.blocking = blocking;
        this.time = 0;
        maxTime = time;
        fading = true;
        while (fading) { yield return null; }
    }

    public IEnumerator FadeIn(float time, bool blocking = true)
    {
        fadeIn = true;
        this.blocking = blocking;
        this.time = 0;
        maxTime = time;
        fading = true;
        while (fading) { yield return null; }
        
    }
}
