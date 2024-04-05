using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Gesture", menuName = "Gesture")]
public class Gesture : ScriptableObject
{
    public string gestureName;
    public List<JointData> jointDatas = new List<JointData>();
}
