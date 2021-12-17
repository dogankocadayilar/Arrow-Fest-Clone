using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    [SerializeField] Transform arrow;
    [SerializeField] public Vector3 pos;
    [SerializeField] float dampTime = 1f;

    Vector3 vel = Vector3.zero;
    Vector3 dampPos;

    void Update()
    {
        dampPos = new Vector3(transform.position.x, arrow.position.y, arrow.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, dampPos + pos, ref vel, dampTime);
    }
}
