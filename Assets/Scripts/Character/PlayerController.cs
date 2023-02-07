using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour, ISavable
{


    private Gamepad gamepad;

    private Vector2 input;

    IEnumerator movementRoutine;


    private Character character;

    public Character Character => character;

    private void Awake()
    {
        gamepad = Gamepad.current;
        character = GetComponent<Character>();
    }


    // Update is called once per frame
    public void HandleUpdate()
    {
        if (gamepad == null)
        {
            gamepad = Gamepad.current;
        }
        if (!character.IsMoving)
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
                movementRoutine = character.Move(input, OnMoveOver);
                StartCoroutine(movementRoutine);
            }
        }
        character.HandleUpdate();
        if (Input.GetKeyDown(KeyCode.Z) || gamepad.buttonSouth.wasPressedThisFrame)
            Interact();
    }


    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, character.OffsetY), 0.2f, GameLayers.i.TriggerableLayer);
        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position - new Vector3(0, character.OffsetY) + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    public object CaptureState()
    {
        return new PlayerSave(transform.position.x, transform.position.y, character.Animator.currentAnim);
    }

    public void RestoreState(object state)
    {
        PlayerSave save = (PlayerSave)state;
        transform.position = new Vector3(save.x, save.y);
        character.Animator.currentAnim = save.currentAnim;
    }
}

[System.Serializable]
struct PlayerSave
{
    public readonly float x;
    public readonly float y;
    public readonly string currentAnim;

    public PlayerSave(float x, float y, string currentAnim)
    {
        this.x = x;
        this.y = y;
        this.currentAnim = currentAnim;
    }

}


