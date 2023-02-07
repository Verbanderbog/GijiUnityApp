using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    [SerializeField] List<Sprite> idleDownSprites;
    [SerializeField] List<Sprite> idleUpSprites;
    [SerializeField] List<Sprite> idleRightSprites;
    [SerializeField] List<Sprite> idleLeftSprites;

    [SerializeField] List<Sprite> slideDownSprites;
    [SerializeField] List<Sprite> slideUpSprites;
    [SerializeField] List<Sprite> slideRightSprites;
    [SerializeField] List<Sprite> slideLeftSprites;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool OnIce { get; set; }
    [SerializeField] bool directionalIdle;
    bool wasOnIce;

    Dictionary<string, SpriteAnimator> animations;



    public string currentAnim;
    bool wasPreviouslyMoving;

    SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animations = new Dictionary<string, SpriteAnimator>();
        animations.Add("walkDownAnim", new SpriteAnimator(walkDownSprites, spriteRenderer));
        animations.Add("walkUpAnim", new SpriteAnimator(walkUpSprites, spriteRenderer));
        animations.Add("walkRightAnim", new SpriteAnimator(walkRightSprites, spriteRenderer));
        animations.Add("walkLeftAnim", new SpriteAnimator(walkLeftSprites, spriteRenderer));

        if (idleDownSprites.Count==0){
            idleDownSprites.Add(walkDownSprites[0]);
        }
        if (directionalIdle){
            if (idleRightSprites.Count==0){
                idleRightSprites.Add(walkRightSprites[0]);
            }
            if (idleLeftSprites.Count==0){
                idleLeftSprites.Add(walkLeftSprites[0]);
            }
            if (idleUpSprites.Count==0){
                idleUpSprites.Add(walkUpSprites[0]);
            }
        }else{
            if (idleRightSprites.Count==0){
                idleRightSprites.AddRange(idleDownSprites);
            }
            if (idleLeftSprites.Count==0){
                idleLeftSprites.AddRange(idleDownSprites);
            }
            if (idleUpSprites.Count==0){
                idleUpSprites.AddRange(idleDownSprites);
            }
        }
        
        animations.Add("idleDownAnim", new SpriteAnimator(idleDownSprites, spriteRenderer));
        animations.Add("idleUpAnim", new SpriteAnimator(idleUpSprites, spriteRenderer));
        animations.Add("idleRightAnim", new SpriteAnimator(idleRightSprites, spriteRenderer));
        animations.Add("idleLeftAnim", new SpriteAnimator(idleLeftSprites, spriteRenderer));

        animations.Add("slideDownAnim", new SpriteAnimator(slideDownSprites, spriteRenderer, 0.08f));
        animations.Add("slideUpAnim", new SpriteAnimator(slideUpSprites, spriteRenderer, 0.08f));
        animations.Add("slideRightAnim", new SpriteAnimator(slideRightSprites, spriteRenderer, 0.08f));
        animations.Add("slideLeftAnim", new SpriteAnimator(slideLeftSprites, spriteRenderer,0.08f));

        currentAnim = "idleDownAnim";
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        if (!OnIce)
        {
            if (wasOnIce)
            {
                if (animations[currentAnim].AtStartSprite())
                {
                    wasOnIce=false;
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

        if (currentAnim != prevAnim || (wasPreviouslyMoving != IsMoving && !OnIce) || (wasPreviouslyMoving != IsMoving && animations[currentAnim].AtStartSprite() && OnIce))
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
                //spriteRenderer.sprite = animations[currentAnim].Frames[0];
                if (currentAnim == "walkUpAnim" || currentAnim == "slideUpAnim")
                {
                    currentAnim = "idleUpAnim";
                    animations[currentAnim].Start();
                }
                else if (currentAnim == "walkRightAnim" || currentAnim == "slideRightAnim")
                {
                    currentAnim = "idleRightAnim";
                    animations[currentAnim].Start();

                }
                else if (currentAnim == "walkLeftAnim" || currentAnim == "slideLeftAnim")
                {
                    currentAnim = "idleLeftAnim";
                    animations[currentAnim].Start();
                }
                else if (currentAnim == "walkDownAnim" || currentAnim == "slideDownAnim")
                {
                    currentAnim = "idleDownAnim";
                    animations[currentAnim].Start();
                }
                
                animations[currentAnim].HandleUpdate();
            }
                
        }
        wasPreviouslyMoving = IsMoving;
    }
}
