using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeuPedroCelebracao : NPCsCelebracao, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;

    public override void Interagir()
    {
        //código geral p caso de: tem apenas 4 dts, 2 antes da briga e 2 depois da briga
        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            //if (!Prologo.aconteceuBriga && indexAtual > 1) indexAtual = 1;
            if (indexAtual == dt.Count && Prologo.aconteceuBriga) indexAtual = 2;

            if (!Prologo.aconteceuBriga)
            {
                if (indexAtual > 1) indexAtual = 0;
            }
            else
            {
                if (indexAtual > 3) indexAtual = 2;
                if (indexAtual < 1) indexAtual = 2;
            }

            if (indexAtual == 0) { Prologo.qntdNpcsConversados++; } // se for a primeira vez conversando
        }

        if (!JogadorController.Instance.estaAndando)
            Falar(dt[indexAtual]);

        if (JogadorController.Instance.podeMover)
        {
            if (indexAtual != dt.Count - 1)
                indexAtual++;
        }
    }

    public override void Interagir_CelebracaoCutscene(int index=0)
    {
        // 0 - primeira vez falando
        // 1 - após briga?
        Falar(dt[index]);
    }

    public void Falar(DialogoTexto dialogoTexto)
    {
        //dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }
}