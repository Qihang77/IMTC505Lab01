using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);
        transform.localEulerAngles = new Vector3(-90, transform.rotation.y, 0);
    }
}
