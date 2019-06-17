using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GazeInteractionEngine
{


    public class GazeInteraction_InteractableObject : MonoBehaviour, IGazeListener
    {

        #region Variables

            #region Parameters
        public float reponseTime = 0.5f;
        public float maxThresholdTime = 0.25f;
        #endregion

            #region Private Variables
        private float _currentTime = 0.0f;
        private float _currentThresholdTime = 0.0f;
        private bool _isInInteraction = false;
        private bool _isInThreshold = false;
        #endregion

            #region Events
        public UnityEvent OnStartInteraction;
        public UnityEvent OnStayInteration;
        public UnityEvent OnStopInteraction;

        public Object broadcastInteractionObject;
        #endregion

        #endregion

        #region Functions

        #region IGazeListener Interface
        public void OnEvent(EVENT_TYPE eventType, Component Sender, Object param = null)
        {

            switch (eventType)
            {
                case EVENT_TYPE.ON_EYE_INTERACTION:
                    {
                        OnEyeInteraction();
                        break;
                    }
                case EVENT_TYPE.ON_EYE_STOP_INTERACTION:
                    {
                        OnEyeStopInteraction();
                        break;
                    }
                    
            }
        }
        #endregion

            #region Interactions
        private void OnEyeInteraction()
        {
            //Poczatek interakcji wzroku z obiektem
            if(_isInInteraction == false)
            {
                _currentTime += Time.deltaTime;

                if (_currentTime >= reponseTime)
                {
                    _currentTime = 0.0f;
                    _isInInteraction = true;
                    if (OnStartInteraction != null)
                    {
                        OnStartInteraction.Invoke();
                        EventManager.Instance.PostNotification(EVENT_TYPE.BROADCAST_OBJECT_START_INTERACTION, this, broadcastInteractionObject);
                    }

                }
            }
            //Jak dłużej się patrzę na obiekt
            else
            {
                _isInThreshold = false;
                if(OnStayInteration != null)
                    OnStayInteration.Invoke();

            }

        }

        private void OnEyeStopInteraction()
        {
            
            _isInThreshold = true;
            _currentTime = 0.0f;
            StartCoroutine(ThresholdCounter());
        }

        #endregion

            #region IEnumerators
        private IEnumerator ThresholdCounter()
        {
            _currentThresholdTime = maxThresholdTime;


            while(_isInThreshold)
            {
                _currentThresholdTime -= Time.deltaTime;
                
                if(_currentThresholdTime <= 0.0f)
                {
                    if (OnStopInteraction != null)
                    {
                        OnStopInteraction.Invoke();
                        EventManager.Instance.PostNotification(EVENT_TYPE.BROADCAST_OBJECT_END_INTERACTION, this, broadcastInteractionObject);
                    }

                    _isInInteraction = false;
                    _currentTime = 0.0f;
                    _isInThreshold = false;

                }
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion

        #endregion

    }
}