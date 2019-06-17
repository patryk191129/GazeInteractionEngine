using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;
using UnityEngine.UI;
using System;
using UnityEditor;
using System.IO;
using SwipeType;

public class ExperimentEngine : MonoSingleton<ExperimentEngine>
{

    #region Enumerators
    private enum ExperimentMode
    {
        NORMAL_KEYBOARD,
        SWYPE_KEYBOARD
    }


    private enum ExperimentStatus
    {
        INACTIVE,
        ACTIVE_SWYPE,
        ACTIVE_NORMAL_KEYBOARD
    }
    #endregion


    ExperimentMode experimentMode = ExperimentMode.NORMAL_KEYBOARD;
    ExperimentStatus experimentStatus = ExperimentStatus.INACTIVE;


    public GameObject phraseConsole;
    public GameObject calibrationObject;
    public GameObject playerCamera;
    public GameObject UI;

    private GameObject _calibrationInstantiatedObject;


    public Text phraseText;
    public Text outputPhraseText;

    public AudioClip correctBell;
    public AudioClip incorrectBell;
    public AudioClip doneExperimentBell;
    public KeyCode keycode;


    public Color greenRay;
    public Color redRay;

    private AudioSource _audioSource;
    private SwypeKeyboard _swypeKeyboard;


    private string _targetString;
    private string _currentString;
    private string _path;
    private string[] _targetWords;

    private int _swypeCurrentIndex;


    private bool _isPressedButton = false;
    private bool _isButtonReleased = false;
    private bool _currentPressed = false;


    struct ExperimentSample
    {
        public float timestamp;
        public string character;
    }

    private struct Experiment
    {
        public string date;
        public string time;
        public string participant;
        public string phrase;

        public List<ExperimentSample> samples;

    }


    public void Calibration()
    {
        _calibrationInstantiatedObject = Instantiate(calibrationObject);
        UI.SetActive(false);
        playerCamera.GetComponent<Camera>().enabled = false;
    }

    public void EndCalibration()
    {
        Destroy(_calibrationInstantiatedObject);
        UI.SetActive(true);
        playerCamera.GetComponent<Camera>().enabled = true;
    }

    public bool IsInactive()
    {
        return experimentStatus == ExperimentStatus.INACTIVE;
    }

    Experiment experiment;

    private MatchSwipeType _selectedSwipeType;



    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _swypeKeyboard = GetComponent<SwypeKeyboard>();

        phraseConsole.SetActive(false);
    }


    public void SetSwipeType(MatchSwipeType matchSwipeType)
    {
        _selectedSwipeType = matchSwipeType;
        Debug.Log("Set new dictionary");
    }


    public void Abort()
    {
        experimentStatus = ExperimentStatus.INACTIVE;
        phraseConsole.SetActive(false);
    }

    public void InsertString(string value)
    {
        if (experimentStatus != ExperimentStatus.INACTIVE)
        {
            switch (experimentMode)
            {
                case ExperimentMode.NORMAL_KEYBOARD:
                    InsertStringNormalKeyboard(value);
                    break;
                case ExperimentMode.SWYPE_KEYBOARD:
                    InsertStringSwypeKeyboard(value);
                    break;
            }
        }
    }



    public void EndExperiment()
    {
        experimentStatus = ExperimentStatus.INACTIVE;
        phraseConsole.SetActive(false);


        if (_path.Length != 0)
        {

            try
            {
                FileStream fileStream = File.Open(_path, FileMode.Append, FileAccess.Write);

                StreamWriter fileWriter = new StreamWriter(fileStream);

                fileWriter.WriteLine(experimentMode + "," + experiment.participant + "," + experiment.phrase + "," + experiment.date + "," + experiment.time);

                ExperimentSample firstSample = experiment.samples[0];


                foreach(ExperimentSample sample in experiment.samples)
                    fileWriter.WriteLine((sample.timestamp - firstSample.timestamp) + "," + sample.character);


                fileWriter.Flush();
                fileWriter.Close();
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe);
            }


        }
    }



    private void InsertStringSwypeKeyboard(string value)
    {
        if(_isPressedButton)
            _currentString += value;

    }


    private void SwypeButtonRelease()
    {
        ExperimentSample experimentSample = new ExperimentSample();
        experimentSample.character = "RELEASE";
        experimentSample.timestamp = Time.realtimeSinceStartup;
        _currentPressed = false;
        experiment.samples.Add(experimentSample);


        if (_currentString.Length > 0)
        {
            if (_swypeKeyboard.GetSuggestion(_selectedSwipeType, _currentString, _targetWords[_swypeCurrentIndex]))
            {

                ExperimentSample experimentSample2 = new ExperimentSample();
                experimentSample2.character = ("CORRECT,"  + _currentString + "," + _targetWords[_swypeCurrentIndex]);
                experimentSample2.timestamp = Time.realtimeSinceStartup;
                experiment.samples.Add(experimentSample2);


                _audioSource.PlayOneShot(correctBell);
                _currentString = "";

                outputPhraseText.text += " " + _targetWords[_swypeCurrentIndex++];


                if (_swypeCurrentIndex >= _targetWords.Length)
                    EndExperiment();

            }
            else
            {
                ExperimentSample experimentSample2 = new ExperimentSample();
                experimentSample2.character = ("INVALID," + _currentString + "," + _targetWords[_swypeCurrentIndex]);
                experimentSample2.timestamp = Time.realtimeSinceStartup;

                experiment.samples.Add(experimentSample2);


                _currentString = "";
                _audioSource.PlayOneShot(incorrectBell);
            }
        }
    }



    private void InsertStringNormalKeyboard(string value)
    {

        if (value.Equals("UNDO"))
            _currentString = _currentString.Substring(0, _currentString.Length - 1);
        else
            _currentString += value;

        ExperimentSample experimentSample = new ExperimentSample();
        experimentSample.character = value;
        experimentSample.timestamp = Time.realtimeSinceStartup;

        experiment.samples.Add(experimentSample);


        if (_currentString.Equals(_targetString))
        {
            Debug.Log("Ukonczono");
            outputPhraseText.text = "";
            EndExperiment();
        }



        if (_currentString.Equals(_targetString.Substring(0,_currentString.Length)))
        {
            _audioSource.PlayOneShot(correctBell);
        }
        else
        {
            _audioSource.PlayOneShot(incorrectBell);
        }


        outputPhraseText.text = _currentString;
    }

    public void NormalKeyboardMode()
    {
        experimentMode = ExperimentMode.NORMAL_KEYBOARD;
    }

    public void SwypeKeyboardMode()
    {
        experimentMode = ExperimentMode.SWYPE_KEYBOARD;
    }


    public void StartExperiment(string phrase, string participant, string path)
    {

        if(experimentStatus == ExperimentStatus.INACTIVE)
        {
            Debug.Log("Zaczynam eksperyment dla klawiatury: " + experimentMode);
            Debug.Log("Wybrana fraza to: " + phrase);
            Debug.Log("Wybrany uczestnik: " + participant);


            if (participant.Length == 0)
                participant = "unnamed";

            phraseConsole.SetActive(true);
            _targetString = phrase.ToUpper();
            _targetWords = _targetString.Split();
            _path = path;


            int fontSize = Mathf.Clamp(110 - _targetString.Length, 50, 100);

            phraseText.text = _targetString;
            phraseText.fontSize = fontSize;




            _swypeCurrentIndex = 0;
            _currentString = "";

            outputPhraseText.text = _currentString;
            outputPhraseText.fontSize = fontSize;

            experiment = new Experiment();
            experiment.date = DateTime.Now.ToShortDateString();
            experiment.time = DateTime.Now.ToLongTimeString();
            experiment.participant = participant;

            experiment.phrase = phrase;
            experiment.samples = new List<ExperimentSample>();


           switch (experimentMode)
           {
                case ExperimentMode.NORMAL_KEYBOARD:
                    experimentStatus = ExperimentStatus.ACTIVE_NORMAL_KEYBOARD;
                break;

                case ExperimentMode.SWYPE_KEYBOARD:
                    experimentStatus = ExperimentStatus.ACTIVE_SWYPE;
                break;
           }
                

        }


    }


    private void Update()
    {

        _isPressedButton = Input.GetKey(KeyCode.JoystickButton15);
        _isButtonReleased = Input.GetKeyUp(KeyCode.JoystickButton15);


        if (_isPressedButton && experimentStatus == ExperimentStatus.ACTIVE_SWYPE)
        {

            if (!_currentPressed)
            {
                _currentPressed = true;
                ExperimentSample experimentSample = new ExperimentSample();
                experimentSample.character = "PRESSED";
                experimentSample.timestamp = Time.realtimeSinceStartup;
                experiment.samples.Add(experimentSample);
            }
        }

        if(_isButtonReleased && experimentStatus == ExperimentStatus.ACTIVE_SWYPE)
            SwypeButtonRelease();

    }


}