using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] List<CutsceneScript> dialogs;
    [SerializeField] List<TextAsset> dialogFiles;
    public int dialogIndex { get { return GameController.i.GetFlag(name+"DialogIndex"); } set { GameController.i.SetFlag(name + "DialogIndex", value); } }
    [SerializeField] List<MovementPath> paths;
    int currentPath { get { return GameController.i.GetFlag(name + "CurrentPath"); } set { GameController.i.SetFlag(name + "CurrentPath", value); } }
    [SerializeField] NPCType type;

    bool inCutscene;
    float idleTimer = 0f;

    PathAction currentPathAction;
    bool actionStarted = false;

    Character character;
    private enum NPCType { Talking, Inspectable, Knockable, Silent}

    public void Interact(PlayerController player)
    {
        if (type == NPCType.Silent)
            return;
        inCutscene = true;
        StartCoroutine(CutsceneManager.i.StartCutscene(dialogs[dialogIndex], OnCutsceneFinished));
    }
    private void OnCutsceneFinished()
    {
        inCutscene = false;
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
        if (paths.Count == 0)
        {
            character.HandleUpdate();
            return;
        }
        if (currentPath < 0 || (inCutscene && paths[currentPath].blockedByDialog) || GameController.i.State==(GameState.Menu | GameState.SceneSwitch))
            return;
        if (paths[currentPath].hasNext() && currentPathAction == null)
        {
            currentPathAction = paths[currentPath].next();
        }
        if (currentPathAction.state == NPCState.Idle)
        {
            if (!actionStarted)
            {
                
                if (currentPathAction.useStartPos)
                {
                    character.SetPostitionAndSnapToTile(currentPathAction.startPos);
                }
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
                if (currentPathAction.useStartPos)
                {
                    character.SetPostitionAndSnapToTile(currentPathAction.startPos);
                }
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


    string Interactable.GetContextName()
    {
        return type switch
        {
            NPCType.Talking => "Talk",
            NPCType.Inspectable => "Inspect",
            NPCType.Knockable => "Knock",
            _ => "",
        };
    }
}

public enum NPCState { Idle, Walking, Sitting }
