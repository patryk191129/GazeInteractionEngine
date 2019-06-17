using SwipeType;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class Keyboard : MonoBehaviour
{

    public enum KEYBOARD_MODE
    {
        classic,
        swipe
    };


    public string dictionary;


    [SerializeField]
    string equalValue;

    [SerializeField]
    KEYBOARD_MODE keyboardMode;


    MatchSwipeType matchSwipeType;

    string _currentText;
    bool isEnabled = true;


    public Text outputBox;


    public string outputText;


    private void Validate()
    {
        
    }


    private void Start()
    {
        matchSwipeType = new MatchSwipeType(File.ReadAllLines(dictionary)); // File with a list of words
    }

    public void InsertChar(string value)
    {

        if(isEnabled)
        {
            if (outputText.Length == 0 || outputText[outputText.Length - 1].Equals(value))
            {
                StringBuilder sb1 = new StringBuilder(outputText);
                sb1.Append(value);
                outputText = sb1.ToString();
            }
        }


        string val = outputBox.text;

        StringBuilder sb = new StringBuilder(val);
        sb.Append(value);

        outputBox.text = sb.ToString();

    }
    
    public void ClearValues()
    {
        outputBox.text = "";
    }



    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("KEYDOWN");
            isEnabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Debug.Log("KEYUP");
            isEnabled = false;
            StartCoroutine(Swype());
        }


    }


    public IEnumerator Swype()
    {

        foreach (var x in matchSwipeType.GetSuggestion(outputText, 2))
        {
            Debug.Log(x);
        }

        outputText = "";


        yield return null;
    }



}
