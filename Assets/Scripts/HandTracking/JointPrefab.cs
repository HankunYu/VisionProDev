using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class JointPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jointName;
    [SerializeField] private TextMeshProUGUI jointPosition;
    [SerializeField] private TextMeshProUGUI jointRotation;
    [SerializeField] private MeshRenderer meshRenderer;
    private void Start()
    {
        jointName.text = name;
    }
    private void Update()
    {
        jointPosition.text = transform.position.ToString("F2");
        jointRotation.text = transform.rotation.eulerAngles.ToString("F2");
    }
    public void ChangeMaterial(bool isTracked)
    {
        meshRenderer.material.color = isTracked ? Color.white : Color.red;
    }
    [Button]
    private void DestroySelf()
    {
        DestroyImmediate(gameObject);
    }
}
