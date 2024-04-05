using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands;

public class TestInfo : MonoBehaviour
{
    public TextMeshProUGUI text;
    private GameObject _joint0, _joint1, _joint2;
    private void Start()
    {
        text.text = "This is a test info";
    }

    private void Update()
    {
        if (!_joint0 || !_joint1 || !_joint2)
        {
            _joint0 = GameObject.Find("IndexMetacarpal");
            _joint1 = GameObject.Find("IndexProximal");
            _joint2 = GameObject.Find("IndexIntermediate");
        }
        var jointData0 = new JointData()
        {
            id = XRHandJointID.IndexMetacarpal,
            position = _joint0.transform.position,
            rotation = _joint0.transform.rotation,
            isTracked = true
        };
        var jointData1 = new JointData()
        {
            id = XRHandJointID.IndexProximal,
            position = _joint1.transform.position,
            rotation = _joint1.transform.rotation,
            isTracked = true
        };
        var jointData2 = new JointData()
        {
            id = XRHandJointID.IndexIntermediate,
            position = _joint2.transform.position,
            rotation = _joint2.transform.rotation,
            isTracked = true
        };
        
        var result = FingerCalculate.CalculateFingerBaseCurl(jointData0, jointData1, jointData2);
        text.text = result.ToString("F");
    }
}
