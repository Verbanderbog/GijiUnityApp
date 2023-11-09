using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    NPCController controller;
    CharacterAnimator animator;
    AudioSource audioSource;
    [SerializeField] FootstepSounds footsteps;
    public float moveSpeed;
    public float OffsetY = 0.3f;
    public bool AlwaysSlide;

    public CharacterAnimator Animator { get => animator; }
    public bool IsMoving { get; private set; }
    public bool OnIce { get; private set; }
    

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<NPCController>();
        SetPostitionAndSnapToTile(transform.position);
    }
    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
        animator.OnIce = OnIce;
        animator.AlwaysSlide = AlwaysSlide;
        if (IsMoving && !audioSource.isPlaying && audioSource.clip != null)
            audioSource.Play();
        else if (!IsMoving && audioSource.isPlaying)
            audioSource.Pause();

    }
    public void SetDialogIndex(int index)
    {
        controller.dialogIndex = index;
    }
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null, Action OnAnimationFinished = null)
    {
       
        animator.OnAnimationFinished += OnAnimationFinished;
        if (animator == null)
            animator = GetComponent<CharacterAnimator>();
        animator.MoveX = moveVec.x;
        animator.MoveY = moveVec.y;

        moveVec.x = AdjustInput(moveVec.x);
        moveVec.y = AdjustInput(moveVec.y);

        var targetPos = transform.position;

        var distance = Convert.ToInt32(moveVec.magnitude);
        Vector3 dir = moveVec.normalized;
        Vector3 positionAdjust;
        if (dir.y != 0)
        {
            int height = animator.GetHeight();
            positionAdjust = new Vector3(0, height-2, 0);
        }
        else if (dir.x!=0)
        {
            int width = animator.GetWidth();
            positionAdjust = new Vector3(width-1, 0, 0);
        } 
        else
        {
            positionAdjust = Vector3.zero;
        }
        var walkable = IsWalkable(transform.position + positionAdjust + dir, dir);
        if (!walkable[0])
        {
            yield break;
        }
        while (walkable[1])
        {
            yield return null;
            if (dir.y != 0)
            {
                int height = animator.GetHeight();
                positionAdjust = new Vector3(0, height - 2, 0);
            }
            else if (dir.x != 0)
            {
                int width = animator.GetWidth();
                positionAdjust = new Vector3(width - 1, 0, 0);
            }
            else
            {
                positionAdjust = Vector3.zero;
            }
            walkable = IsWalkable(transform.position + positionAdjust + dir, dir);
        }
        IsMoving = true;
        for (int i = distance; i > 0; i--)
        {
            var breakWalking = false;
            targetPos += dir;
            var prevPos = transform.position;
            var targetTile = TileDetector.Instance.GetTileType(targetPos);
            if (!footsteps.GetClip(targetTile).Equals(audioSource.clip))
            {
                audioSource.clip = footsteps.GetClip(targetTile);
            }
            if (targetTile == TileType.Ice)
            {
                OnIce = true;
                i++;
            }
            else
            {
                if (OnIce || AlwaysSlide)
                {
                    transform.position = prevPos;
                    breakWalking = true;
                }
                OnIce = false;
            }
            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                if (dir.y != 0)
                {
                    int height = animator.GetHeight();
                    positionAdjust = new Vector3(0, height - 2, 0);
                }
                else if (dir.x != 0)
                {
                    int width = animator.GetWidth();
                    positionAdjust = new Vector3(width - 1, 0, 0);
                }
                else
                {
                    positionAdjust = Vector3.zero;
                }
                walkable = IsWalkable(targetPos + positionAdjust, dir);
                if (targetTile == TileType.Empty)
                {
                    //walkable[0] = false;
                }
                    
                if (!walkable[0])
                {
                    transform.position = prevPos;
                    breakWalking = true;
                    break;
                }
                while (walkable[1])
                {
                    IsMoving = false;
                    yield return null;
                    walkable = IsWalkable(targetPos + positionAdjust, dir);
                    IsMoving = true;

                }
                yield return null;
            }
            if (breakWalking)
                break;
            transform.position = targetPos;
            walkable = IsWalkable(transform.position + positionAdjust + dir, dir);
            if (!walkable[0])
            {
                break;
            }
            while (walkable[1])
            {
                if (dir.y != 0)
                {
                    int height = animator.GetHeight();
                    positionAdjust = new Vector3(0, height - 2, 0);
                }
                else if (dir.x != 0)
                {
                    int width = animator.GetWidth();
                    positionAdjust = new Vector3(width - 1, 0, 0);
                }
                else
                {
                    positionAdjust = Vector3.zero;
                }
                IsMoving = false;
                yield return null;
                walkable = IsWalkable(transform.position + positionAdjust + dir, dir);
                IsMoving = true;
            }
        }

        IsMoving = false;
        if (Math.Abs(moveVec.x) >= 1 || Math.Abs(moveVec.y) >= 1)
        {
            OnMoveOver?.Invoke();
        }
        
    }

    public void SetPostitionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }



    private bool[] IsWalkable(Vector3 targetPos, Vector3 dir)
    {
        targetPos.x = Mathf.Floor(targetPos.x)+0.5f;
        targetPos.y = Mathf.Floor(targetPos.y - OffsetY) + 0.5f;
        Vector3 bCorner = (dir.x != 0) ? new Vector3(0, animator.GetHeight() - 2, 0) : new Vector3(animator.GetWidth() - 1, 0, 0);
        Collider2D[] collisions = Physics2D.OverlapAreaAll(targetPos, targetPos+bCorner, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer);
        
        var playerCollide = false;
        var isNotSelf = false;
        if (collisions != null)
            foreach (Collider2D collision in collisions)
            {
                var collisionIsNotSelf = collision.GetComponent<Character>() != this;
                isNotSelf = (isNotSelf) || collisionIsNotSelf;
                playerCollide = (collisionIsNotSelf) ? GameLayers.i.PlayerLayer == (GameLayers.i.PlayerLayer | (1 << collision.gameObject.layer)): playerCollide;
            }

        //var playerCollide = (collision != null) ? GameLayers.i.PlayerLayer == (GameLayers.i.PlayerLayer | (1 << collision.gameObject.layer)) : false;
        //var isNotSelf = (collision != null) ? collision.GetComponent<Character>() != this : false;
        return new bool[] { !(collisions != null && isNotSelf && !playerCollide), playerCollide && isNotSelf };
    }

    public IEnumerator SpecialAnimate(string animationName, string nextAnim = "idleDownAnim", Action OnAnimationFinished = null)
    {
        return animator.SpecialAnimate(animationName, nextAnim, OnAnimationFinished);
    }

    public IEnumerator ChangeState(string nextAnim = "idleDownAnim", Action OnAnimationFinished = null)
    {
        return animator.ChangeState(nextAnim, OnAnimationFinished);
    }
    public IEnumerator ChangeState(int index, string nextAnim = "idleDownAnim", Action OnAnimationFinished = null)
    {
        return animator.ChangeState(index, nextAnim, OnAnimationFinished);
    }

    public IEnumerator Wait(float duration, Action OnAnimationFinished = null)
    {
        return animator.Wait(duration, OnAnimationFinished);
    }

    public static float AdjustInput(float input)
    {
        if (input > .35f)
        {
            return Mathf.Ceil(input);
        }
        else if (input < -.35f)
        {
            return Mathf.Floor(input);
        }
        else
        {
            return 0;
        }
    }
}
