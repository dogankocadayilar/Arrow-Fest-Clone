using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    [SerializeField] Material blue;
    [SerializeField] Material red;
    [SerializeField] Text eventText;

    [Header("Math Events")]
    [SerializeField] int maxMultiply = 3;
    [SerializeField] int maxSum = 80;
    [SerializeField] int maxSubstract = 80;
    [SerializeField] int maxDivide = 4;


    [SerializeField] public string gateEvent;
    [SerializeField] public int value;

    MeshRenderer mat;


    void Start()
    {

        int x = Random.Range(0, 2);

        mat = this.gameObject.GetComponent<MeshRenderer>();

        if (x == 0) { mat.material = blue; eventText.text = BlueEvent();}
        else { mat.material = red; eventText.text = RedEvent(); }

        gateEvent = eventText.text.Substring(0,1);
        value = int.Parse(eventText.text.Substring(1));
    }


    string BlueEvent()
    {
        int rand = Random.Range(0, 7);
        int sum = Random.Range(1, maxSum);
        int mult = Random.Range(2, maxMultiply);

        return (rand < 5) ? "+" + sum : "X" + mult;

    }

    string RedEvent()
    {
        int rand = Random.Range(0, 7);
        int sub = Random.Range(1, maxSubstract);
        int div = Random.Range(1, maxDivide);

        return (rand < 5) ? "-" + sub : "/" + div;

    }

}
