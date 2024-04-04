using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

[RequireComponent(typeof(HandManager))]
public class HandVisualization : MonoBehaviour
{
    public GameObject jointPrefab;
    private bool _isInitialized;
    private List<GameObject> _leftHandJoints = new List<GameObject>();
    private List<GameObject> _rightHandJoints = new List<GameObject>();

    private void Update()
    {
        Initialize();
        UpdateJointTransform(); 
    }
    
    private void Initialize()
    {
        if (!HandManager.Instance.isEnable) return;
        if (_isInitialized) return;
        foreach (var joint in HandManager.Instance.leftHandJoints)
        {
            var jointObject = Instantiate(jointPrefab, joint.position, joint.rotation);
            jointObject.name = joint.name;
            _leftHandJoints.Add(jointObject);
        }
        foreach (var joint in HandManager.Instance.rightHandJoints)
        {
            var jointObject = Instantiate(jointPrefab, joint.position, joint.rotation);
            jointObject.name = joint.name;
            _rightHandJoints.Add(jointObject);
        }
        _isInitialized = true;
    }

    private void UpdateJointTransform()
    {
        foreach (var joint in _leftHandJoints)
        {
            joint.transform.position = HandManager.Instance.leftHandJoints.Find(x => x.name == joint.name).position;
            joint.transform.rotation = HandManager.Instance.leftHandJoints.Find(x => x.name == joint.name).rotation;
            joint.SetActive(HandManager.Instance.leftHandJoints.Find(x => x.name == joint.name).isTracked);
        }
        foreach (var joint in _rightHandJoints)
        {
            joint.transform.position = HandManager.Instance.rightHandJoints.Find(x => x.name == joint.name).position;
            joint.transform.rotation = HandManager.Instance.rightHandJoints.Find(x => x.name == joint.name).rotation;
            joint.SetActive(HandManager.Instance.rightHandJoints.Find(x => x.name == joint.name).isTracked);
        }
    }
    
}
