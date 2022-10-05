using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;
    public float moveSpeed;

    public Animator Animator { get => animator; }
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void HandleUpdate()
    {
        animator.SetBool("isMoving", IsMoving);
    }
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null)
    {
        animator.SetFloat("moveX", moveVec.x);
        animator.SetFloat("moveY", moveVec.y);

        var targetPos = transform.position;
        targetPos.x += adjustInput(moveVec.x);
        targetPos.y += adjustInput(moveVec.y);

        if (!IsWalkable(targetPos))
        {
            yield break;
        }
        IsMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            Debug.Log("Point Reached");
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        IsMoving = false;
        OnMoveOver?.Invoke();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    private float adjustInput(float input)
    {
        if (input > .35f)
        {
            return 1f;
        }
        else if (input < -.35f)
        {
            return -1f;
        }
        else
        {
            return 0;
        }
    }
}
