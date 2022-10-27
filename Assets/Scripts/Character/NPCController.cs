using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] List<Dialog> dialogs;
    [SerializeField] List<TextAsset> dialogFiles;
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
        StartCoroutine(DialogManager.Instance.ShowDialog(dialogs[dialogIndex], OnDialogFinished));
    }
    private void OnDialogFinished()
    {
        inDialog = false;
    }
    
    void Awake()
    {
        character = GetComponent<Character>();
        foreach (TextAsset t in dialogFiles)
        {
            var dialogStart = false;
            string[] lines = t.text.Split('\n');
            foreach (string s in lines)
            {
                if (s.Equals("//DIALOG START//"))
                    dialogStart = true;
                if (dialogStart)
                {

                }
                else
                {
                    //CharacterDialogArtManager.Instance.Participants;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath < 0 || (inDialog && paths[currentPath].blockedByDialog) || GameController.Instance.State==(GameState.Menu | GameState.SceneSwitch))
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
