using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Etapa2 : MonoBehaviour
{
    [SerializeField] GameObject Saci;
    AudioSource audioSource;

    #region Primeiro Encontro Saci
    [SerializeField] Transform posMenina_ConversaSaci;
    [SerializeField] AudioClip SaciAssobio; 

    bool meninaAndando_EncontroSaci;
    #endregion
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "01_saci")
            JogadorController.Instance.velocidade = 1;  
            
    }

    void FixedUpdate()
    {
        if(Etapas.PrimeiroEncontroSaci && this.gameObject.name == "PrimeiroEncontroSaci")
        {
            if(meninaAndando_EncontroSaci)
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posMenina_ConversaSaci.position, JogadorController.Instance.velocidade * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<JogadorController>() && gameObject.name == "PrimeiroEncontroSaci")
        {
            StartCoroutine(PrimeiroEncontroSaci());
        }
    }

    IEnumerator PrimeiroEncontroSaci()
    {
        #region  menina andando e escuta assobio
        MudarEstadoJogador(false);
        meninaAndando_EncontroSaci = true;
        audioSource.PlayOneShot(SaciAssobio);
        yield return new WaitForSeconds(2);
        meninaAndando_EncontroSaci = false;
        #endregion

    }

    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
        JogadorController.Instance.estaDuranteCutscene = !acao;
        //JogadorController.Instance.podeFlipX = !acao;
    }
}
