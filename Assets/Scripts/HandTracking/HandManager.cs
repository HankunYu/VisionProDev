using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandManager : MonoBehaviour
{
    private XRHandSubsystem _handSubsystem;
    public static HandManager Instance;
    
    // Hand Tracking Data
    [ReadOnly] public bool isEnable;
    [HideInInspector]
    [ReadOnly] public List<JointData> leftHandJoints = new List<JointData>();
    [HideInInspector]
    [ReadOnly] public List<JointData> rightHandJoints = new List<JointData>();
    [HideInInspector]
    [ReadOnly] public List<FingerData> leftHandFingers = new List<FingerData>();
    [HideInInspector]
    [ReadOnly] public List<FingerData> rightHandFingers = new List<FingerData>();
    
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        GetHandSubsystem();
        InitialJoints();
        InitialFingers();
    }
    
    protected virtual void Update()
    {
        if (!CheckHandSubsystem()) return;
        
        var updateSuccessFlags = _handSubsystem.TryUpdateHands(XRHandSubsystem.UpdateType.Dynamic);
        
        if((updateSuccessFlags & (XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose 
                               | XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose)) != 0)
        {
            UpdateJointsData();
            UpdateFingerData();
        }
    }

    #region Initial Data
    protected virtual void InitialJoints()
    {
        leftHandJoints.Clear();
        rightHandJoints.Clear();
        for (var jointID = 1; jointID < Enum.GetValues(typeof(XRHandJointID)).Length; jointID++)
        {
            var leftJoint = new JointData()
            {
                id = (XRHandJointID)jointID,
                position = Vector3.zero,
                rotation = Quaternion.identity,
                isTracked = false
            };
            var rightJoint = new JointData()
            {
                id = (XRHandJointID)jointID,
                position = Vector3.zero,
                rotation = Quaternion.identity,
                isTracked = false
            };
            leftHandJoints.Add(leftJoint);
            rightHandJoints.Add(rightJoint);
        }
    }

    protected virtual void InitialFingers()
    {
        foreach(var finger in Enum.GetValues(typeof(XRHandFingerID)))
        {
            var leftFinger = new FingerData()
            {
                fingerType = (XRHandFingerID)finger,
                baseCurl = 0,
                tipCurl = 0,
                fullCurl = 0
            };
            var rightFinger = new FingerData()
            {
                fingerType = (XRHandFingerID)finger,
                baseCurl = 0,
                tipCurl = 0,
                fullCurl = 0
            };
            leftHandFingers.Add(leftFinger);
            rightHandFingers.Add(rightFinger);
        }
    }
    #endregion
    protected virtual void UpdateJointsData()
    {
        // Joint ID 0 is not used
        for (var jointID = 1; jointID < Enum.GetValues(typeof(XRHandJointID)).Length; jointID++)
        {
            var rightJoint = _handSubsystem.rightHand.GetJoint((XRHandJointID)jointID);
            var leftJoint = _handSubsystem.leftHand.GetJoint((XRHandJointID)jointID);
            var rightIsTracked = rightJoint.TryGetPose(out var rightPose);
            var leftIsTracked = leftJoint.TryGetPose(out var leftPose);
            
            var leftJointData = new JointData
            {
                id = (XRHandJointID)jointID,
                position = leftIsTracked ? leftPose.position : leftHandJoints[jointID - 1].position,
                rotation = leftIsTracked ? leftPose.rotation : leftHandJoints[jointID - 1].rotation,
                isTracked = leftIsTracked
            };
            leftHandJoints[jointID - 1] = leftJointData;
            
            var rightJointData = new JointData
            {
                id = (XRHandJointID)jointID,
                position = rightIsTracked ? rightPose.position : rightHandJoints[jointID - 1].position,
                rotation = rightIsTracked ? rightPose.rotation : rightHandJoints[jointID - 1].rotation,
                isTracked = rightIsTracked
            };
            rightHandJoints[jointID - 1] = rightJointData;
        }
    }

    protected virtual void UpdateFingerData()
    {
        // left hand fingers
        for(var i = 0; i < leftHandFingers.Count; i++)
        {
            // thumb's curl is calculated differently
            if (i == 0)
            {
                continue;
            }
            var baseCurl = FingerCalculate.CalculateFingerBaseCurl(leftHandJoints[5 * i + 1], 
                leftHandJoints[5 * i + 2], leftHandJoints[5 * i + 3]);
            var tipCurl = FingerCalculate.CalculateFingerTipCurl(leftHandJoints[5 * i + 2], 
                leftHandJoints[5 * i + 3], leftHandJoints[5 * i + 4], leftHandJoints[5 * i + 5]);
            var fullCurl = FingerCalculate.CalculateFingerFullCurl(baseCurl, tipCurl);
            leftHandFingers[i].baseCurl = baseCurl;
            leftHandFingers[i].tipCurl = tipCurl;
            leftHandFingers[i].fullCurl = fullCurl;
        }
        // right hand fingers
        for(var i = 0; i < rightHandFingers.Count; i++)
        {
            // thumb's curl is calculated differently
            if (i == 0)
            {
                continue;
            }
            var baseCurl = FingerCalculate.CalculateFingerBaseCurl(rightHandJoints[5 * i + 1], 
                rightHandJoints[5 * i + 2], rightHandJoints[5 * i + 3]);
            var tipCurl = FingerCalculate.CalculateFingerTipCurl(rightHandJoints[5 * i + 2], 
                rightHandJoints[5 * i + 3], rightHandJoints[5 * i + 4], rightHandJoints[5 * i + 5]);
            var fullCurl = FingerCalculate.CalculateFingerFullCurl(baseCurl, tipCurl);
            rightHandFingers[i].baseCurl = baseCurl;
            rightHandFingers[i].tipCurl = tipCurl;
            rightHandFingers[i].fullCurl = fullCurl;
        }
    }
    protected virtual bool CheckHandSubsystem()
    {
        if (_handSubsystem != null) return true;
        Debug.LogError("Hand Subsystem is missing.");
        isEnable = false;
        return false;
    }
    
    private void GetHandSubsystem()
    {
        var xrManager = XRGeneralSettings.Instance.Manager;
        if (xrManager == null)
        {
            Debug.LogError("XR Manager is missing.");
            return;
        }
        if(xrManager.activeLoader)
            _handSubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
        if (_handSubsystem == null)
        {
            Debug.LogError("Hand Subsystem is missing.");
        }

        _handSubsystem?.Start();
        isEnable = true;
    }
}

[Serializable]
public class JointData
{
    public XRHandJointID id;
    public Vector3 position;
    public Quaternion rotation;
    public bool isTracked;
}

[Serializable]
public class FingerData
{
    public XRHandFingerID fingerType;
    public float baseCurl;
    public float tipCurl;
    public float fullCurl;
}

