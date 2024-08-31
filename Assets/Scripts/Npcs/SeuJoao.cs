using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SeuJoao : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;

    public bool tutCompleto;
    public bool podePegarBola;
    public bool foiPego;

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (!tutCompleto)
    //    {
    //        if (collision.GetComponent<JogadorController>())
    //        {
    //            if (collision.GetComponent<JogadorController>().estaEscondido)
    //            {
    //                podePegarBola = true;
    //            }
    //            else
    //            {
    //                podePegarBola = false;
    //                foiPego = true;
    //                Debug.Log("Foi pego");
    //            }
    //        }
    //        foiPego = false;
    //    }
        
    //}

    public override void Interagir()
    {

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            indexAtual++;
            if (indexAtual == dt.Count) { indexAtual = 0; }
        }

        if (!JogadorController.Instance.estaAndando)
            Falar(dt[indexAtual]);
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        //dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }

    public void CompletarTutorial()
    {
        tutCompleto = true;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 5f);
    //}
}
