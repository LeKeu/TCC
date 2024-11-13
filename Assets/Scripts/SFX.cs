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

    private void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }

    public void FlorestaNoite()
    {
        audioSource[0].PlayOneShot(florestaNoite);
        audioSource[0].loop = true;
    }
}
