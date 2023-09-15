using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotateCapsule : MonoBehaviour
{
    [SerializeField] private Transform rotationCenter;
    Vector3 rotationAxis;
    void Start()
    {
        rotationAxis = new Vector3(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        //gameObject.transform.RotateAround(rotationCenter, Vector3.left, 20 * Time.deltaTime);
        transform.RotateAround(rotationCenter.position, rotationAxis, 20 * Time.deltaTime);
    }
}
