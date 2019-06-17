using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GazeInteractionEngine
{

    public class GazeInteraction_InteractableButton : MonoBehaviour, IGazeListener
    {

        #region Variables

            #region Parameters
        public float reponseTime = 0.3f;
        public float maxThresholdTime = 0.2f;
        public float focusTime = 0.8f;
        public float buttonTimeout = 1.0f;
        public Image buttonBackgroundImage;
        #endregion

            #region Private Variables
        private float _currentTime = 0.0f;
        private float _currentFocusTime = 0.0f;
        private float _currentThresholdTime = 0.0f;
        private bool _isInInteraction = false;
        private bool _isInThreshold = false;
        #endregion

            #region Events
        public UnityEvent OnButtonPressed;
        public UnityEvent OnButtonHover;
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
            if (_isInInteraction == false)
            {
                _currentTime += Time.deltaTime;

                if (_currentTime >= reponseTime)
                {
                    _currentTime = 0.0f;
                    _isInInteraction = true;
                    if (OnButtonHover != null)
                    {
                        OnButtonHover.Invoke();
                    }

                }
            }
            //Jak dłużej się patrzę na obiekt
            else
            {
                _isInThreshold = false;
                _currentFocusTime += Time.deltaTime;


                if(buttonBackgroundImage)
                    buttonBackgroundImage.fillAmount = (_currentFocusTime / focusTime);


                if (_currentFocusTime >= focusTime)
                {

                    _currentFocusTime = 0.0f;
                    StartCoroutine(PressedButton());

                    if (OnButtonPressed != null)
                    {
                        OnButtonPressed.Invoke();
                        EventManager.Instance.PostNotification(EVENT_TYPE.BROADCAST_BUTTON_PRESSED, this, broadcastInteractionObject);
                    }
                        
                }

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


            while (_isInThreshold)
            {
                _currentThresholdTime -= Time.deltaTime;

                if (_currentThresholdTime <= 0.0f)
                {
                    _isInInteraction = false;
                    _currentTime = 0.0f;
                    _isInThreshold = false;
                    StartCoroutine(ThresholdButtonCounter());
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }


        private IEnumerator ThresholdButtonCounter()
        {

            while(_currentFocusTime > 0.0f && !_isInInteraction)
            {
                _currentFocusTime -= Time.deltaTime;

                if(buttonBackgroundImage)
                    buttonBackgroundImage.fillAmount = (_currentFocusTime / focusTime);

                yield return new WaitForEndOfFrame();
            }

        }

        private IEnumerator PressedButton()
        {
            Animator animator = null;

            if(buttonBackgroundImage)
                animator = buttonBackgroundImage.GetComponent<Animator>();

            if(animator)
                animator.SetBool("animate", true);

            this.GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(buttonTimeout);
            this.GetComponent<Collider>().enabled = true;

            if (animator)
                animator.SetBool("animate", false);

            if(buttonBackgroundImage)
                buttonBackgroundImage.fillAmount = 0;
        }
        #endregion

        #endregion

    }
}
