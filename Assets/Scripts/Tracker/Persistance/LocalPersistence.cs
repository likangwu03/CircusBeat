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
        eventQueue = new ConcurrentQueue<TrackerEvent>();
        isRunning = false;
        filePath = Path.Combine(Application.persistentDataPath, sessionId + serializer.FileExtension);
        writer = new StreamWriter(filePath, true);
        newEventSignal = new AutoResetEvent(false);
        this.serializer = serializer;
    }

    public override void SendEvent(TrackerEvent e)
    {
        throw new System.NotImplementedException();
    }

    public override void SendEvents(IEnumerable<TrackerEvent> events)
    {
        throw new System.NotImplementedException();
    }
    public override void Flush()
    {
        newEventSignal.Set();
    }


    public void Start()
    {
        writerThread = new Thread(WriteToFile);
        writerThread.Start();
    }

    public void Release()
    {
        Flush();
        isRunning = false;
        writerThread.Join();
        writer.Close();

    }

    private void WriteToFile()
    {
        writer.Write("[");
        while (isRunning || !eventQueue.IsEmpty)
        {
            newEventSignal.WaitOne();
            while (eventQueue.Count > 0)
            {
                if (eventQueue.TryDequeue(out TrackerEvent e))
                {
                    writer.Write(serializer.Serialize(e));
                    writer.Write(",");
                }
            }
        }
        writer.Flush();
        CloseFile();
    }


    private void CloseFile()
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
        {
            fileStream.Flush();
            fileStream.Seek(-1, SeekOrigin.End);
            int lastByte = fileStream.ReadByte();

            if (lastByte == (byte)',')
            {
                fileStream.SetLength(fileStream.Length - 1);
            }

            // Agregar el cierre del array JSON
            byte[] endArray = Encoding.UTF8.GetBytes("]");
            fileStream.Write(endArray, 0, endArray.Length);

            // Asegurar que el cierre se escriba en el archivo
            fileStream.Flush();
        }
    }
}
