using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform trans;
    bool col = false;
    private int track = -1;

    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private int score = 1;

    public Vector3 targetPos { private get; set; }

    TrackerComponent trackerComp;


    private void Start()
    {
        trans = GetComponent<Transform>();

        trackerComp = TrackerComponent.Instance;
    }

    public void SetTrack(int tr) { track = tr; }
    private void Update()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        trans.position = Vector3.MoveTowards(trans.position, targetPos, step);
    }

    // se llama cuando el objeto se sale de la camara
    // (si se destruye antes de salir de la camara, tb se accede a este metodo)
    private void OnBecameInvisible()
    {
        if (GameManager.Instance != null && !col)
        {
            GameManager.Instance.addScore(score);
            GameManager.Instance.addCombo(score);

            // TRACKER EVENT Evasión de obstáculo
            if (trackerComp != null && trackerComp.Tracker != null)
            {
                trackerComp.SendEvent(trackerComp.Tracker.CreateObstacleEvent(gameObject.name, track, ObstacleEvent.ObstacleAction.EVASION));
            }

            Destroy(gameObject);
        }
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            col = true;
            GameManager.Instance.setCombo(0);
            GameManager.Instance.pLC.damage();

            // TRACKER EVENT Evasión de obstáculo
            if (trackerComp != null && trackerComp.Tracker != null)
            {
                trackerComp.SendEvent(trackerComp.Tracker.CreateObstacleEvent(gameObject.name, track, ObstacleEvent.ObstacleAction.COLLISION));
            }

            Destroy(gameObject);
        }
    }
}
