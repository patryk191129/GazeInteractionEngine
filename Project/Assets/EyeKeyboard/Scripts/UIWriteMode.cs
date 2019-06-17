using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriteMode : MonoBehaviour
{

    public Text writingMode;

    public GameObject swypeKeyboard;
    public GameObject normalKeyboard;


    private void Start()
    {
        NormalMode();
    }

    public void SwypeMode()
    {
        ExperimentEngine.instance.SwypeKeyboardMode();
        swypeKeyboard.SetActive(true);
        normalKeyboard.SetActive(false);
        writingMode.text = "Swype";
    }

    public void NormalMode()
    {
        ExperimentEngine.instance.NormalKeyboardMode();
        swypeKeyboard.SetActive(false);
        normalKeyboard.SetActive(true);
        writingMode.text = "Domyślna klawiatura";
    }


    public void Update()
    {
        if (Input.GetKey(KeyCode.F9))
            NormalMode();

        if (Input.GetKey(KeyCode.F10))
            SwypeMode();



    }

}
