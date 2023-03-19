using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    readonly SpriteRenderer spriteRenderer;
    readonly List<FrameTime> frames;

    int currentFrame;
    float timer;

    bool looping;
    public bool finished;
    

    public SpriteAnimator(List<FrameTime> frames, SpriteRenderer spriteRenderer, bool looping = true)
    {
        this.looping = looping;
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        
    }

    

    public int GetWidth()
    {
        return (int) Math.Ceiling(frames[currentFrame].sprite.rect.width / 32);
    }
    public int GetHeight()
    {
        return (int) Math.Ceiling(frames[currentFrame].sprite.rect.height / 32);
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frames[currentFrame].duration)
        {
            //if (!frames[currentFrame].sprite.name.Contains("Giji"))
                //Debug.Log(frames[currentFrame].sprite.name);
            if (!looping && currentFrame+2 > frames.Count)
            {
                finished = true;
                return;
            }
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame].sprite;
            timer -= frames[currentFrame].duration;
        }
    }
    public void Start()
    {
        finished = false;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0].sprite;
    }
    public bool AtStartSprite()
    {
        return frames[currentFrame].sprite.Equals(frames[0].sprite);
    }
}
