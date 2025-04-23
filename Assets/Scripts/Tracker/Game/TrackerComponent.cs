using System;
using System.Collections.Generic;
using UnityEngine;


public class TrackerComponent : MonoBehaviour
{
    public static TrackerComponent Instance = null;
    public GameTracker Tracker { get; private set; }

    [SerializeField]
    bool active = true;

    [SerializeField]
    uint maxQueueSize = 100;

    [SerializeField]
    float sendTime = 5.0f;

    // Configuracion de serializacion y persistencia
    public enum SerializationTypes { JSON, XML, /* BINARY, CSV, ... */ }
    public enum PersistenceTypes { LOCAL, /* REMOTE, ... */}

    [Serializable]
    public class PersistenceSettings
    {
        public SerializationTypes serialization;
        public List<PersistenceTypes> persistence = new List<PersistenceTypes>();
    }
    
    [SerializeField]
    List<PersistenceSettings> persistenceSettings = new List<PersistenceSettings>();


    // Configuracion de eventos
    enum AllEventTypes {
        IGNORE_ALL = TrackerEventType.IGNORE_ALL,
        SESSION_START = TrackerEventType.SESSION_START,
        SESSION_END = TrackerEventType.SESSION_END,

        LEVEL_START = GameEventType.LEVEL_START,
        SONG_START = GameEventType.SONG_START,
        SONG_END = GameEventType.SONG_END,
        PLAYER_DEATH = GameEventType.PLAYER_DEATH,
        LEVEL_END = GameEventType.LEVEL_END,
        LEVEL_QUIT = GameEventType.LEVEL_QUIT,
        PHASE_CHANGE = GameEventType.PHASE_CHANGE,
        OBSTACLE_SPAWN = GameEventType.OBSTACLE_SPAWN,
        OBSTACLE_COLLISION = GameEventType.OBSTACLE_COLLISION,
        OBSTACLE_DODGE = GameEventType.OBSTACLE_DODGE,
        RECOVER_HEALTH = GameEventType.RECOVER_HEALTH,
        PLAYER_MOVEMENT = GameEventType.PLAYER_MOVEMENT
    };

    [SerializeField]
    List<AllEventTypes> eventsToIgnore = new List<AllEventTypes>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (active)
            {
                CreateAndConfigureTracker();
            }
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        if (Tracker != null)
        {
            Tracker.Close();
        }
    }


    public void CreateAndConfigureTracker()
    {
        string sessionId = Environment.MachineName + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();
        Tracker = new GameTracker(sessionId, maxQueueSize, SetupPersistenceMethods(sessionId), SetupIgnoredEvents());
        Tracker.Open();
        InvokeRepeating(nameof(SendEvent), sendTime, sendTime);
    }

    private HashSet<BasePersistence> SetupPersistenceMethods(string sessionId)
    {
        // Se guardan todos los metodos de persistencia con el mismo tipo de serialización en
        // la misma lista (por si hay metodos de serializacion iguales con listas distintas)
        Dictionary<SerializationTypes, HashSet<PersistenceTypes>> uniqueSettings = new Dictionary<SerializationTypes, HashSet<PersistenceTypes>>(); 
        foreach (PersistenceSettings setting in persistenceSettings)
        {
            if (!uniqueSettings.ContainsKey(setting.serialization))
            {
                uniqueSettings.Add(setting.serialization, new HashSet<PersistenceTypes>());
            }

            foreach (PersistenceTypes persistence in setting.persistence)
            {
                uniqueSettings[setting.serialization].Add(persistence);
            }
        }
        
        // Se crean los metodos de persistencia a partir de la configuracion
        HashSet<BasePersistence> persistenceMethods = new HashSet<BasePersistence>();
        foreach (var setting in uniqueSettings)
        {
            // Se crea el metodo de serializacion una unica vez para poder reutilizarlo
            ISerializer serializer = null;
            switch (setting.Key)
            {
                case SerializationTypes.JSON: serializer = new JsonSerializer(); break;
                case SerializationTypes.XML: serializer = new XMLSerializer(); break;

                // Para anadir soporte para nuevos metodos de serializacion:
                //case SerializationTypes.BINARY: serializer = new BinarySerializer(); break;
                //case SerializationTypes.CSV: serializer = new CSVSerializer(); break;
            }
            
            // Se recorren todos los metodos de persistencia asociados al metodo de
            // serializacion y se crea el persistor correspondiente con el formato indicado
            foreach (PersistenceTypes persistence in setting.Value)
            {
                switch (persistence)
                {
                    case PersistenceTypes.LOCAL: 
                        persistenceMethods.Add(new LocalPersistence(sessionId, serializer)); 
                        break;

                    // Para anadir soporte para nuevos metodos de persistencia:
                    //case PersistenceTypes.REMOTE:
                    //    persistenceMethods.Add(new RemotePersistence(sessionId, serializer));
                    //    break;
                }
            }
        }

        return persistenceMethods;
    }

    private HashSet<int> SetupIgnoredEvents()
    {
        HashSet<int> ignoredEvents = new HashSet<int>();

        // Se guardan todos los eventos a ignorar en un set para ignorar repeticiones
        foreach (AllEventTypes type in eventsToIgnore)
        {
            ignoredEvents.Add((int)type);
        }

        // Si hay tantos eventos a ignorar como eventos en total (sin incluir el de ignorar todos), se ignoran todos los eventos
        if (ignoredEvents.Count >= Enum.GetNames(typeof(AllEventTypes)).Length - 1)
        {
            ignoredEvents.Add((int)AllEventTypes.IGNORE_ALL);
        }

        return ignoredEvents;
    }

    public void SendEvent(ITrackerEvent evt)
    {
        evt.Send(Tracker);
        //Tracker.SendEvent(evt);
    }

    public void SendEvent()
    {
        Tracker.SendEvent();
    }

}
