using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<CustomAnimation> customAnimationSprites;

    [SerializeField] List<SpriteSetState> spriteSetStates;


    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool OnIce { get; set; }
    public bool AlwaysSlide { get; set; }
    public bool IsSpecialAnimating { get; private set; }
    public bool IsWaiting { get; private set; }
    public bool IsBusy { get => IsWaiting || IsSpecialAnimating || IsMoving; }

    [SerializeField] bool directionalIdle;
    bool wasOnIce;

    Dictionary<string, SpriteAnimator> animations;

    public int spriteSetStateIndex;
    public string currentAnim;
    public string nextAnim;
    bool wasPreviouslyMoving;
    float timer;
    float waitDuration;

    public event Action OnAnimationFinished;

    SpriteRenderer spriteRenderer;
    
    public int GetWidth()
    {
        return animations[currentAnim].GetWidth();
    }
    public int GetHeight()
    {
        return animations[currentAnim].GetHeight();
    }
    private void ReverseAnimation(List<FrameTime> source, ref List<FrameTime> target)
    {
        target = new List<FrameTime>
        {
            source[0]
        };
        for (int i = source.Count-1; i > 0; i--)
        {
            target.Add(source[i]);
        }
    }

    public IEnumerator SpecialAnimate(string animationName, string nextAnim = "idleDownAnim", Action OnAnimationFinished = null)
    {
        
        while (IsBusy)
            yield return null;
        if (animations.ContainsKey(animationName))
        {
            IsSpecialAnimating = true;
            this.OnAnimationFinished = OnAnimationFinished;
            currentAnim = animationName;
            this.nextAnim = nextAnim;
            animations[animationName].Start();
            yield return null;
        }
        else
        {
            Debug.Log("no such animation: " + animationName);
        }
    }

    public IEnumerator ChangeState(string nextAnim = "idleDownAnim", Action OnAnimationFinished = null)
    {
        return ChangeState((spriteSetStateIndex + 1) % spriteSetStates.Count, nextAnim, OnAnimationFinished);
    }
    public IEnumerator ChangeState(int index, string nextAnim = "idleDownAnim", Action OnAnimationFinished = null)
    {
        Debug.Log("State change");
        spriteSetStateIndex = index;
        animations["stateTrasitionAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].transitionSprites, spriteRenderer, false);
        if (spriteSetStates[spriteSetStateIndex].transitionSprites.Count > 0)
        {
            StartCoroutine(SpecialAnimate("stateTrasitionAnim", !String.IsNullOrEmpty(spriteSetStates[spriteSetStateIndex].transitionTo) ? spriteSetStates[spriteSetStateIndex].transitionTo : "idleDownAnim", OnAnimationFinished));
        }
        if (spriteSetStates[spriteSetStateIndex].walkDownSprites.Count == 0 && spriteSetStates[spriteSetStateIndex].walkUpSprites.Count > 0)
        {
            ReverseAnimation(spriteSetStates[spriteSetStateIndex].walkUpSprites, ref spriteSetStates[spriteSetStateIndex].walkDownSprites);
        }
        if (spriteSetStates[spriteSetStateIndex].walkDownSprites.Count > 0 && spriteSetStates[spriteSetStateIndex].walkUpSprites.Count == 0)
        {
            ReverseAnimation(spriteSetStates[spriteSetStateIndex].walkDownSprites, ref spriteSetStates[spriteSetStateIndex].walkUpSprites);
        }
        if (spriteSetStates[spriteSetStateIndex].walkRightSprites.Count > 0 && spriteSetStates[spriteSetStateIndex].walkLeftSprites.Count == 0)
        {
            ReverseAnimation(spriteSetStates[spriteSetStateIndex].walkLeftSprites, ref spriteSetStates[spriteSetStateIndex].walkRightSprites);
        }
        if (spriteSetStates[spriteSetStateIndex].walkRightSprites.Count == 0 && spriteSetStates[spriteSetStateIndex].walkLeftSprites.Count > 0)
        {
            ReverseAnimation(spriteSetStates[spriteSetStateIndex].walkRightSprites, ref spriteSetStates[spriteSetStateIndex].walkLeftSprites);
        }

        animations["walkDownAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].walkDownSprites, spriteRenderer);
        animations["walkUpAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].walkUpSprites, spriteRenderer);
        animations["walkRightAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].walkRightSprites, spriteRenderer);
        animations["walkLeftAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].walkLeftSprites, spriteRenderer);

        if (spriteSetStates[spriteSetStateIndex].idleDownSprites.Count == 0)
        {
            spriteSetStates[spriteSetStateIndex].idleDownSprites.Add(spriteSetStates[spriteSetStateIndex].walkDownSprites[0]);
        }
        if (directionalIdle)
        {
            if (spriteSetStates[spriteSetStateIndex].idleRightSprites.Count == 0)
            {
                spriteSetStates[spriteSetStateIndex].idleRightSprites.Add(spriteSetStates[spriteSetStateIndex].walkRightSprites[0]);
            }
            if (spriteSetStates[spriteSetStateIndex].idleLeftSprites.Count == 0)
            {
                spriteSetStates[spriteSetStateIndex].idleLeftSprites.Add(spriteSetStates[spriteSetStateIndex].walkLeftSprites[0]);
            }
            if (spriteSetStates[spriteSetStateIndex].idleUpSprites.Count == 0)
            {
                spriteSetStates[spriteSetStateIndex].idleUpSprites.Add(spriteSetStates[spriteSetStateIndex].walkUpSprites[0]);
            }
        }
        else
        {
            if (spriteSetStates[spriteSetStateIndex].idleRightSprites.Count == 0)
            {
                spriteSetStates[spriteSetStateIndex].idleRightSprites.AddRange(spriteSetStates[spriteSetStateIndex].idleDownSprites);
            }
            if (spriteSetStates[spriteSetStateIndex].idleLeftSprites.Count == 0)
            {
                spriteSetStates[spriteSetStateIndex].idleLeftSprites.AddRange(spriteSetStates[spriteSetStateIndex].idleDownSprites);
            }
            if (spriteSetStates[spriteSetStateIndex].idleUpSprites.Count == 0)
            {
                spriteSetStates[spriteSetStateIndex].idleUpSprites.AddRange(spriteSetStates[spriteSetStateIndex].idleDownSprites);
            }
        }

        animations["idleDownAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].idleDownSprites, spriteRenderer);
        animations["idleUpAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].idleUpSprites, spriteRenderer);
        animations["idleRightAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].idleRightSprites, spriteRenderer);
        animations["idleLeftAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].idleLeftSprites, spriteRenderer);

        if (spriteSetStates[spriteSetStateIndex].slideDownSprites.Count == 0)
        {
            spriteSetStates[spriteSetStateIndex].slideDownSprites.AddRange(spriteSetStates[spriteSetStateIndex].walkDownSprites);
        }
        if (spriteSetStates[spriteSetStateIndex].slideRightSprites.Count == 0)
        {
            spriteSetStates[spriteSetStateIndex].slideRightSprites.AddRange(spriteSetStates[spriteSetStateIndex].walkRightSprites);
        }
        if (spriteSetStates[spriteSetStateIndex].slideUpSprites.Count == 0)
        {
            spriteSetStates[spriteSetStateIndex].slideUpSprites.AddRange(spriteSetStates[spriteSetStateIndex].walkUpSprites);
        }
        if (spriteSetStates[spriteSetStateIndex].slideLeftSprites.Count == 0)
        {
            spriteSetStates[spriteSetStateIndex].slideLeftSprites.AddRange(spriteSetStates[spriteSetStateIndex].walkLeftSprites);
        }
        animations["slideDownAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].slideDownSprites, spriteRenderer);
        animations["slideUpAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].slideUpSprites, spriteRenderer);
        animations["slideRightAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].slideRightSprites, spriteRenderer);
        animations["slideLeftAnim"] = new SpriteAnimator(spriteSetStates[spriteSetStateIndex].slideLeftSprites, spriteRenderer);
        yield return null;
    }

    public IEnumerator Wait(float duration, Action OnAnimationFinished = null)
    {
        while (IsBusy)
            yield return null;
        waitDuration = duration;
        this.OnAnimationFinished = OnAnimationFinished;
        IsWaiting = true;
        yield return null;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animations = new Dictionary<string, SpriteAnimator>
        {
            { "stateTransitionAnim", null },

            { "walkDownAnim", null },
            { "walkUpAnim", null },
            { "walkRightAnim", null },
            { "walkLeftAnim", null },



            { "idleDownAnim", null },
            { "idleUpAnim", null },
            { "idleRightAnim", null },
            { "idleLeftAnim", null },



            { "slideDownAnim", null },
            { "slideUpAnim", null },
            { "slideRightAnim", null },
            { "slideLeftAnim", null }
        };
        foreach (CustomAnimation animation in customAnimationSprites)
        {
            animations.Add(animation.name, new SpriteAnimator(animation.frames, spriteRenderer, animation.looping));
        }
        currentAnim = "idleDownAnim";
        nextAnim = "idleDownAnim";
        StartCoroutine(ChangeState(spriteSetStateIndex));
        
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        if (wasPreviouslyMoving && !IsMoving)
        {
            OnAnimationFinished?.Invoke();
            OnAnimationFinished = null;
        }

        if (IsWaiting)
        {
            timer += Time.deltaTime;
            if (timer > waitDuration)
            {
                IsWaiting = false;
                timer = 0;
                waitDuration = 0;
                OnAnimationFinished?.Invoke();
                OnAnimationFinished = null;
            }
            return;
        }
        if (IsSpecialAnimating)
        {
            if (!animations[currentAnim].finished)
            {
                animations[currentAnim].HandleUpdate();
                return;
            }
            else
            {
                IsSpecialAnimating = false;
                currentAnim = nextAnim;
                OnAnimationFinished?.Invoke();
                OnAnimationFinished = null;
            }
            
        }
        if (IsMoving)
        {
            if (!OnIce || !AlwaysSlide)
            {
                if (wasOnIce)
                {
                    if (animations[currentAnim].AtStartSprite())
                    {
                        wasOnIce = false;
                        if (MoveX > 0)
                        {
                            currentAnim = "walkRightAnim";
                        }
                        else if (MoveX < 0)
                        {
                            currentAnim = "walkLeftAnim";
                        }
                        else if (MoveY > 0)
                        {
                            currentAnim = "walkUpAnim";
                        }
                        else if (MoveY < 0)
                        {
                            currentAnim = "walkDownAnim";
                        }
                    }
                }
                else
                {
                    if (MoveX > 0)
                    {
                        currentAnim = "walkRightAnim";
                    }
                    else if (MoveX < 0)
                    {
                        currentAnim = "walkLeftAnim";
                    }
                    else if (MoveY > 0)
                    {
                        currentAnim = "walkUpAnim";
                    }
                    else if (MoveY < 0)
                    {
                        currentAnim = "walkDownAnim";
                    }
                }

            }
            else
            {
                wasOnIce = true;
                if (MoveX > 0)
                {
                    currentAnim = "slideRightAnim";
                }
                else if (MoveX < 0)
                {
                    currentAnim = "slideLeftAnim";
                }
                else if (MoveY > 0)
                {
                    currentAnim = "slideUpAnim";
                }
                else if (MoveY < 0)
                {
                    currentAnim = "slideDownAnim";
                }
            }
        }
        if (currentAnim != prevAnim || (wasPreviouslyMoving != IsMoving && (!OnIce || !AlwaysSlide)) || (wasPreviouslyMoving != IsMoving && animations[currentAnim].AtStartSprite() && (OnIce || AlwaysSlide)))
            animations[currentAnim].Start();

        if (IsMoving)
        {
            animations[currentAnim].HandleUpdate();
        }
        else
        {
            if (!animations[currentAnim].AtStartSprite())
                animations[currentAnim].HandleUpdate();
            else
            {
                if ((MoveX<0.35f && MoveX>-0.35f) || (MoveY < 0.35f && MoveY > -0.35f) || currentAnim == "walkUpAnim" || currentAnim == "slideUpAnim" || currentAnim == "walkRightAnim" || currentAnim == "slideRightAnim" || currentAnim == "walkLeftAnim" || currentAnim == "slideLeftAnim" || currentAnim == "walkDownAnim" || currentAnim == "slideDownAnim")
                {
                    //spriteRenderer.sprite = animations[currentAnim].Frames[0];
                    if (MoveY > 0)
                    {
                        currentAnim = "idleUpAnim";
                        animations[currentAnim].Start();
                        
                    }
                    else if (MoveX > 0)
                    {
                        currentAnim = "idleRightAnim";
                        animations[currentAnim].Start();
                        
                    }
                    else if (MoveX < 0)
                    {
                        currentAnim = "idleLeftAnim";
                        animations[currentAnim].Start();
                        
                    }
                    else if (MoveY < 0)
                    {
                        currentAnim = "idleDownAnim";
                        animations[currentAnim].Start();
                        
                    }
                    if ((MoveX < 0.35f && MoveX > -0.35f) || (MoveY < 0.35f && MoveY > -0.35f))
                    {
                        OnAnimationFinished?.Invoke();
                        OnAnimationFinished = null;
                    }
                }
                animations[currentAnim].HandleUpdate();
            }
                
        }
        wasPreviouslyMoving = IsMoving;
    }
}
[Serializable]
internal class SpriteSetState
{
    public List<FrameTime> transitionSprites;

    public string transitionTo;

    public List<FrameTime> walkDownSprites;
    public List<FrameTime> walkUpSprites;
    public List<FrameTime> walkRightSprites;
    public List<FrameTime> walkLeftSprites;

    public List<FrameTime> idleDownSprites;
    public List<FrameTime> idleUpSprites;
    public List<FrameTime> idleRightSprites;
    public List<FrameTime> idleLeftSprites;

    public List<FrameTime> slideDownSprites;
    public List<FrameTime> slideUpSprites;
    public List<FrameTime> slideRightSprites;
    public List<FrameTime> slideLeftSprites;
}

[Serializable]
internal class CustomAnimation
{
    public string name;
    public bool looping;
    public List<FrameTime> frames;
}
