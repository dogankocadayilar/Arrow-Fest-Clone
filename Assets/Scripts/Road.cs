using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] Transform tailTransform;

    public Road head = null, tail = null;



    public void Attached(Road r)
    {
        r.transform.position = tailTransform.position;
        r.transform.rotation = tailTransform.rotation;
        tail = r;
        r.head = this;
    }
}
