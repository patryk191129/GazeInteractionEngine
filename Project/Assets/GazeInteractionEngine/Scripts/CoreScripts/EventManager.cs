using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GazeInteractionEngine
{

    public class EventManager : MonoBehaviour
    {
        #region C# properties
        //-----------------------------------
        //Public access to instance
        public static EventManager Instance
        {
            get { return instance; }
            set { }
        }
        #endregion

        #region variables
        // Notifications Manager instance (singleton design pattern)
        private static EventManager instance = null;

        //Array of listeners (all objects registered for events)
        private Dictionary<EVENT_TYPE, List<IGazeListener>> Listeners = new
        Dictionary<EVENT_TYPE, List<IGazeListener>>();
        #endregion
        //-----------------------------------------------------------
        #region methods
        //Called at start-up to initialize
        void Awake()
        {
            //If no instance exists, then assign this instance
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                DestroyImmediate(this);
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Function to add listener to array of listeners
        /// </summary>
        /// <param name="Event_Type">Event to Listen for</param>
        /// <param name="Listener">Object to listen for event</param>
        public void AddListener(EVENT_TYPE Event_Type, IGazeListener Listener)
        {
            //List of listeners for this event
            List<IGazeListener> ListenList = null;

            // Check existing event type key. If exists, add to list
            if (Listeners.TryGetValue(Event_Type, out ListenList))
            {
                //List exists, so add new item
                ListenList.Add(Listener);
                return;
            }

            //Otherwise create new list as dictionary key
            ListenList = new List<IGazeListener>();
            ListenList.Add(Listener);
            Listeners.Add(Event_Type, ListenList);
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Function to post event to listeners
        /// </summary>
        /// <param name="Event_Type">Event to invoke</param>
        /// <param name="Sender">Object invoking event</param>
        /// <param name="Param">Optional argument</param>
        public void PostNotification(EVENT_TYPE Event_Type, Component Sender,
        Object Param = null)
        {
            //Notify all listeners of an event

            //List of listeners for this event only
            List<IGazeListener> ListenList = null;

            //If no event exists, then exit
            if (!Listeners.TryGetValue(Event_Type, out ListenList))
                return;

            //Entry exists. Now notify appropriate listeners
            for (int i = 0; i < ListenList.Count; i++)
            {
                if (!ListenList[i].Equals(null))

                    ListenList[i].OnEvent(Event_Type, Sender, Param);
            }

        }

        //-----------------------------------------------------------
        //Remove event from dictionary, including all listeners
        public void RemoveEvent(EVENT_TYPE Event_Type)
        {
            //Remove entry from dictionary
            Listeners.Remove(Event_Type);
        }
        //-----------------------------------------------------------
        //Remove all redundant entries from the Dictionary
        public void RemoveRedundancies()
        {
            //Create new dictionary
            Dictionary<EVENT_TYPE, List<IGazeListener>> TmpListeners = new
            Dictionary<EVENT_TYPE, List<IGazeListener>>();

            //Cycle through all dictionary entries
            foreach (KeyValuePair<EVENT_TYPE, List<IGazeListener>> Item in Listeners)
            {
                //Cycle all listeners, remove null objects
                for (int i = Item.Value.Count - 1; i >= 0; i--)
                {
                    //If null, then remove item
                    if (Item.Value[i].Equals(null))
                        Item.Value.RemoveAt(i);
                }

                //If items remain in list, then add to tmp dictionary
                if (Item.Value.Count > 0)
                    TmpListeners.Add(Item.Key, Item.Value);
            }

            //Replace listeners object with new dictionary
            Listeners = TmpListeners;
        }
        //-----------------------------------------------------------
        //Called on scene change. Clean up dictionary
        void OnLevelWasLoaded()
        {
            RemoveRedundancies();
        }
        //-----------------------------------------------------------
        #endregion
    }

}