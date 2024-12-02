using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OQueFazer : MonoBehaviour
{
    [SerializeField] GameObject QuadroDeQuests;

    List<string> tutorial = new List<string>() 
    { "Fale com a Velha N�mia.", "V� com a Tia Marta.", "Fale com o Seu Pedro.", "Ajude o Pedrinho.", "Pegue seu almo�o com a Wanda." };

    List<string> celebr_seq = new List<string>()
    { "Fale com o povo ao redor antes de come�ar a celebra��o", "Procure pela bola de futebol perto das casas", "V� para sua casa, a telha dela � azul", "Siga os rastros do vulto." };

    List<string> saci_cenas = new List<string>()
    { "Siga os rastros e encontre seu irm�o.", "Sobreviva ao encontro!", "Explore a floresta e tente encontrar seu irm�o.", "saci 4" };

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "01_comunidade")
            QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = tutorial[0];

        if (SceneManager.GetActiveScene().name == "02_comunidade")
            QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = celebr_seq[0];

        if (SceneManager.GetActiveScene().name == "01_saci")
            QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = saci_cenas[0];

        if (SceneManager.GetActiveScene().name == "03_saci") // BOSS SACI
            QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = saci_cenas[0];
    }

    public void GerenciarQuadroQuest_tutorial(int index) 
        => QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = tutorial[index];

    public void GerenciarQuadroQuest_celebr_seq(int index)
        => QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = celebr_seq[index];

    public void GerenciarQuadroQuest_saci_cenas(int index)
        => QuadroDeQuests.GetComponentInChildren<TextMeshProUGUI>().text = saci_cenas[index];

    public void AtivarPainelQuests(bool acao) => QuadroDeQuests.SetActive(acao);
}
