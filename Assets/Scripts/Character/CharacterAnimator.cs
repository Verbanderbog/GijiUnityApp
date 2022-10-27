using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    [SerializeField] List<Sprite> slideDownSprites;
    [SerializeField] List<Sprite> slideUpSprites;
    [SerializeField] List<Sprite> slideRightSprites;
    [SerializeField] List<Sprite> slideLeftSprites;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool OnIce { get; set; }
    bool wasOnIce;

    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator slideRightAnim;
    SpriteAnimator slideLeftAnim;
    SpriteAnimator slideUpAnim;
    SpriteAnimator slideDownAnim;

    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        slideDownAnim = new SpriteAnimator(slideDownSprites, spriteRenderer, 0.08f);
        slideUpAnim = new SpriteAnimator(slideUpSprites, spriteRenderer, 0.08f);
        slideRightAnim = new SpriteAnimator(slideRightSprites, spriteRenderer, 0.08f);
        slideLeftAnim = new SpriteAnimator(slideLeftSprites, spriteRenderer,0.08f);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        if (!OnIce)
        {
            if (wasOnIce)
            {
                if (currentAnim.AtStartSprite())
                {
                    wasOnIce=false;
                    if (MoveX > 0)
                    {
                        currentAnim = walkRightAnim;
                    }
                    else if (MoveX < 0)
                    {
                        currentAnim = walkLeftAnim;
                    }
                    else if (MoveY > 0)
                    {
                        currentAnim = walkUpAnim;
                    }
                    else if (MoveY < 0)
                    {
                        currentAnim = walkDownAnim;
                    }
                }
            }
            else
            {
                if (MoveX > 0)
                {
                    currentAnim = walkRightAnim;
                }
                else if (MoveX < 0)
                {
                    currentAnim = walkLeftAnim;
                }
                else if (MoveY > 0)
                {
                    currentAnim = walkUpAnim;
                }
                else if (MoveY < 0)
                {
                    currentAnim = walkDownAnim;
                }
            }
            
        }
        else
        {
            wasOnIce = true;
            if (MoveX > 0)
            {
                currentAnim = slideRightAnim;
            }
            else if (MoveX < 0)
            {
                currentAnim = slideLeftAnim;
            }
            else if (MoveY > 0)
            {
                currentAnim = slideUpAnim;
            }
            else if (MoveY < 0)
            {
                currentAnim = slideDownAnim;
            }
        }

        if (currentAnim != prevAnim || (wasPreviouslyMoving != IsMoving && !OnIce) || (wasPreviouslyMoving != IsMoving && currentAnim.AtStartSprite() && OnIce))
            currentAnim.Start();

        if (IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            if (!currentAnim.AtStartSprite())
                currentAnim.HandleUpdate();
            else
            {
                spriteRenderer.sprite = currentAnim.Frames[0];
            }
                
        }
        wasPreviouslyMoving = IsMoving;
    }
}
