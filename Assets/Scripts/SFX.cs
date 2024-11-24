using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFX : MonoBehaviour
{
    AudioSource[] audioSource;

    #region Floresta Noite
    [Header("Floresta Noite")]
    [SerializeField] List<AudioClip> ventosAssustadores;
    [SerializeField] AudioClip florestaNoite;
    #endregion

    #region Comunidade
    [Header("Comunidade")]
    [SerializeField] AudioClip comunidadeFloresta;
    [SerializeField] AudioClip comunidadeSino;
    #endregion

    #region Jogador
    [Header("Jogador")]
    [SerializeField] AudioClip jogadorPassosGrama;
    [SerializeField] AudioClip jogadorPassosMadeira;
    [SerializeField] List<AudioClip> jogadorEspadaAtaques;
    #endregion

    #region Saci
    [SerializeField] AudioClip saciAssobio;
    #endregion

    private void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }

    public void FlorestaNoite()
    {
        Debug.Log("floresta");
        audioSource[0].PlayOneShot(florestaNoite);
        audioSource[0].loop = true;
    }

    public void ComunidadeFloresta()
    {
        Debug.Log(audioSource[0].isPlaying);
        if (audioSource[0].isPlaying)
        {
            audioSource[0].PlayOneShot(comunidadeFloresta);
            audioSource[0].loop = true;
        }
    }

    public void ComunidadeSino()
    {
        audioSource[1].PlayOneShot(comunidadeSino);
    }

    public void AssobioSaci()
    {
        Debug.Log("assobio");
        audioSource[1].PlayOneShot(saciAssobio);
    }

    public void PararAssobioSaci() => audioSource[1].Stop();

    #region Jogador

    #region Passos grama
    public void JogadorPassosGrama()
    {
        if (!audioSource[1].isPlaying)
        {
            audioSource[1].PlayOneShot(jogadorPassosGrama);
            audioSource[1].loop = true;
        }
    }
    public void PararJogadorPassosGrama() => audioSource[1].Stop();
    #endregion

    #region Passos Madeira
    public void JogadorPassosMadeira()
    {
        audioSource[1].PlayOneShot(jogadorPassosMadeira);
        audioSource[1].loop = true;
    }
    public void PararJogadorPassosMadeira() => audioSource[1].Stop();
    #endregion

    #region Ataques espada
    public void JogadorEspadaAtaques()
    {
        audioSource[2].PlayOneShot(jogadorEspadaAtaques[Random.Range(0, jogadorEspadaAtaques.Count)]);
    }
    #endregion

    #endregion
}
