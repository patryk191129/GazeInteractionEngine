using SwipeType;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UISwypeMenu : MonoBehaviour
{

    public string[] swypeDictionaries;
    public Dropdown dictionaryDropdown;


    private struct SwypeInfo
    {
        public string name;
        public MatchSwipeType swipeType;

    }

    private List<SwypeInfo> swypeInfo;

    private void InsertNewDictionary(string path)
    {
        SwypeInfo currentswypeInfo = new SwypeInfo();
        currentswypeInfo.swipeType = new MatchSwipeType(File.ReadAllLines(path));
        currentswypeInfo.name = Path.GetFileNameWithoutExtension(path);

        Debug.Log("Loaded " + currentswypeInfo.name);
        swypeInfo.Add(currentswypeInfo);

        UpdateDropdown();
        
    }


    private void UpdateDropdown()
    {
        dictionaryDropdown.options.Clear();

        foreach(SwypeInfo currentSwype in swypeInfo)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = currentSwype.name;
            dictionaryDropdown.options.Add(optionData);
        }    
    }


    public void InsertNewDictionary()
    {

        string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "");

        if (path.Length != 0)
        {
            try
            {
                InsertNewDictionary(path);
            }
            catch
            {
                EditorUtility.DisplayDialog("BŁĄD", "NIEPOPRAWNY PLIK", "OK");
            }
        }


    }


    private void Start()
    {
        swypeInfo = new List<SwypeInfo>();


        foreach (string currentDictionary in swypeDictionaries)
            InsertNewDictionary(currentDictionary);

        SetSwypeDictionary();
    }


    public void SetSwypeDictionary()
    {
        int value = dictionaryDropdown.value;
        ExperimentEngine.instance.SetSwipeType(swypeInfo[value].swipeType);
    }







}
