using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class ArrowController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int maxNumOfArrows = 10;
    [SerializeField] float dampTime = .5f;
    [SerializeField] float forwardDampTime = .5f;
    [SerializeField] float finishDuration = 10f;
    [SerializeField] float duration = 10f;
    [SerializeField] float lastDuration = 10f;
    [SerializeField] float speedX = 1f;

    [Header("Objects")]
    [SerializeField] Camera main;
    [SerializeField] GameObject arrow;
    [SerializeField] TextMeshProUGUI numOfArrows ;
    [SerializeField] GameObject roadgen;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] GameObject nextLevel;

    [Header("Save")]
    [SerializeField] public int level;
    [SerializeField] public int gold;

    Vector3 vel = Vector3.zero;
    CameraFallow camPos;
    List<GameObject> arrowPool = new List<GameObject>();
    Transform finish;
    Rigidbody rb;

    float x;
    float rad = 0.05f;

    void OnEnable()
    {
        PlayerData data = SaveSystem.LoadSave();

        if(data != null)
        {
            level = data.level;
            duration = data.level >= 10 ? data.level : 10f;
            gold = data.gold;
        }
        else
        {
            level = 1;
            duration = 10f;
            gold = 0;
        }
    }

    void Start()
    {
        levelText.text = "Level " + level.ToString();
        goldText.text = gold.ToString();
        numOfArrows.text = "1";
        rb = GetComponent<Rigidbody>();


        var center = transform.position;
        for (int i = 0; i < maxNumOfArrows; i++)
        {
            Vector3 pos = RandomCircle(center, rad);
            GameObject ar = Instantiate(arrow, pos, Quaternion.Euler(-53f, -90f, 0f));
            ar.transform.SetParent(transform);
            if (i % 15 == 0) rad += 0.05f;

            ar.SetActive(false);
            arrowPool.Add(ar);
        }


        finish = roadgen.GetComponent<RoadGenerator>().current.transform;

        camPos = main.GetComponent<CameraFallow>();

    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    bool canMove = true;
    void FixedUpdate()
    {

        if (Input.GetMouseButton(0) && canMove)
        {

            x = Input.GetAxisRaw("Mouse X");

            var pos = transform.position;
            pos.x -= x * speedX;
            pos.x = Mathf.Clamp(pos.x, -4.0f, 4.0f);
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref vel, dampTime);

        }
    }

    void Move()
    {
        transform.DOMoveZ(finish.position.z - 30f, duration, false).SetEase(Ease.Linear).OnComplete(() =>
        {
            Merge();
            canMove = false;
            numOfArrows.gameObject.SetActive(false);
            main.DOShakePosition(0.3f).OnComplete(() =>
            {
                camPos.pos = new Vector3(0f, camPos.pos.y + 10f, camPos.pos.z);
                main.transform.DORotateQuaternion(Quaternion.Euler(25f, 180f, 0f), duration);
                rb.isKinematic = true;
                transform.DOMoveZ(finish.position.z - 150f, lastDuration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    gold += int.Parse(numOfArrows.text);
                    goldText.text = gold.ToString();
                    nextLevel.SetActive(true);
                });
            });
        });
    }

    void Merge()
    {
        float x = -5.0f;
        float y = 0.0f;
        foreach (GameObject arrow in arrowPool)
        {
            arrow.transform.DOMove(new Vector3(x, transform.position.y + y, transform.position.z), finishDuration);
            x += 0.5f;
            if (x == 5) { y += 0.1f; x = -5.0f; }
        }
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Gate")
        {
            Gate gate = other.GetComponent<Gate>();
            switch (gate.gateEvent) { 
                case "+":
                    Sum(gate.value);
                    break;
                case "-":
                    Sub(gate.value);
                    break;
                case "X":
                    Mult(gate.value);
                    break;
                case "/":
                    Div(gate.value);
                    break;
            }
        }
    }

    void Sum(int value)
    {

        int num = int.Parse(numOfArrows.text);
        int x = num;

        for (int i = num; i < value + num; i++)
        {
            x++;
            if(x < maxNumOfArrows)
                if (!arrowPool[i].activeInHierarchy) arrowPool[i].SetActive(true);
            numOfArrows.text = x.ToString();
        }
    }

    void Sub(int value)
    {
        main.DOShakePosition(.1f);

        int num = int.Parse(numOfArrows.text);
        int x = num;
        if (num > value)
        {
            for (int i = num; i > num - value; i--)
            {
                x--;
                if(x < maxNumOfArrows)
                    if (arrowPool[i].activeInHierarchy) arrowPool[i].SetActive(false);
                numOfArrows.text = x.ToString();
            }
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }
    void Mult(int value)
    {
        int x = int.Parse(numOfArrows.text);
        int y = x;
        for (int i = x; i < (value * x); i++)
        {
            y++;
            if (y < maxNumOfArrows)
                if (!arrowPool[i].activeInHierarchy) arrowPool[i].SetActive(true);
            numOfArrows.text = y.ToString();
        }
    }
    void Div(int value)
    {
        main.DOShakePosition(.1f);

        int x = int.Parse(numOfArrows.text);
        int y = x;
        if (x >= value)
        {
            for (int i = x; i > (x / value); i--)
            {
                y--;
                if(x < maxNumOfArrows)
                    if (arrowPool[i].activeInHierarchy) arrowPool[i].SetActive(false);
                numOfArrows.text = y.ToString();
            }
        }
    }

    public void Play()
    {
        Move();
    }

    public void NextLevel()
    {
        level++;
        SaveSystem.SaveArrowController(this);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
