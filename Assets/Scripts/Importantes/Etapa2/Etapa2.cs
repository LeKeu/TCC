using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Etapa2 : MonoBehaviour
{
    [SerializeField] GameObject Saci;

    [Header("Audios Geral")]
    [SerializeField] SFX sfx_script;


    #region Primeiro Encontro Saci
    [SerializeField] Transform posMenina_ConversaSaci;
    [SerializeField] AudioClip SaciAssobio;

    bool aconteceuEncontro;
    bool meninaAndando_EncontroSaci;
    #endregion
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "01_saci")
        {
            JogadorController.Instance.velocidade = 1;
            sfx_script.FlorestaNoite();
            Saci.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Debug.Log(JogadorController.Instance.velocidade);  
        if(Etapas.PrimeiroEncontroSaci && this.gameObject.name == "PrimeiroEncontroSaci")
        {
            if(meninaAndando_EncontroSaci)
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posMenina_ConversaSaci.position, 1 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<JogadorController>() && gameObject.name == "PrimeiroEncontroSaci" && !aconteceuEncontro)
        {
            StartCoroutine(PrimeiroEncontroSaci());
        }
    }

    IEnumerator PrimeiroEncontroSaci()
    {
        Etapas.PrimeiroEncontroSaci = true;
        #region  menina andando e escuta assobio
        meninaAndando_EncontroSaci = true;

        sfx_script.AssobioSaci();
        yield return new WaitForSeconds(2);
        meninaAndando_EncontroSaci = false;
        MudarEstadoJogador(false);
        #endregion

        #region encontrando o saci
        sfx_script.PararAssobioSaci();
        Saci.SetActive(true);
        yield return new WaitForSeconds(3);
        Interagir_Geral(Saci.GetComponent<SaciDialog>(), 0);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region inicio batalha saci
        Saci.GetComponent<Saci>().IniciarBatalha_primeiroEncontroSaci();
        #endregion

        MudarEstadoJogador(true);
        aconteceuEncontro = true;
        Etapas.PrimeiroEncontroSaci = false;
        JogadorController.Instance.velocidade = 4f;
    }

    void Interagir_Geral(MonoBehaviour script, int index, string metodoNome = "Interagir")
    {
        object[] parametros = { index };
        //Debug.Log(script.name);
        var metodo = script.GetType().GetMethod($"{metodoNome}");
        metodo.Invoke(script, parametros);
    }
    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
        JogadorController.Instance.estaDuranteCutscene = !acao;
        //JogadorController.Instance.podeFlipX = !acao;
    }
}
