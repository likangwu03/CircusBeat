using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class ThreadedEventLogger
{
    private ConcurrentQueue<string> eventQueue = new ConcurrentQueue<string>();
    private Thread writerThread;
    private bool isRunning = true;
    private AutoResetEvent newEventSignal = new AutoResetEvent(false);


    public void Start()
    {
        writerThread = new Thread(WriteToFile);
        writerThread.Start();
    }

    public void OnDestroy()
    {
        isRunning = false;
        newEventSignal.Set();
        writerThread.Join();
    }

    public void AddEvent(string eventMessage)
    {
        eventQueue.Enqueue(eventMessage);
        if (eventQueue.Count == 1)
        {
            newEventSignal.Set();
        }
    }

    private void WriteToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "events.txt");
        while (isRunning || !eventQueue.IsEmpty)
        {
            if (eventQueue.TryDequeue(out string eventMessage))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(eventMessage);
                }
            }
            else
            {
                newEventSignal.WaitOne();
            }
        }
    }
}
