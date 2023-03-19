using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] GameObject dialogContainer;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Image dialogReaction;
    int textSpeed=50;

    Gamepad gamepad;


    public bool blockingAnims;
    public bool blockingDialog;

    public event Action OnStartCutscene;
    public event Action OnEndCutscene;

    //public bool IsShowing { get; private set; }
    Action onCutsceneFinished;
    CutsceneScript script;
    int currentLine = 0;
    int currentAnim = 0;
    bool isTyping = false;
    bool endTyping = false;
    List<Character> charactersAnimating = new();
    public static CutsceneManager i { get; private set; }
    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        gamepad = Gamepad.current;
    }
    


    public IEnumerator StartCutscene(CutsceneScript script, Action onCutsceneFinished=null)
    {
        UpdateTextSpeed();
        if (gamepad == null)
            gamepad = Gamepad.current;
        this.onCutsceneFinished = onCutsceneFinished;
        yield return new WaitForEndOfFrame();
        OnStartCutscene?.Invoke();
        //IsShowing = true;
        this.script = script;
        if (!String.IsNullOrEmpty(script.Lines[0].Line))
            dialogContainer.SetActive(true);
        else
            dialogContainer.SetActive(false);
        dialogReaction.sprite = (script.Lines[0].Reaction != Reaction.Empty && script.Lines[0].ParticipantIndex >=0) ? script.Participants[script.Lines[0].ParticipantIndex].images[(int)script.Lines[0].Reaction] : null;
        if (script.Lines[0].Reaction != Reaction.Empty && script.Lines[0].ParticipantIndex >= 0)
        {
            dialogReaction.enabled = true;
            dialogReaction.sprite = script.Participants[script.Lines[0].ParticipantIndex].images[(int)script.Lines[0].Reaction];
        }
        else
        {
            dialogReaction.enabled = false;
        }
        if (!String.IsNullOrEmpty(script.Lines[currentLine].Line))
        {
            string type = script.Participants[script.Lines[0].ParticipantIndex].Name;
            type = (type == null || type == "") ? script.Lines[0].Line : type + ": " + script.Lines[0].Line;
            StartCoroutine(TypeDialog(type));
        }

    }

    public void UpdateTextSpeed()
    {
        var temp = PlayerPrefs.GetInt("Text Speed");
        textSpeed = (temp != 0) ? temp : 50;
    }

    public IEnumerator TypeDialog(string line)
    {

        isTyping = true;
        dialogText.SetText("");
        foreach (var letter in line.ToCharArray())
        {
            if (endTyping)
            {
                endTyping = false;
                break;
            }
            dialogText.text += letter;
            float speedMultiplier = (gamepad.buttonSouth.isPressed) ? 2f: 1f;
            yield return new WaitForSeconds(1f / textSpeed * speedMultiplier);
        }
        isTyping = false;

    }

    public IEnumerator ChangeAvatar(Sprite image)
    {
        if (!dialogReaction.sprite.Equals(image))
        {
            if (dialogReaction.color.a == 1f)
                for (int i = 254; dialogReaction.color.a > 0f; i--)
                {
                    dialogReaction.color = new Color(1f, 1f, 1f, i / 255f);
                    yield return null;
                }
            
            dialogReaction.sprite = image;
            for (int i = 1; dialogReaction.color.a < 255f; i++)
            {
                dialogReaction.color = new Color(1f, 1f, 1f, i / 255f);
                yield return null;
            }
        }

    }

    public void HandleUpdate()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;
        if (gamepad.buttonEast.wasPressedThisFrame && isTyping)
        {
            endTyping = true;
            string type = script.Participants[script.Lines[currentLine].ParticipantIndex].Name;
            type = (type == null || type == "") ? script.Lines[currentLine].Line : type + ": " + script.Lines[currentLine].Line;
            dialogText.text = type;
        }
        if ((gamepad.buttonSouth.wasPressedThisFrame || gamepad.buttonEast.wasPressedThisFrame) && !isTyping)
        {
            ++currentLine;
            currentAnim = 0;
            if (currentLine < script.Lines.Count)
            {
                if (!String.IsNullOrEmpty(script.Lines[currentLine].Line))
                {
                    if (script.Lines[currentLine].Reaction == Reaction.Empty || script.Lines[currentLine].ParticipantIndex < 0)
                    {
                        dialogReaction.enabled = false;
                    }
                    else if (!script.Participants[script.Lines[currentLine].ParticipantIndex].images[(int)script.Lines[currentLine].Reaction].Equals(dialogReaction.sprite))
                    {
                        dialogReaction.enabled = true;
                        dialogReaction.sprite = script.Participants[script.Lines[currentLine].ParticipantIndex].images[(int)script.Lines[currentLine].Reaction];
                    }

                    string type = (script.Lines[currentLine].ParticipantIndex < 0) ? null : script.Participants[script.Lines[currentLine].ParticipantIndex].Name;
                    type = (type == null || type == "") ? script.Lines[currentLine].Line : type + ": " + script.Lines[currentLine].Line;
                    dialogContainer.SetActive(true);
                    StartCoroutine(TypeDialog(type));
                }
                else
                {
                    dialogContainer.SetActive(false);
                }
                
            }
            else
            {
                currentLine = 0;
                currentAnim = 0;
                //IsShowing = false;
                dialogContainer.SetActive(false);
                onCutsceneFinished?.Invoke();
                OnEndCutscene?.Invoke();
                return;
            }
        }
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        
        if (blockingAnims || script.Lines[currentLine].Anims.Count==0 )
            return;
        
        for (int i=currentAnim; i<script.Lines[currentLine].Anims.Count; i++)
        {
            if (GameController.i.characters.ContainsKey(script.Lines[currentLine].Anims[currentAnim].character) || (script.Lines[currentLine].Anims[currentAnim].character == "Player"))
            {

                Character character =(script.Lines[currentLine].Anims[currentAnim].character == "Player") ? GameController.i.playerController.Character : GameController.i.characters[script.Lines[currentLine].Anims[currentAnim].character];
                
                if (character.Animator.IsBusy || charactersAnimating.Contains(character))
                {
                    currentAnim = i;
                    break;
                }

                void OnAnimationFinished()
                {
                    Debug.Log("Anim finished");
                    charactersAnimating.Remove(character);
                    blockingAnims = false;
                }
                switch (script.Lines[currentLine].Anims[currentAnim].cutsceneType)
                {
                    case CutsceneAnimType.StateChange:
                        Debug.Log(character.name + " changing state ");
                        if (script.Lines[currentLine].Anims[currentAnim].incrementState)
                            StartCoroutine(character.ChangeState((!String.IsNullOrEmpty(script.Lines[currentLine].Anims[currentAnim].nextAnimName)) ? script.Lines[currentLine].Anims[currentAnim].nextAnimName : "idleDownAnim", OnAnimationFinished));
                        else
                            StartCoroutine(character.ChangeState(script.Lines[currentLine].Anims[currentAnim].stateIndex, (!String.IsNullOrEmpty(script.Lines[currentLine].Anims[currentAnim].nextAnimName)) ? script.Lines[currentLine].Anims[currentAnim].nextAnimName : "idleDownAnim", OnAnimationFinished));
                        charactersAnimating.Add(character);
                        break;
                    case CutsceneAnimType.Movement:
                        Debug.Log(character.name + " moving " + script.Lines[currentLine].Anims[currentAnim].move);
                        StartCoroutine(character.Move(script.Lines[currentLine].Anims[currentAnim].move,null, OnAnimationFinished));
                        charactersAnimating.Add(character);
                        break;
                    case CutsceneAnimType.SpecialAnim:
                        Debug.Log(character.name + " animating " + script.Lines[currentLine].Anims[currentAnim].animName);
                        StartCoroutine(character.SpecialAnimate(script.Lines[currentLine].Anims[currentAnim].animName, (!String.IsNullOrEmpty(script.Lines[currentLine].Anims[currentAnim].nextAnimName)) ? script.Lines[currentLine].Anims[currentAnim].nextAnimName : "idleDownAnim", OnAnimationFinished));
                        charactersAnimating.Add(character);
                        break;
                    case CutsceneAnimType.Wait:
                        Debug.Log(character.name + " waiting " + script.Lines[currentLine].Anims[currentAnim].duration);
                        StartCoroutine(character.Wait(script.Lines[currentLine].Anims[currentAnim].duration, OnAnimationFinished));
                        charactersAnimating.Add(character);
                        break;
                }
                blockingAnims = script.Lines[currentLine].Anims[currentAnim].blocksAnims || blockingAnims;
                currentAnim = i;

            }
            else
            {
                Debug.LogError("No such character: " + script.Lines[currentLine].Anims[currentAnim].character);
            }
            if (i + 1 >= script.Lines[currentLine].Anims.Count)
                currentAnim = i + 1;
        }
    }
}
