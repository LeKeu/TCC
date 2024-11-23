using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelhaNamiaCelebracao : NPCsCelebracao, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;

    public override void Interagir()
    {
        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (indexAtual == 0) { Prologo.qntdNpcsConversados++; } // se for a primeira vez conversando

            if (!Prologo.aconteceuBriga) indexAtual = 0;
            if (Prologo.aconteceuBriga && indexAtual < 5) indexAtual = 5;

            Debug.Log("count="+dt.Count);
            Debug.Log("indexatual="+indexAtual);
            if (indexAtual == dt.Count && Prologo.aconteceuBriga) indexAtual = 5; 
        }

        if (!JogadorController.Instance.estaAndando)
            Falar(dt[indexAtual]);

        if (JogadorController.Instance.podeMover)
        {
            Debug.Log("aquiuiuiuiu");
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
