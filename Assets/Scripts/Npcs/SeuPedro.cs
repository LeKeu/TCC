using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeuPedro : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    public bool tutCompleto;
    [SerializeField] private Sprite perfil;

    [SerializeField] OQueFazer oQueFazer_script;
    TiaMarta TiaMartaScript;

    private void Start()
    {
        TiaMartaScript = GameObject.Find("DonaMarta").GetComponent<TiaMarta>();
    }

    public override void Interagir()
    {
        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (!TiaMartaScript.tutCompleto)
                indexAtual = 0;// enqnt tut da TiaMarta n estiver completo, fica no txt 0 (pedindo p completar)
            else
            {
                if (tutCompleto) // se o tut da TiaMarta e do SeuPedro estiverem completo, pode seguir p outros txts
                    indexAtual++;
                else
                {
                    indexAtual = 1; // se o tut da TiaMarta estiver completo mas o da donamarta não, fica no txt pos 1, que é explicando o que fazer
                    CompletarTutorial();
                }
            }

            if (indexAtual == dt.Count && tutCompleto) { indexAtual = 2; } // fica rodando entre os textos não relacionados a completar o tut
        }

        if(!JogadorController.Instance.estaAndando)
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
        tutCompleto = true;
        oQueFazer_script.GerenciarQuadroQuest_tutorial(5);
        //Debug.Log("seu pedro OK");
    }
}