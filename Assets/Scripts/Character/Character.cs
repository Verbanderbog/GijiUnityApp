using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;
    public float moveSpeed;

    public CharacterAnimator Animator { get => animator; }
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }
    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {

        if (animator == null)
            animator = GetComponent<CharacterAnimator>();
        animator.MoveX = moveVec.x;
        animator.MoveY = moveVec.y;

        moveVec.x += adjustInput(moveVec.x);
        moveVec.y += adjustInput(moveVec.y);

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
        OnMoveOver?.Invoke();
    }
    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;
        return !Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer);
    }
    private bool[] IsWalkable(Vector3 targetPos)
    {
        var collision = Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer);
        var playerCollide = (collision != null)? GameLayers.i.PlayerLayer == (GameLayers.i.PlayerLayer | (1 << collision.gameObject.layer)) :false;
        var isNotSelf = (collision != null) ? collision.GetComponent<Character>() != this : false;
        return new bool[] { !(collision != null && isNotSelf && !playerCollide), playerCollide && isNotSelf};
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
