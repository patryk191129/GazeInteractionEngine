using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : MonoBehaviour
{

    public AudioClip buttonPressed;
    public AudioClip buttonHover;

    public string word;

    public void OnButtonPressed()
    {
        if(buttonPressed)
            this.GetComponent<AudioSource>().PlayOneShot(buttonPressed);
        ExperimentEngine.instance.InsertString(word);
    }

    public void OnButtonHover()
    {
        if(buttonHover)
            this.GetComponent<AudioSource>().PlayOneShot(buttonHover);
    }

    public void ButtonUndo()
    {
        ExperimentEngine.instance.InsertString("UNDO");
    }



}
