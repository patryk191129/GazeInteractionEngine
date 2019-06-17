using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;


public class UIExperimentMenu : MonoBehaviour
{


    public string[] phraseDictionaries;
    public Dropdown phrasesDropdown;
    public Dropdown phrasesInsideDropdown;

    public InputField participant;
    public InputField repeatsInputField;

    private struct Phrases
    {
        public string name;
        public List<string> phraseList;
    }

    List<Phrases> phrasesList;



    private void InsertNewPhraseList(string path)
    {

        StreamReader file = new StreamReader(path);
        string line = "";


        Phrases currentPhrase = new Phrases();
        currentPhrase.name = Path.GetFileNameWithoutExtension(path);
        currentPhrase.phraseList = new List<string>();

        while((line = file.ReadLine()) != null)
        {
            currentPhrase.phraseList.Add(line);
        }

        phrasesList.Add(currentPhrase);
        file.Close();
        UpdateDropdown();

    }


    private void UpdateDropdown()
    {
        phrasesDropdown.options.Clear();

        foreach (Phrases currentPhrase in phrasesList)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = currentPhrase.name;
            phrasesDropdown.options.Add(optionData);
        }

        UpdatePhraseInside();

    }


    public void UpdatePhraseInside()
    {
        Debug.Log("KK");
        phrasesInsideDropdown.options.Clear();

        Phrases currentPhrase = phrasesList[phrasesDropdown.value];

        foreach (string currentString in currentPhrase.phraseList)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = currentString;
            phrasesInsideDropdown.options.Add(optionData);
        }

        phrasesInsideDropdown.value = 0;

    }



    // Start is called before the first frame update
    void Start()
    {
        phrasesList = new List<Phrases>();


        foreach (string currentPhrase in phraseDictionaries)
            InsertNewPhraseList(currentPhrase);

    }


    public void InsertNewPhraseList()
    {

        string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "");

        if (path.Length != 0)
        {
            try
            {
                InsertNewPhraseList(path);
            }
            catch
            {
                EditorUtility.DisplayDialog("BŁĄD", "NIEPOPRAWNY PLIK", "OK");
            }
        }


    }


    public void StartExperiment()
    {

        if (ExperimentEngine.instance.IsInactive())
        {
            string path = EditorUtility.SaveFilePanel("Zapisz eksperyment", "", "", "csv");
            ExperimentEngine.instance.StartExperiment(phrasesList[phrasesDropdown.value].phraseList[phrasesInsideDropdown.value], participant.text, path);
        }

    }


    public void StartExperiment(string path, string phrase)
    {

        if (ExperimentEngine.instance.IsInactive())
            ExperimentEngine.instance.StartExperiment(phrase, participant.text, path);

    }



    public void StartExperimentWithRandomPhrases()
    {
        if(ExperimentEngine.instance.IsInactive())
        {
            string path = EditorUtility.SaveFilePanel("Zapisz eksperyment", "", "", "csv");
            StartCoroutine(Experiment(path, int.Parse(repeatsInputField.text)));
        }
    }


    public void AbortExperiment()
    {

        if(!ExperimentEngine.instance.IsInactive())
        {
            ExperimentEngine.instance.Abort();
            StopAllCoroutines();
        }

    }



    private IEnumerator Experiment(string path, int iterations)
    {

        List<string> testPhrases = new List<string>(phrasesList[phrasesDropdown.value].phraseList);

        while(testPhrases.Count > iterations)
        {
            int randomIdx = UnityEngine.Random.Range(0, testPhrases.Count);
            testPhrases.RemoveAt(randomIdx);
        }


        while(testPhrases.Count > 0)
        {

            if(ExperimentEngine.instance.IsInactive())
            {
                int randomIdx = UnityEngine.Random.Range(0, testPhrases.Count-1);
                string phrase = testPhrases[randomIdx];
                testPhrases.RemoveAt(randomIdx);


                StartExperiment(path, phrase);


            }

            yield return new WaitForSeconds(0.2f);
        }       
    }


    private void Update()
    {
        
        if(Input.GetKey(KeyCode.F12))
        {
            if(ExperimentEngine.instance.IsInactive())
                StartCoroutine(Experiment("D:/Wyniki/wynik.csv", 4));
        }

        if (Input.GetKey(KeyCode.Q))
            ExperimentEngine.instance.Abort();


    }




}
