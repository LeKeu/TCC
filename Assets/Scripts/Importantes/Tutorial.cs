using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //[SerializeField] GameObject canvasPainel;
    public bool duranteTutorial;
    KeyCode teclaTutorial;


    private void Update()
    {
        if(duranteTutorial && Input.GetKeyDown(teclaTutorial))
            PararTutorial();
    }

    public void IniciarTutorial_PararTempo(string texto, KeyCode tecla)
    {
        gameObject.SetActive(true);
        duranteTutorial = true;
        teclaTutorial = tecla;
        Time.timeScale = 0;

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = texto;
    }

    void PararTutorial()
    {
        gameObject.SetActive(false);
        duranteTutorial = false;
        Time.timeScale = 1;
    }
}
