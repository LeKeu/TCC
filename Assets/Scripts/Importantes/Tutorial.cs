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
        if (duranteTutorial && Input.GetKeyDown(teclaTutorial))
            PararTutorial();
        //if (duranteTutorial)
        //{
        //    if (Input.GetKeyDown(teclaTutorial))
        //        PararTutorial();
        //    else
        //        return;
        //}
    }

    public void IniciarTutorial_PararTempo(string texto, KeyCode tecla)
    {
        JogadorController.Instance.podeFalar = false;
        Debug.Log("tutorial");
        gameObject.SetActive(true);
        duranteTutorial = true;
        teclaTutorial = tecla;
        Time.timeScale = 0;

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = texto;
    }

    void PararTutorial()
    {
        JogadorController.Instance.podeFalar = true;
        gameObject.SetActive(false);
        duranteTutorial = false;
        Time.timeScale = 1;
    }
}
