using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeInteractionEngine
{
    public class GazeEventListener : MonoBehaviour, IGazeListener
    {

        private struct Events
        {
            public EVENT_TYPE eventType;
            public float timestamp;
            public Component sender;
            public Object eventObject;
        }


        public bool displayLog;
        List<Events> _events;

        private void Start()
        {
            _events = new List<Events>();

            EventManager.Instance.AddListener(EVENT_TYPE.BROADCAST_BUTTON_PRESSED, this);
            EventManager.Instance.AddListener(EVENT_TYPE.BROADCAST_OBJECT_END_INTERACTION, this);
            EventManager.Instance.AddListener(EVENT_TYPE.BROADCAST_OBJECT_START_INTERACTION, this);
        }

        public void OnEvent(EVENT_TYPE eventType, Component Sender, Object param = null)
        {
            Events newEvent = new Events();
            newEvent.eventType = eventType;
            newEvent.timestamp = Time.realtimeSinceStartup;
            newEvent.eventObject = param;
            newEvent.sender = Sender;

            _events.Add(newEvent);

            if(displayLog)
                Debug.Log("Registered new Event: " + eventType + " at + " + Time.realtimeSinceStartup + " on " + newEvent.eventObject);
        }
    }
}