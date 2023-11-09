using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    public static CinemachineController i { get; private set; }
    public bool Move { get => move;  }

    CinemachineVirtualCamera cineCam;
    CinemachineFramingTransposer body;
    Vector3 startOffset;
    Vector3 endOffset;
    float startOrtho;
    float endOrtho;
    float startDutch;
    float endDutch; 
    float duration;
    float timeElapsed;
    bool move=false;
    event Action OnTransitionFinish = null;
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
        if (move)
        {
            body.m_TrackedObjectOffset = Vector3.MoveTowards(body.m_TrackedObjectOffset, endOffset, (Time.deltaTime/duration)*Vector3.Distance(startOffset,endOffset) );
            timeElapsed += Time.deltaTime;
            float deltaRatio = duration == 0 ? 1 : (Time.deltaTime / duration);
            if (deltaRatio > 1)
                deltaRatio = 1;
            if (deltaRatio < -1)
                deltaRatio = -1;
            var orthoDistance = deltaRatio * (endOrtho - startOrtho);
            var dutchDistance = deltaRatio * (endDutch - startDutch);

            if ((orthoDistance > 0 && cineCam.m_Lens.OrthographicSize + orthoDistance > endOrtho) || (orthoDistance < 0 && cineCam.m_Lens.OrthographicSize + orthoDistance < endOrtho))
                cineCam.m_Lens.OrthographicSize = endOrtho;
            else
                cineCam.m_Lens.OrthographicSize = cineCam.m_Lens.OrthographicSize + orthoDistance;
            if ((dutchDistance > 0 && cineCam.m_Lens.Dutch + dutchDistance > endDutch) || (dutchDistance < 0 && cineCam.m_Lens.Dutch + dutchDistance < endDutch))
                cineCam.m_Lens.Dutch = endDutch;
            else
                cineCam.m_Lens.Dutch = (cineCam.m_Lens.Dutch + dutchDistance);
            if (body.m_TrackedObjectOffset == endOffset && cineCam.m_Lens.OrthographicSize == endOrtho && cineCam.m_Lens.Dutch == endDutch && timeElapsed > duration)
            {
                move = false;
                OnTransitionFinish?.Invoke();

            }
            
        }
        
    }

    public void MoveCamera(Character target, Vector3 offset, float ortho, float dutch, float duration, Action OnTransitionFinish = null)
    {
        cineCam.Follow = target.transform;
        endOffset = offset;
        startOffset = body.m_TrackedObjectOffset;
        startOrtho = cineCam.m_Lens.OrthographicSize;
        startDutch = cineCam.m_Lens.Dutch;
        endOrtho = ortho;
        endDutch = dutch;
        this.duration = duration >= 0 ? duration : 0.0001f;
        this.OnTransitionFinish = OnTransitionFinish;
        timeElapsed = 0;
        move = true;
    }
 }
