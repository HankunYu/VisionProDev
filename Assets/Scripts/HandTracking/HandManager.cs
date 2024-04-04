using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandManager : MonoBehaviour
{
    private XRHandSubsystem _handSubsystem;
    public static HandManager Instance;
    [ReadOnly] public bool isEnable;
    [HideInInspector]
    public List<JointData> leftHandJoints = new List<JointData>();
    [HideInInspector]
    public List<JointData> rightHandJoints = new List<JointData>();
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
    }


    private void Update()
    {
        if (!CheckHandSubsystem()) return;
        
        var updateSuccessFlags = _handSubsystem.TryUpdateHands(XRHandSubsystem.UpdateType.Dynamic);
        
        if((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose) != 0)
        {
            UpdateJoints();
        }
    }
    private void InitialJoints()
    {
        leftHandJoints.Clear();
        rightHandJoints.Clear();
        for (var jointID = 1; jointID < Enum.GetValues(typeof(XRHandJointID)).Length; jointID++)
        {
            var leftJoint = new JointData()
            {
                name = Enum.GetName(typeof(XRHandJointID), jointID),
                position = Vector3.zero,
                rotation = Quaternion.identity,
                isTracked = false
            };
            var rightJoint = new JointData()
            {
                name = Enum.GetName(typeof(XRHandJointID), jointID),
                position = Vector3.zero,
                rotation = Quaternion.identity,
                isTracked = false
            };
            leftHandJoints.Add(leftJoint);
            rightHandJoints.Add(rightJoint);
        }
    }
    private void UpdateJoints()
    {
        for (var jointID = 1; jointID < Enum.GetValues(typeof(XRHandJointID)).Length; jointID++)
        {
            var rightJoint = _handSubsystem.rightHand.GetJoint((XRHandJointID)jointID);
            var leftJoint = _handSubsystem.leftHand.GetJoint((XRHandJointID)jointID);
            var rightIsTracked = rightJoint.TryGetPose(out var rightPose);
            var leftIsTracked = leftJoint.TryGetPose(out var leftPose);
            
            var rightJointData = new JointData
            {
                name = Enum.GetName(typeof(XRHandJointID), jointID),
                position = rightIsTracked ? rightPose.position : Vector3.zero,
                rotation = rightIsTracked ? rightPose.rotation : Quaternion.identity,
                isTracked = rightIsTracked
            };
            rightHandJoints[jointID - 1] = rightJointData;
            
            var leftJointData = new JointData
            {
                name = Enum.GetName(typeof(XRHandJointID), jointID),
                position = leftIsTracked ? leftPose.position : Vector3.zero,
                rotation = leftIsTracked ? leftPose.rotation : Quaternion.identity,
                isTracked = leftIsTracked
            };
            leftHandJoints[jointID - 1] = leftJointData;
        }
    }
    public bool CheckHandSubsystem()
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
public struct JointData
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public bool isTracked;
}