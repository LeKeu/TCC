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

    #region seq cuca
    [SerializeField] AudioClip musicaCuca;
    [SerializeField] AudioClip coisasCaindo;
    [SerializeField] AudioClip vidroEstoura;
    [SerializeField] AudioClip passosVidro;
    [SerializeField] AudioClip garraCuca;
    [SerializeField] AudioClip meninaCorpoCaindo;
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

    #region seq cuca 
    public void MusicaCuca() => audioSource[1].PlayOneShot(musicaCuca);
    public void PararMusicaCuca() => audioSource[1].Stop();
    public void VidroEstora() => audioSource[1].PlayOneShot(vidroEstoura);
    public void PassosVidro() => audioSource[1].PlayOneShot(passosVidro);
    public void GarraCuca() => audioSource[1].PlayOneShot(garraCuca);
    public void MeninaCaindo() => audioSource[1].PlayOneShot(meninaCorpoCaindo);
    public void CoisasCaindo() => audioSource[1].PlayOneShot(coisasCaindo);
    #endregion

    #region saci
    public void AssobioSaci()
    {
        Debug.Log("assobio");
        audioSource[1].PlayOneShot(saciAssobio);
    }

    public void PararAssobioSaci() => audioSource[1].Stop();
    #endregion

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
