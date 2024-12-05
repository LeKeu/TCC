using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedrinho : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    public bool podePegarBola;
    public bool tutCompleto;
    [SerializeField] private Sprite perfil;

    [SerializeField] OQueFazer oQueFazer_script;
    SeuPedro seuPedroScript;

    private void Start()
    {
        seuPedroScript = GameObject.Find("SeuPedro").GetComponent<SeuPedro>();
    }

    public override void Interagir()
    {


        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (tutCompleto)
                indexAtual++;

            if (!seuPedroScript.tutCompleto) indexAtual = 2;

            if (indexAtual == dt.Count & !tutCompleto) { indexAtual = 0; }
            if (indexAtual == dt.Count & tutCompleto) { indexAtual = 1; }
        }

        if(indexAtual == 0) podePegarBola = true;
        if(indexAtual == 1 || indexAtual == 2) podePegarBola = false;

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
        oQueFazer_script.GerenciarQuadroQuest_tutorial(7);
    }
}
