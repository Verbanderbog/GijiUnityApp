using UnityEngine;
public enum CutsceneAnimType { StateChange, Movement, SpecialAnim, Wait, Camera }
[System.Serializable]
public class CutsceneAnim
{
    [SerializeField] public string character;
    [SerializeField] public CutsceneAnimType cutsceneType;
    [SerializeField] public bool blocksAnims;
    [SerializeField] public bool blocksDialog;
    [SerializeField] public int stateIndex;
    [SerializeField] public bool incrementState = true;
    [SerializeField] public string animName;
    [SerializeField] public string nextAnimName;
    [SerializeField] public Vector3 move;
    [SerializeField] public float rotate;
    [SerializeField] public float zoom;
    [SerializeField] public float duration;




}