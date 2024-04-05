using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FingerCalculate : MonoBehaviour
{
    public static float CalculateFingerBaseCurl(JointData metacarpal, JointData proximal, JointData intermediate)
    {
        var v0 = (proximal.position - metacarpal.position).normalized;
        var v1 = (intermediate.position - proximal.position).normalized;
        var degree = Vector3.Angle(v0, v1);
        var result = Mathf.InverseLerp(15f,80f,degree);
        result = Mathf.Lerp(0f,1f,result);
        if (math.dot(math.mul(metacarpal.rotation,Vector3.forward), math.cross(-v0, v1)) < 0f)
            result *= -1f;
        return result;
    }

    public static Quaternion Test()
    {
        var metacarpal = HandManager.Instance.leftHandJoints[6];
        var proximal = HandManager.Instance.leftHandJoints[7];
        var intermediate = HandManager.Instance.leftHandJoints[8];
        var v0 = math.mul(proximal.rotation, Vector3.forward);
        var rot = Quaternion.FromToRotation(proximal.position, Vector3.forward);
        return rot;
    }
    
    public static float CalculateFingerTipCurl(JointData proximal, JointData intermediate, JointData distal, JointData tip)
    {
        var v0 = (intermediate.position - proximal.position).normalized;
        var v1 = (distal.position - intermediate.position).normalized;
        var v2 = (tip.position - distal.position).normalized;
        var right = new Vector3(0, 0, 1f);
        
        var degree = Vector3.Angle(v0, v1);
        var degree1 = Vector3.Angle(v1, v2);
        
        var result = Mathf.InverseLerp(15f,80f,degree);
        result = Mathf.Lerp(0f,1f,result);
        
        var result1 = Mathf.InverseLerp(15f,80f,degree1);
        result1 = Mathf.Lerp(0f,1f,result1);
        
        if (math.dot(math.mul(intermediate.rotation, right), math.cross(-v0, v1)) < 0f) result *= -1f;
        if (math.dot(math.mul(distal.rotation, right), math.cross(v1, v2)) < 0f) result1 *= -1f;
        return 0.5f * (result + result1);
    }

    public static float CalculateFingerFullCurl(float baseCurl, float tipCurl)
    {
        return (baseCurl + tipCurl) / 2;
    }
}
