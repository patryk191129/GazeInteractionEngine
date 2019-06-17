using SwipeType;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SwypeManager : MonoBehaviour
{

    public string dictionary;


    // Start is called before the first frame update
    void Start()
    {
        var swype = new MatchSwipeType(File.ReadAllLines(dictionary)); // File with a list of words
        string testCases = "";


        foreach (var x in swype.GetSuggestion(testCases, 2))
        {
            Debug.Log(x);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
