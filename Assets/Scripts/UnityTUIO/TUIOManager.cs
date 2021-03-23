using System.Collections;
using System.Collections.Generic;
using System.Text;
using TUIO;
using UnityEngine;

namespace UnityTUIO
{
    public class TUIOManager : MonoBehaviour
    {
        private static TUIOManager instance = null;
        private static bool instanceDestroyed = false;

        public static TUIOManager Instance
        {
            get
            {
                if (instanceDestroyed)
                {
                    return null;
                }

                if (instance == null)
                {
                    new GameObject("TUIOManager").AddComponent<TUIOManager>();
                }

                return instance;
            }
        }

        private readonly Queue<TUIOObjectMessage> messageQueue = new Queue<TUIOObjectMessage>();
        private readonly Stack<TUIOObjectMessage> messagePool = new Stack<TUIOObjectMessage>();

        private readonly Dictionary<int, ITUIOController> controllerDictionary = new Dictionary<int, ITUIOController>();

        private TUIOObjectMessage currentMessage = null;

        private readonly TUIOListenerImpl tuioListener = new TUIOListenerImpl();
        private readonly TuioClient tuioClient = new TuioClient();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            ProcessMessageQueue();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                tuioClient.disconnect();
                instanceDestroyed = true;
            }
        }

        private bool isDebugShowing = false;
        private readonly StringBuilder debugStringBuilder = new StringBuilder();

        private void OnGUI()
        {
            if (isDebugShowing)
            {
                GUI.Box(new Rect(10, 10, 500, 170), "TUIO Debug Messages");
                if (currentMessage != null)
                {
                    GUI.Label(new Rect(30, 40, 480, 50), currentMessage.tuioObject.ToString());
                }
                debugStringBuilder.Clear();
                debugStringBuilder.AppendLine("Registered Symbol ID:");
                foreach (int id in controllerDictionary.Keys)
                {
                    debugStringBuilder.Append(id).Append(" ");
                }
                GUI.Label(new Rect(30, 60, 480, 50), debugStringBuilder.ToString());
                if (GUI.Button(new Rect(30, 120, 460, 30), "Hide"))
                {
                    isDebugShowing = false;
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, 10, 250, 30), "Show TUIO Debug Messages"))
                {
                    isDebugShowing = true;
                }
            }
        }

        private void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            tuioClient.addTuioListener(tuioListener);
            tuioClient.connect();
        }

        public void RegisterController(ITUIOController controller)
        {
            if (controllerDictionary.ContainsKey(controller.SymbolID))
            {
                Debug.Log($"Registration failed. Symbol ID {controller.SymbolID} has been registered.");
                return;
            }
            controllerDictionary.Add(controller.SymbolID, controller);
        }

        public void UnregisterController(ITUIOController controller)
        {
            if (!controllerDictionary.Remove(controller.SymbolID))
            {
                Debug.Log($"Unregistration failed. Symbol ID {controller.SymbolID} has not been registered.");
            }
        }

        public void EnqueueMessage(TuioObject tuioObject, TUIOMessageType messageType)
        {
            TUIOObjectMessage message;
            if (messagePool.Count > 0)
            {
                message = messagePool.Pop();
                message.tuioObject = tuioObject;
                message.messageType = messageType;
            }
            else
            {
                message = new TUIOObjectMessage(tuioObject, messageType);
            }
            messageQueue.Enqueue(message);
        }

        private void ProcessMessageQueue()
        {
            lock ((messageQueue as ICollection).SyncRoot)
            {
                if (messageQueue.Count == 0)
                {
                    return;
                }
                currentMessage = messageQueue.Dequeue();
            }
            if (controllerDictionary.TryGetValue(currentMessage.tuioObject.SymbolID, out ITUIOController controller))
            {
                switch (currentMessage.messageType)
                {
                    case TUIOMessageType.Add:
                        controller.TUIOObjectAppear(currentMessage.tuioObject);
                        break;
                    case TUIOMessageType.Update:
                        controller.TUIOObjectUpdate(currentMessage.tuioObject);
                        break;
                    case TUIOMessageType.Remove:
                        controller.TUIOObjectDisappear(currentMessage.tuioObject);
                        break;
                    default:
                        break;
                }
            }
            messagePool.Push(currentMessage);
        }
    }
}
