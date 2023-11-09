using UnityEngine;
public enum CutsceneEventType { StateChange, Movement, SpecialAnim, Wait, Camera, FlagChange }
[System.Serializable]
public class CutsceneEvent
{
    [SerializeField] public string character;
    [SerializeField] public CutsceneEventType cutsceneType;
    [SerializeField] public bool blocksAnims;
    [SerializeField] public bool blocksDialog;
    [SerializeField] public int stateIndex;
    [SerializeField] public bool incrementStateFlag = true;
    [SerializeField] public string animName;
    [SerializeField] public string nextAnimName;
    [SerializeField] public Vector3 move;
    [SerializeField] public float rotate;
    [SerializeField] public float zoom;
    [SerializeField] public float duration;
    [SerializeField] public string flagKey;
    [SerializeField] public int flagValue;



}