using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] List<Dialog> dialogs;
    [SerializeField] int dialogIndex;
    [SerializeField] List<MovementPath> paths;
    [SerializeField] int currentPath;

    bool inDialog;
    float idleTimer = 0f;

    PathAction currentPathAction;
    bool actionStarted = false;

    Character character;

    public void Interact()
    {
        inDialog = true;
        StartCoroutine(DialogManager.Instance.ShowDialog(dialogs[dialogIndex], onDialogFinished));
    }
    private void onDialogFinished()
    {
        inDialog = false;
    }
    
    void Awake()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath < 0 || (inDialog && paths[currentPath].blockedByDialog) )
            return;
        if (paths[currentPath].hasNext() && currentPathAction == null)
        {
            currentPathAction = paths[currentPath].next();
        }
        if (currentPathAction.state == NPCState.Idle)
        {
            if (!actionStarted)
            {
                actionStarted = true;
                StartCoroutine(character.Move(currentPathAction.GetVector()));
            }
                
            idleTimer += Time.deltaTime;
            if (idleTimer > currentPathAction.durationOrDistance)
            {
                idleTimer = 0f;
                if (paths[currentPath].hasNext())
                {
                    currentPathAction = paths[currentPath].next();
                    actionStarted = false;
                }
                    
            }
        }
        else if (currentPathAction.state == NPCState.Walking)
        {
            if (!actionStarted)
            {
                actionStarted = true;
                StartCoroutine(character.Move(currentPathAction.GetVector(), OnMoveOver));
            }
            
        }

        character.HandleUpdate();
    }

    private void OnMoveOver()
    {
        if (currentPathAction.state == NPCState.Walking && paths[currentPath].hasNext())
        {
            currentPathAction = paths[currentPath].next();
            actionStarted = false;
        }
    }

    public object CaptureState()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new NotImplementedException();
    }
}

public enum NPCState { Idle, Walking }
