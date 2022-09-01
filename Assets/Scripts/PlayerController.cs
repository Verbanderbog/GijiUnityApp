using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISavable
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;

    private bool isMoving;
    private Vector2 input;
    private bool lastMoveVert;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            if (lastMoveVert)
            {
                input.y = Input.GetAxisRaw("Vertical");
                if (input.y == 0)
                {
                    lastMoveVert = false;
                    input.x = Input.GetAxisRaw("Horizontal");
                }
                else
                {
                    lastMoveVert = true;
                    input.x = 0;
                }
            }
            else
            {
                input.x = Input.GetAxisRaw("Horizontal");
                if (input.x == 0)
                {
                    lastMoveVert = true;
                    input.y = Input.GetAxisRaw("Vertical");
                }
                else
                {
                    lastMoveVert = false;
                    input.y = 0;
                }
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

        if (Input.GetKeyDown(KeyCode.Z))
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
        if (input > .2f)
        {
            return 1f;
        } else if (input < -.2f)
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


