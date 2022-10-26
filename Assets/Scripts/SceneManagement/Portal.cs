using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DestinationIdentifier { A, B, C, D, E, F, G, H, I, J, K, L }

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier portalID;
    [SerializeField] DestinationIdentifier targetPortal;
    [SerializeField] bool offsetSpawn = false;
    [SerializeField] Transform spawnPoint;
    PlayerController player;

    public Transform SpawnPoint => spawnPoint;

    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        GameController.Instance.SceneState(true);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        var destPortal = FindObjectsOfType<Portal>().First(x => x!= this && this.targetPortal == x.portalID);
        var pos = (offsetSpawn) ? destPortal.SpawnPoint.position + (player.transform.position - this.transform.position) : destPortal.SpawnPoint.position;
        player.Character.SetPostitionAndSnapToTile(pos);
        GameController.Instance.SceneState(false);
        Destroy(gameObject);
    }


}
