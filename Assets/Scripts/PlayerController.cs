using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISavable
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;

    private Gamepad gamepad;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;
    public CharacterArt art;

    private void Awake()
    {
        gamepad = Gamepad.current;
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    public void HandleUpdate()
    {
        if (gamepad == null)
        {
            gamepad = Gamepad.current;
        }
        if (!isMoving)
        {
            input = gamepad.leftStick.ReadValue();
            if (Math.Abs(input.x)> Math.Abs(input.y))
            {
                input.y = 0;
            } else
            {
                input.x = 0;
            }
            
            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += adjustInput(input.x);
                targetPos.y += adjustInput(input.y);

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z) || gamepad.buttonSouth.wasPressedThisFrame)
            Interact();
    }


    private void FixedUpdate()
    {
        
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactablesLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactablesLayer) != null)
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
        } else if (input < -.35f)
        {
            return -1f;
        }
        else
        {
            return 0;
        }
    }

    public object CaptureState()
    {
        throw new System.NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }
}


