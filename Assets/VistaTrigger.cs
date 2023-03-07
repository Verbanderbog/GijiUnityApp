using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VistaTrigger : MonoBehaviour, IPlayerTriggerable
{
    private PlayerController player;
    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer body;
    [SerializeField] private Transform skybox;
    [SerializeField] private bool reset = false;
    void Start()
    {
        cam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        body = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        if (reset)
        {
            
            cam.Follow = player.transform;
            body.m_TrackedObjectOffset = new Vector3(0, 0, 0);
            body.m_YDamping = 0f;
            
        }
        else
        {
            body.m_YDamping = 10;
            body.m_TrackedObjectOffset = new Vector3(0, -0.5f, 0);
            cam.Follow = skybox;
        }
        
        
        yield return null;
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
