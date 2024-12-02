using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFX : MonoBehaviour
{
    AudioSource[] audioSource;

    [SerializeField] AudioClip chuva;

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

    [SerializeField] List<AudioClip> jogadoraMachucada;
    [SerializeField] List<AudioClip> jogadoraAtacando;
    bool chamouPararPassos;
    bool chamouMachucada;
    #endregion

    #region Saci
    [SerializeField] AudioClip saciAssobio;
    [SerializeField] AudioClip saciVentonia;
    [SerializeField] AudioClip saciVentoForte;
    #endregion

    #region seq cuca
    [SerializeField] AudioClip musicaCuca;
    [SerializeField] AudioClip coisasCaindo;
    [SerializeField] AudioClip vidroEstoura;
    [SerializeField] AudioClip passosVidro;
    [SerializeField] AudioClip garraCuca;
    [SerializeField] AudioClip meninaCorpoCaindo;
    #endregion

    #region combate
    [Header("Combate")]
    [SerializeField] AudioClip musicaCombate;
    [SerializeField] AudioClip purificar;
    #endregion

    private void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }

    private void Update()
    {
        if (JogadorVida.levouDano && !chamouMachucada)
            JogadorMachucada();
        if(!JogadorVida.levouDano)
            chamouMachucada = false;
        //PASSOS GRAMA
        //if (JogadorController.Instance.estaAndando)
        //    JogadorPassosGrama();

        //if (!JogadorController.Instance.estaAndando && !chamouPararPassos)
        //    PararJogadorPassosGrama();
    }

    #region geral
    public void PararAudioSource03() => audioSource[2].Stop();
    public void TocarAudioSource03() => audioSource[2].Play();

    public void Purificar() => audioSource[3].PlayOneShot(purificar);

    public void Chuva()
    {
        if (!audioSource[1].isPlaying)
        {
            audioSource[1].clip = chuva; 
            audioSource[1].loop = true; 
            audioSource[1].Play();
        }
    }

    public void PararChuva() => audioSource[1].Stop();
    #endregion

    public void FlorestaNoite()
    {
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

    public void MusicaCombate()
    {
        Debug.Log("m combate");
        if (!audioSource[4].isPlaying)
        {
            audioSource[4].clip = musicaCombate;
            audioSource[4].loop = true;
            audioSource[4].Play();
        }
    }

    public void PararMusicaCombate()
    {
        audioSource[4].Stop();      
        //audioSource[4].mute = true;
        //audioSource[4].clip = null;  
        //audioSource[4].loop = false;
        //audioSource[4].enabled = false; 
    }

    public void ComunidadeSino()
    {
        audioSource[1].PlayOneShot(comunidadeSino);
    }

    #region seq cuca 
    public void MusicaCuca() => audioSource[1].PlayOneShot(musicaCuca);
    public void PararMusicaCuca() => audioSource[1].Stop();
    public void VidroEstora() => audioSource[3].PlayOneShot(vidroEstoura);
    public void PassosVidro() => audioSource[1].PlayOneShot(passosVidro);
    public void GarraCuca() => audioSource[1].PlayOneShot(garraCuca);
    public void MeninaCaindo() => audioSource[1].PlayOneShot(meninaCorpoCaindo);
    public void CoisasCaindo() => audioSource[1].PlayOneShot(coisasCaindo);
    #endregion

    #region saci
    public void AssobioSaci() => audioSource[1].PlayOneShot(saciAssobio);
    public void PararAssobioSaci() => audioSource[1].Stop();
    public void VentoniaSaci() => audioSource[1].PlayOneShot(saciVentonia);
    public void VentoForteSaci() => audioSource[1].PlayOneShot(saciVentoForte);
    #endregion

    #region Jogador

    #region Passos grama
    public void JogadorPassosGrama()
    {
        if (!audioSource[1].isPlaying)
        {
            audioSource[1].PlayOneShot(jogadorPassosGrama);
            audioSource[1].loop = true;
            chamouPararPassos = false;
        }
    }
    public void PararJogadorPassosGrama()
    {
        chamouPararPassos = true;
        audioSource[1].Stop();
    }
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

    public void JogadorMachucada()
    {
        chamouMachucada = true;
        audioSource[3].PlayOneShot(jogadoraMachucada[Random.Range(0, jogadoraMachucada.Count)]);
    }

    public void JogadorAtacando()
    {
        audioSource[4].PlayOneShot(jogadoraAtacando[Random.Range(0, jogadoraAtacando.Count)]);
    }
    #endregion

    #endregion
}
