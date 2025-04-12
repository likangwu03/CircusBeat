using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class LocalPersistence : BasePersistence
{
    protected bool isRunning;
    string filePath;
    StreamWriter writer;
    protected AutoResetEvent newEventSignal;
    protected Thread writerThread;

    public LocalPersistence(string sessionId, ISerializer serializer) : base(sessionId, serializer)
    {
        isRunning = false;
        filePath = Path.Combine(Application.persistentDataPath, sessionId + serializer.FileExtension);       
    }

    public override void SendEvent(TrackerEvent e)
    {
        eventQueue.Enqueue(e);
        Flush();
    }

    public override void SendEvents(IEnumerable<TrackerEvent> events)
    {
        foreach (TrackerEvent e in events)
        {
            eventQueue.Enqueue(e);
        }
        Flush();

    }

    public override void Flush()
    {
        newEventSignal.Set();
    }


    public override void Start()
    {
        isRunning = true;
        writer = new StreamWriter(filePath, true);
        newEventSignal = new AutoResetEvent(false);
        writerThread = new Thread(WriteToFile);
        writerThread.Start();
    }

    public override void Release()
    {
        Flush();
        isRunning = false;
        writerThread.Join();
        writer.Close();
    }

    private void WriteToFile()
    {
        writer.Write(serializer.Opener);

        while (isRunning)
        {
            newEventSignal.WaitOne();
            while (eventQueue.Count > 1)
            {
                if (eventQueue.TryDequeue(out TrackerEvent e))
                {
                    writer.Write(serializer.Serialize(e));
                    writer.Write(serializer.Separator);
                }
            }
            writer.Flush();
        }

        if (eventQueue.TryDequeue(out TrackerEvent last_event))
        {
            writer.Write(serializer.Serialize(last_event));
        }

        writer.Write(serializer.Closer);
        writer.Flush();
    }
}
