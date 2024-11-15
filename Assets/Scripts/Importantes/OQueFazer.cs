using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OQueFazer : MonoBehaviour
{
    [SerializeField] GameObject QuadroDeQuests;

    public List<string> tutorial = new List<string>() 
    { "Fale com a velha nâmia", "Vá com a Tia Marta", "Talk com o seu Pedro", "Ajude o Pedrinho", "Fale com o homem Peixe" };

    public List<string> celebr_seq = new List<string>()
    { "Fale com o povo ao redor antes de começar a celebração", "Participe da celebração", "Procure seu irmão", "Saia da floresta escura" };

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "01_comunidade")
            QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = tutorial[0];

        if (SceneManager.GetActiveScene().name == "02_comunidade")
            QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = celebr_seq[0];
    }

    public void GerenciarQuadroQuest_tutorial(int index) 
        => QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = tutorial[index];

    public void GerenciarQuadroQuest_celebr_seq(int index)
        => QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = celebr_seq[index];

    public void AtivarPainelQuests(bool acao) => QuadroDeQuests.SetActive(acao);
}
