using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFX : MonoBehaviour
{
    AudioSource[] audioSource;
    #region Floresta Noite
    [SerializeField] List<AudioClip> ventosAssustadores;
    [SerializeField] AudioClip florestaNoite;
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
        audioSource[0].PlayOneShot(florestaNoite);
        audioSource[0].loop = true;
    }

    public void AssobioSaci()
    {
        audioSource[1].PlayOneShot(saciAssobio);
    }

    public void PararAssobioSaci() => audioSource[1].Stop();
}
