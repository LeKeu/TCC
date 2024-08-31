using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizarQuests : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;
    public bool todosTutCompleto;

    VelhaNamia velhaNamia;
    TiaMarta tiaMarta;
    SeuPedro seuPedro;
    Pedrinho pedrinho;

    private void Start()
    {
        velhaNamia = GameObject.Find("VelhaNamia").GetComponent<VelhaNamia>();
        tiaMarta = GameObject.Find("DonaMarta").GetComponent<TiaMarta>();
        seuPedro = GameObject.Find("SeuPedro").GetComponent<SeuPedro>();
        pedrinho = GameObject.Find("Pedrinho").GetComponent<Pedrinho>();
    }

    public override void Interagir()
    {

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (!todosTutCompleto)
                CompletarTutorial();

            if (todosTutCompleto) // só vai sair do txt de tut qnd tiver completa a 'quest'
                indexAtual++;

            if (indexAtual == dt.Count && !todosTutCompleto) { indexAtual = 0; }
            if (indexAtual == dt.Count && todosTutCompleto) { indexAtual = 1; }; // index 0 sempre será o txt do tut, ent qnd ele estiver completo, não volta mais
        }
        Debug.Log(indexAtual);
        Falar(dt[indexAtual]);
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        //dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }

    void CompletarTutorial()
    {
        if (velhaNamia.tutCompleto && tiaMarta.tutCompleto && seuPedro.tutCompleto && pedrinho.tutCompleto)
        { todosTutCompleto = true;  Debug.Log("tudo completo"); }
    }
}
