using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.XR.Hands;

public class HandPoseCopy : MonoBehaviour
{
    public List<JointDataDebug> HandJointsDebug = new List<JointDataDebug>();
    public HandType handType;
    public bool copyRotation;
    public bool copyPosition;
    public GameObject jointPrefab;
    public bool showJoints;
    
    private List<GameObject> jointObjs = new List<GameObject>();
    
    [Button]
    private void InitialInspector()
    {
        HandJointsDebug.Clear();
        foreach (var obj in jointObjs)
        {
            DestroyImmediate(obj);
        }
        jointObjs.Clear();
        for (var jointID = 1; jointID < Enum.GetValues(typeof(XRHandJointID)).Length; jointID++)
        {
            var jointName = (handType == HandType.Left ? "L_" : "R_") + (XRHandJointID) jointID;
            if (!GameObject.Find(jointName)) continue; //jointName = "L_Wrist";
            var jointTransform = GameObject.Find(jointName).transform;
            var joint = new JointDataDebug
            {
                id = (XRHandJointID)jointID,
                jointTransform = jointTransform
            };
            if (showJoints)
            {
                var jointObj = Instantiate(jointPrefab, jointTransform.position, jointTransform.rotation);
                jointObj.name = joint.id.ToString();
                jointObj.transform.parent = jointTransform;
                jointObjs.Add(jointObj);
            }

            HandJointsDebug.Add(joint);
        }
    }

    private void Update()
    {
        for(int i = 0; i < HandJointsDebug.Count; i++)
        {
            if(copyPosition)
                HandJointsDebug[i].jointTransform.position = 
                    handType == HandType.Left ? HandManager.Instance.leftHandJoints[i].position 
                        : HandManager.Instance.rightHandJoints[i].position;
            if(copyRotation)
                HandJointsDebug[i].jointTransform.rotation = 
                    handType == HandType.Left ? HandManager.Instance.leftHandJoints[i].rotation 
                        : HandManager.Instance.rightHandJoints[i].rotation;
        }
    }
}
