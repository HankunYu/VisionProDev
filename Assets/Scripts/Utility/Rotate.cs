using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool enable;
    public float speed = 1f;
    public Vector3 axis = Vector3.up;
    
    private void Update()
    {
        if (enable)
        {
            transform.Rotate(axis, speed * Time.deltaTime);
        }
    }
}
