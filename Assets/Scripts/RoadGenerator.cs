using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> roads;
    [SerializeField] GameObject finishRoad;
    [SerializeField] int levelLength = 0;

    public Road current;
    Road head;

    void OnEnable()
    {
        PlayerData data = SaveSystem.LoadSave();
        if (data != null)
            levelLength = (Mathf.FloorToInt(data.level / 2) > 5) ? Mathf.FloorToInt(data.level / 2) : 5;
        else levelLength = 5;


        for (int i = 0; i < levelLength; i++)
        {
            var road = Instantiate(roads[Random.Range(0, roads.Count)],
                transform).GetComponent<Road>();
            var rp = road.transform.position;
            rp.x = 0f;
            road.transform.position = rp;
            if (head == null) head = road;
            current?.Attached(road);
            current = road;
        }

        var finish = Instantiate(finishRoad, transform).GetComponent<Road>();
        var fp = finish.transform.position;
        fp.x = 0f;
        finish.transform.position = fp;
        current?.Attached(finish);
        current = finish;
    }

}
