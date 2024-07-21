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

    private Queue<string> paragrafos = new Queue<string>();

    public bool conversaAcabou;
    string p;

    public void DisplayProximoParagrafo(DialogoTexto dialogoTexto)
    {
        if(paragrafos.Count == 0)
        {
            if (!conversaAcabou)
            {
                JogadorController.Instance.podeMover = false;
                IniciarConversa(dialogoTexto);
            }
            else
            {
                AcabarConversa();
                return;
            }
        }

        p = paragrafos.Dequeue();
        NPCDialogoTexto.text = p;

        if(paragrafos.Count == 0)
        {
            conversaAcabou = true;
            JogadorController.Instance.podeMover = true;
        }
    }

    private void IniciarConversa(DialogoTexto dialogoTexto)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        NPCNomeTexto.text = dialogoTexto.nome;
        NPCPerfil.sprite = dialogoTexto.perfil;
        for(int i = 0; i < dialogoTexto.paragrafos.Length; i++)
        {
            Debug.Log(dialogoTexto.paragrafos[i]);
            paragrafos.Enqueue(dialogoTexto.paragrafos[i]);
        }

    }

    private void AcabarConversa()
    {
        conversaAcabou = false;
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
