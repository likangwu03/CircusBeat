using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class --> referencia
// struct --> instancia
[System.Serializable]
public class ObstacleInfo
{
    public GameObject gameobject;
    public float timeInSeconds;
}

// no hace falta hacer un new porque al haber hecho la clase serializable,
// si se asigna desde el editor, ya se crea sola
[System.Serializable]
public class ColumnInfo
{
    public Vector3 up { get; private set; }
    public Vector3 down { get; private set; }
    public Vector3 direction { get; private set; }

    [SerializeField]
    private GameObject pointUp;
    [SerializeField]
    private GameObject pointDown;

    [SerializeField]
    public List<ObstacleInfo> obstacles;

    public void init()
    {
        // mejorar un poco el rendimiento
        up = pointUp.transform.position;
        down = pointDown.transform.position;
        direction = (pointDown.transform.position - pointUp.transform.position).normalized;

        // se ordena de menor a mayor por tiempo
        obstacles.Sort((obstacle1, obstacle2) => obstacle1.timeInSeconds.CompareTo(obstacle2.timeInSeconds));

        pointUp.GetComponent<SpriteRenderer>().enabled = false;
        pointDown.GetComponent<SpriteRenderer>().enabled = false;
    }
}

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _music;

    List<int>[] garbage;

    [SerializeField]
    private ColumnInfo[] columns;

    // Start is called before the first frame update
    private void Start()
    {
        _music.Play();
        // se reserva la memoria
        garbage = new List<int>[columns.Length];
        for (int i = 0; i < columns.Length; ++i)
        {
            columns[i].init();
            garbage[i] = new List<int>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < columns.Length; ++i)
        {
            ColumnInfo col = columns[i];
            for (int j = 0; j < col.obstacles.Count; ++j)
            {
                List<ObstacleInfo> obstacles = col.obstacles;
                if (obstacles[j].timeInSeconds < _music.time)
                {
                    GameObject obstacleObject = Instantiate(obstacles[j].gameobject, col.up, Quaternion.identity);
                    Obstacle obstacleComp = obstacleObject.GetComponent<Obstacle>();
                    if (obstacleComp != null)
                    {
                        obstacleComp.targetPos = col.down;
                        garbage[i].Add(j);
                    }
                    else
                    {
                        Destroy(obstacleObject);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 0; i < garbage.Length; ++i)
        {
            for (int j = 0; j < garbage[i].Count; ++j)
            {
                columns[i].obstacles.RemoveAt(garbage[i][j]);
            }
            garbage[i].Clear();
        }
    }
}
