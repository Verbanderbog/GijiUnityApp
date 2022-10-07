using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    SpriteRenderer spriteRenderer;
    List<Sprite> frames;
    float framerate;

    int currentFrame;
    float timer;

    public List<Sprite> Frames { get => frames; }

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float framerate=0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.framerate = framerate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > framerate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= framerate;
        }
    }
}
