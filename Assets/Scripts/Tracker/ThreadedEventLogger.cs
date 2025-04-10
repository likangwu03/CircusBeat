using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ThreadedEventLogger
{
    private ConcurrentQueue<TrackerEvent> eventQueue;
    private Thread writerThread;
    private bool isRunning;
    //private AutoResetEvent newEventSignal;
    StreamWriter writer;
    private Mutex mut;

    public ThreadedEventLogger(string sessionId)
    {
        eventQueue = new ConcurrentQueue<TrackerEvent>();
        isRunning = false;
        //newEventSignal = new AutoResetEvent(false);
        string filePath = Path.Combine(Application.persistentDataPath, sessionId + ".json");
        writer = new StreamWriter(filePath, true);
        mut = new Mutex();
    }

    public void Start()
    {
        writerThread = new Thread(WriteToFile);
        writerThread.Start();
    }

    public void Destroy()
    {
        isRunning = false;
        mut.ReleaseMutex();
        writerThread.Join();
        writer.Write("]");
        writer.Close();
    }

    public void WriteEvent(IEnumerable<TrackerEvent> events)
    {
        foreach (TrackerEvent e in events)
        {
            eventQueue.Enqueue(e);

        }
        mut.ReleaseMutex();
    }

    private void WriteToFile()
    {
        while (isRunning || !eventQueue.IsEmpty)
        {

            if (eventQueue.TryDequeue(out TrackerEvent e))
            {
                writer.Write(",");
                writer.Write(JsonUtility.ToJson(e));
            }
            else
            {
                mut.WaitOne();
            }
        }
    }
}
