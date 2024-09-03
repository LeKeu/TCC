using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaBosses : MonoBehaviour
{
    [SerializeField] int VidaMaxSlider;
    Slider vidaSlider;
    int vidaAtual;

    //public string NomeBoss;
    [SerializeField] GameObject ContainerSliderBoss;
    [SerializeField] TextMeshProUGUI nomeText;

    public void CriarContainer(int VidaMax, string Nome)
    {
        ContainerSliderBoss.SetActive(true);
        VidaMaxSlider = VidaMax;
        vidaAtual = VidaMax;
        nomeText.text = Nome;
    }

    public void DesativarContainer() => ContainerSliderBoss.SetActive(false);

    public void ReceberDano(int dano)
    {
        if(vidaAtual > 0) {
            vidaAtual -= dano;
            AtualizarVida();
        }
        else {
            vidaAtual = 0;
            Debug.Log("arrasta p cima");
        }
    }

    void AtualizarVida()
    {
        if (vidaSlider == null)
            vidaSlider = GameObject.Find("VidaSliderBosses").GetComponent<Slider>();

        vidaSlider.maxValue = VidaMaxSlider;
        vidaSlider.value = vidaAtual;
    }

    public bool ContainerEstaAtivo() => ContainerSliderBoss.activeSelf;
}
