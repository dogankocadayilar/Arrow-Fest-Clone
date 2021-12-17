using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> gates;

    Vector3 pos;

    GameObject gate;
    int x;

    void Start()
    {
        x = Random.Range(0, gates.Count);
        if (x == 1)
            gate = Instantiate(gates[x], transform.position, transform.rotation);
        else
        {
            int y = Random.Range(0, 2);
            pos = (y == 0) ? Vector3.left * 2.86f : Vector3.right * 2.86f;

            gate = Instantiate(gates[x], transform.position + pos, transform.rotation);

        }

        gate.transform.SetParent(transform);
    }
}
