using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    CharacterAnimator animator;
    AudioSource audioSource;
    [SerializeField] FootstepSounds footsteps;
    public float moveSpeed;
    public float OffsetY { get; private set; } = 0.3f;
    public CharacterAnimator Animator { get => animator; }
    public bool IsMoving { get; private set; }
    public bool OnIce { get; private set; }

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        audioSource = GetComponent<AudioSource>();
        SetPostitionAndSnapToTile(transform.position);
    }
    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
        animator.OnIce = OnIce;
        if (IsMoving && !audioSource.isPlaying && audioSource.clip != null)
            audioSource.Play();
        else if (!IsMoving && audioSource.isPlaying)
            audioSource.Pause();

    }
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {

        if (animator == null)
            animator = GetComponent<CharacterAnimator>();
        animator.MoveX = moveVec.x;
        animator.MoveY = moveVec.y;

        moveVec.x = adjustInput(moveVec.x);
        moveVec.y = adjustInput(moveVec.y);

        var targetPos = transform.position;

        var distance = Convert.ToInt32(moveVec.magnitude);
        Vector3 dir = moveVec.normalized;
        var walkable = IsWalkable(transform.position + dir);
        if (!walkable[0])
        {
            yield break;
        }
        while (walkable[1])
        {
            yield return null;
            walkable = IsWalkable(transform.position + dir);
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
                if (OnIce)
                {
                    transform.position = prevPos;
                    breakWalking = true;
                }
                OnIce = false;
            }
            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                walkable = IsWalkable(targetPos);
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
                    walkable = IsWalkable(targetPos);
                    IsMoving = true;

                }
                yield return null;
            }
            if (breakWalking)
                break;
            transform.position = targetPos;
            walkable = IsWalkable(transform.position + dir);
            if (!walkable[0])
            {
                break;
            }
            while (walkable[1])
            {
                IsMoving = false;
                yield return null;
                walkable = IsWalkable(transform.position + dir);
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

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;
        return !Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer);
    }
    private bool[] IsWalkable(Vector3 targetPos)
    {
        targetPos.x = Mathf.Floor(targetPos.x) + 0.5f;
        targetPos.y = Mathf.Floor(targetPos.y) + 0.5f;
        var collisions = Physics2D.OverlapCircleAll(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer);
        var playerCollide = false;
        var isNotSelf = false;
        if (collisions != null)
            foreach (Collider2D collision in collisions)
            {
                var collisionIsNotSelf = collision.GetComponent<Character>() != this;
                isNotSelf = (isNotSelf) ? true : collisionIsNotSelf;
                playerCollide = (collisionIsNotSelf) ? GameLayers.i.PlayerLayer == (GameLayers.i.PlayerLayer | (1 << collision.gameObject.layer)): playerCollide;
            }

        //var playerCollide = (collision != null) ? GameLayers.i.PlayerLayer == (GameLayers.i.PlayerLayer | (1 << collision.gameObject.layer)) : false;
        //var isNotSelf = (collision != null) ? collision.GetComponent<Character>() != this : false;
        return new bool[] { !(collisions != null && isNotSelf && !playerCollide), playerCollide && isNotSelf };
    }



    private float adjustInput(float input)
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
