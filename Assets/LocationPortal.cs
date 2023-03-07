using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier portalID;
    [SerializeField] DestinationIdentifier targetPortal;
    public bool offsetSpawn = false;
    [SerializeField] Transform spawnPoint;
    private CinemachineConfiner2D cam;
    PlayerController player;

    public Transform SpawnPoint => spawnPoint;

    void Start()
    {
        cam = GameObject.Find("CM vcam1").GetComponent<CinemachineConfiner2D>();
    }
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchLocation());
    }

    IEnumerator SwitchLocation()
    {
        
        GameController.i.SceneState(true);
        yield return GameController.i.BlackScreen.FadeIn(0.5f);
        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && this.targetPortal == x.portalID);
        var pos = (destPortal.offsetSpawn) ? destPortal.transform.position + (player.transform.position - this.transform.position) + new Vector3(0,-1,0) : destPortal.transform.position;
        cam.m_Damping = 0f;
        player.Character.SetPostitionAndSnapToTile(pos);
        pos = destPortal.SpawnPoint.position - destPortal.transform.position;
        StartCoroutine(player.Character.Move(pos));
        yield return GameController.i.BlackScreen.FadeOut(0.5f);
        cam.m_Damping = 1.2f;
        GameController.i.SceneState(false);
     }
#if UNITY_EDITOR
    private void OnValidate()
    {
        var pos = transform.position;
        var collider = GetComponent<BoxCollider2D>();

        pos.x = Mathf.Floor(pos.x);
        if (collider.size.x % 2 == 1)
            pos.x += 0.5f;
        pos.y = Mathf.Floor(pos.y);
        if (collider.size.y % 2 == 1)
            pos.y += 0.5f;

        transform.position = pos;
    }
#endif
}

