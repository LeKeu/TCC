using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCNomeTexto;
    [SerializeField] private TextMeshProUGUI NPCDialogoTexto;
    [SerializeField] private Image NPCPerfil;
    [SerializeField] private Image JOGADORPerfil;
    private float digitarVel = 5f;

    private Queue<string> paragrafos = new Queue<string>();

    bool conversaAcabou;
    bool estaDigitando;
    string p;

    private Coroutine digitandoDialogoCoroutine;

    const string HTML_ALPHA = "<color=#00000000>";
    const float MAX_DIGITAR_TEMPO = 0.1f;

    public void DisplayProximoParagrafo(DialogoTexto dialogoTexto)
    {
        if(paragrafos.Count == 0)
        {
            if (!conversaAcabou)
            {
                JogadorController.Instance.acabouDialogo = false;
                IniciarConversa(dialogoTexto);
            }
            else if(conversaAcabou && !estaDigitando)
            {
                AcabarConversa();
                return;
            }
        }

        if (!estaDigitando)
        {
            p = paragrafos.Dequeue();
            //Debug.Log("dequeue="+p);
            digitandoDialogoCoroutine = StartCoroutine(DigitarDialogoTexto(p));
        }
        else
        {
            AcabarParagrafoCedo();
        }
        
        //NPCDialogoTexto.text = p;

        if(paragrafos.Count == 0)
        {
            conversaAcabou = true;
            //JogadorController.Instance.podeMover = true;
        }
    }

    private IEnumerator DigitarDialogoTexto(string p)
    {
        var aux = p.Split('_'); //essa parte quebra o paragrafo, separando no nome e texto
        string nomeFalando = aux[0];
        p = aux[1];

        if (nomeFalando.Length <= 1) nomeFalando = "Aila"; // se antes do "_" for vazio, substituir com "Menina"
        NPCNomeTexto.text = nomeFalando;

        if(nomeFalando != "Aila") // colocando o perfil dp npc da vez
            NPCPerfil.sprite = AUX_NPCS_RESOURCES.perfilsNPCs[EncontrarIndexSprite(nomeFalando)];

        if (aux[0].Trim() == nomeFalando) // mudança de sprite dependendo de qual personagem estiver falando
        {
            JOGADORPerfil.GetComponent<Image>().color = Color.grey;
            NPCPerfil.GetComponent<Image>().color = Color.white;
        }
        else
        {
            JOGADORPerfil.GetComponent<Image>().color = Color.white;
            NPCPerfil.GetComponent<Image>().color = Color.grey;
        }

        estaDigitando = true;
        NPCDialogoTexto.text = "";

        string textoOriginal = p;
        string textoDisplayed = "";
        int alphaIndex = 0;

        foreach(char c in p.ToCharArray())
        {
            alphaIndex++;
            NPCDialogoTexto.text = textoOriginal;

            textoDisplayed = NPCDialogoTexto.text.Insert(alphaIndex, HTML_ALPHA);
            NPCDialogoTexto.text = textoDisplayed;

            yield return new WaitForSeconds(MAX_DIGITAR_TEMPO/digitarVel);
        }

        estaDigitando = false;
    }

    int EncontrarIndexSprite(string nomeNPC)
    {
        //Debug.Log("NOME NPC" + nomeNPC);
        for (int i = 0; i < AUX_NPCS_RESOURCES.perfilsNPCs.Length; i++)
        {
            //Debug.Log("AUX_NPCS_RESOURCES.perfilsNPCs[i].name=" + AUX_NPCS_RESOURCES.perfilsNPCs[i].name);
            if (AUX_NPCS_RESOURCES.perfilsNPCs[i].name == nomeNPC)
                return i;
        }
        return -1;
    }

    private void AcabarParagrafoCedo()
    {
        StopCoroutine(digitandoDialogoCoroutine);

        var aux = p.Split('_');
        NPCDialogoTexto.text = aux[1];
        estaDigitando = false;
    }

    private void IniciarConversa(DialogoTexto dialogoTexto)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        //NPCNomeTexto.text = dialogoTexto.nome;
        NPCPerfil.sprite = dialogoTexto.perfilNPC;
        JOGADORPerfil.sprite = JogadorController.Instance.perfil;

        for(int i = 0; i < dialogoTexto.paragrafos.Length; i++)
        {
            //Debug.Log(dialogoTexto.paragrafos[i]);
            paragrafos.Enqueue(dialogoTexto.paragrafos[i]);
        }

    }

    private void AcabarConversa()
    {
        JogadorController.Instance.acabouDialogo = true;

        conversaAcabou = false;
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
