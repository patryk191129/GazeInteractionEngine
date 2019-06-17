using SwipeType;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SwypeKeyboard : MonoBehaviour
{


    public bool GetSuggestion(MatchSwipeType matchSwipeType, string currentString, string matchString)
    {
        if (currentString.Length > 0)
        {


            currentString = currentString.ToLower();

            string targetString = currentString;
            string tmpString = currentString.Substring(0,1);
            matchString = matchString.ToLower();


            for (int i = 1; i < currentString.Length; i++)
            {
                if (!currentString[i].Equals(currentString[i - 1]))
                    tmpString += currentString[i];
            }

            Debug.Log("Input string: " + currentString);
            Debug.Log("For match string: " + targetString);


            if(tmpString.Length == 1)
                return tmpString.Equals(matchString);



            foreach (string x in matchSwipeType.GetSuggestion(targetString, 5))
            {
                Debug.Log("Suggestion: " + x);

                if (matchString.Equals(x.ToLower()))
                    return true;
            }

            return false;
        }
        return false;
    }
}
