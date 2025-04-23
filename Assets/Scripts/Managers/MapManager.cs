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
    List<int>[] garbage;

    [SerializeField]
    private ColumnInfo[] columns;

    private EnemyAnimator eAnimations;
    private bool first = false;

    TrackerComponent trackerComp;
    int currPhase;


    // Start is called before the first frame update
    private void Start()
    {
        eAnimations = GameObject.Find("Enemy").GetComponent<EnemyAnimator>();

        // se reserva la memoria
        garbage = new List<int>[columns.Length];
        for (int i = 0; i < columns.Length; ++i)
        {
            columns[i].init();
            garbage[i] = new List<int>();
        }

        trackerComp = TrackerComponent.Instance;

        currPhase = 0;
        // TRACKER EVENT fase 0
        if (trackerComp != null && trackerComp.Tracker != null)
        {
            trackerComp.SendEvent(trackerComp.Tracker.CreatePhaseChangeEvent(currPhase));
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
                if (obstacles[j].timeInSeconds < GameManager.Instance.musicTime())
                {
                    GameObject obstacleObject = Instantiate(obstacles[j].gameobject, col.up, Quaternion.identity);
                    obstacleObject.name = obstacles[j].gameobject.name;

                    // TRACKER EVENT Spawn de obstáculo
                    if (trackerComp != null && trackerComp.Tracker != null)
                    {
                        trackerComp.SendEvent(trackerComp.Tracker.CreateObstacleEvent(obstacles[j].gameobject.name, i, GameTracker.ObstacleAction.SPAWN));
                    }

                    if ((obstacles[j].gameobject.name == "BowlingPin" || obstacles[j].gameobject.name == "Ball" || obstacles[j].gameobject.name == "Bunny") && !first)
                    {
                        first = true;
                        eAnimations.at_B();
                    }
                    Obstacle obstacleComp = obstacleObject.GetComponent<Obstacle>();
                    if (obstacleComp != null)
                    {
                        obstacleComp.SetTrack(i);
                        obstacleComp.targetPos = col.down;
                        garbage[i].Add(j);
                    }
                    else
                    {
                        Destroy(obstacleObject);
                    }

                    if (GameManager.Instance.musicTime() > 80)
                    {
                        eAnimations.e_Idle();

                        if (currPhase < 3)
                        {
                            currPhase++;
                            // TRACKER EVENT fase 3
                            if (trackerComp != null && trackerComp.Tracker != null)
                            {
                                trackerComp.SendEvent(trackerComp.Tracker.CreatePhaseChangeEvent(currPhase));
                            }
                        }
                    }
                    else if (GameManager.Instance.musicTime() > 63)
                    {
                        eAnimations.at_B();

                        if (currPhase < 2)
                        {
                            currPhase++;
                            // TRACKER EVENT fase 2
                            if (trackerComp != null && trackerComp.Tracker != null)
                            {
                                trackerComp.SendEvent(trackerComp.Tracker.CreatePhaseChangeEvent(currPhase));
                            }
                        }
                    }
                    else if (GameManager.Instance.musicTime() > 39.45)
                    {
                        eAnimations.at_R();

                        if (currPhase < 1)
                        {
                            currPhase++;
                            // TRACKER EVENT fase 1
                            if (trackerComp != null && trackerComp.Tracker != null)
                            {
                                trackerComp.SendEvent(trackerComp.Tracker.CreatePhaseChangeEvent(currPhase));
                            }
                        }
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
