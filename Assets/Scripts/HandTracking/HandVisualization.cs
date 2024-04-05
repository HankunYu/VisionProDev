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
    private List<JointPrefab> _leftJointPrefabs = new List<JointPrefab>();
    private List<JointPrefab> _rightJointPrefabs = new List<JointPrefab>();

    private void Update()
    {
        Initialize();
        UpdateJointTransform(); 
    }
    
    private void Initialize()
    {
        if (!HandManager.Instance.isEnable) return;
        if (_isInitialized) return;
        var parent = new GameObject("HandJoints");
        foreach (var joint in HandManager.Instance.leftHandJoints)
        {
            var jointObject = Instantiate(jointPrefab, joint.position, joint.rotation);
            jointObject.name = joint.id.ToString();
            _leftHandJoints.Add(jointObject);
            jointObject.transform.SetParent(parent.transform);
            _leftJointPrefabs.Add(jointObject.GetComponent<JointPrefab>());
        }
        foreach (var joint in HandManager.Instance.rightHandJoints)
        {
            var jointObject = Instantiate(jointPrefab, joint.position, joint.rotation);
            jointObject.name = joint.id.ToString();
            _rightHandJoints.Add(jointObject);
            jointObject.transform.SetParent(parent.transform);
            _rightJointPrefabs.Add(jointObject.GetComponent<JointPrefab>());
        }
        _isInitialized = true;
    }

    private void UpdateJointTransform()
    {
        for(var i = 0; i < _leftHandJoints.Count; i++)
        {
            _leftHandJoints[i].transform.position = HandManager.Instance.leftHandJoints.Find(x => x.id.ToString() == _leftHandJoints[i].name).position;
            _leftHandJoints[i].transform.rotation = HandManager.Instance.leftHandJoints.Find(x => x.id.ToString() == _leftHandJoints[i].name).rotation;
            _leftJointPrefabs[i].ChangeMaterial(HandManager.Instance.leftHandJoints[i].isTracked);
        }
        for(var i = 0; i < _rightHandJoints.Count; i++)
        {
            _rightHandJoints[i].transform.position = HandManager.Instance.rightHandJoints.Find(x => x.id.ToString() == _rightHandJoints[i].name).position;
            _rightHandJoints[i].transform.rotation = HandManager.Instance.rightHandJoints.Find(x => x.id.ToString() == _rightHandJoints[i].name).rotation;
            _rightJointPrefabs[i].ChangeMaterial(HandManager.Instance.rightHandJoints[i].isTracked);
        }
    }
    
}
