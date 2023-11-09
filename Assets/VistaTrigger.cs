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
    private bool resetting = false;
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


            body.m_TrackedObjectOffset = Vector3.zero;
            resetting = true;


        }
        else
        {
            resetting = false;
            body.m_YDamping = 10;
            body.m_TrackedObjectOffset = new Vector3(0, 4f, 0);
        }
        
        
        yield return null;
    }

    private void Update()
    {
        if (resetting)
        {
            body.m_YDamping = (body.m_YDamping -(10 * Time.deltaTime) <= 0) ? 0 : body.m_YDamping - (10*Time.deltaTime);
            if (body.m_YDamping == 0)
            {
                resetting = false;
            }
        }
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
