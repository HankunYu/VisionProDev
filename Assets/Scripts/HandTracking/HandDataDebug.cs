using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR.Hands;

public class HandDataDebug : HandManager
{
    public List<JointDataDebug> leftHandJointsDebug = new List<JointDataDebug>();
    public GameObject leftHand;
    
    [Button]
    private void InitialInspector()
    {
        leftHandJointsDebug.Clear();
        for (var jointID = 2; jointID < Enum.GetValues(typeof(XRHandJointID)).Length; jointID++)
        {
            var jointName = "L_" + (XRHandJointID) jointID;
            if (!GameObject.Find(jointName)) jointName = "L_Wrist";
            var jointTransform = GameObject.Find(jointName).transform;
            var joint = new JointDataDebug
            {
                id = (XRHandJointID)jointID,
                jointTransform = jointTransform
            };
            leftHandJointsDebug.Add(joint);
        }
    }

    protected override bool CheckHandSubsystem()
    {
        return true;
    }

    protected override void Update()
    {
        UpdateJointsData();
        UpdateFingerData();
    }
    protected override void UpdateJointsData()
    {
        for (var jointID = 0; jointID < leftHandJoints.Count; jointID++)
        {
            leftHandJoints[jointID].position = leftHandJointsDebug[jointID].jointTransform.position;
            leftHandJoints[jointID].rotation = leftHandJointsDebug[jointID].jointTransform.rotation;
            leftHandJoints[jointID].isTracked = true;
        }
    }
}

[Serializable]
public class JointDataDebug
{
    public XRHandJointID id;
    public Transform jointTransform;
}