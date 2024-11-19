using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject canvasPainel;
    public bool duranteTutorial;
    KeyCode teclaTutorial;


    private void Update()
    {
        if(duranteTutorial && Input.GetKeyDown(teclaTutorial))
            PararTutorial();
    }

    public void IniciarTutorial_PararTempo(string texto, KeyCode tecla)
    {
        canvasPainel.SetActive(true);
        duranteTutorial = true;
        teclaTutorial = tecla;
        Time.timeScale = 0;

        canvasPainel.GetComponentInChildren<TextMeshProUGUI>().text = texto;
    }

    void PararTutorial()
    {
        canvasPainel.SetActive(false);
        duranteTutorial = false;
        Time.timeScale = 1;
    }
}
