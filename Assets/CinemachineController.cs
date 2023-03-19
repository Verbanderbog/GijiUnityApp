using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    public static CinemachineController i { get; private set; }
    CinemachineVirtualCamera cineCam;
    CinemachineFramingTransposer body;
    void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        cineCam = GetComponent<CinemachineVirtualCamera>();
        body = cineCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        
    }

    public void MoveCamera(Character target, Vector3 offset, float ortho, float dutch, float duration)
    {
        cineCam.Follow = target.transform;
        body.m_TrackedObjectOffset = offset;
        cineCam.m_Lens.OrthographicSize = ortho;
        cineCam.m_Lens.Dutch = dutch;
    }
 }
