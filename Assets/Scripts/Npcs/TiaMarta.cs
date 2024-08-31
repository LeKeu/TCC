using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TiaMarta : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    public bool tutCompleto;
    [SerializeField] private Sprite perfil;

    VelhaNamia VelhaNamiaScript;

    private void Start()
    {
        VelhaNamiaScript = GameObject.Find("VelhaNamia").GetComponent<VelhaNamia>();
    }

    public override void Interagir()
    {
        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (!VelhaNamiaScript.tutCompleto)
                indexAtual = 0;// enqnt tut da velhaNamie n estiver completo, fica no txt 0 (pedindo p completar)
            else
            {
                if (tutCompleto) // se o tut da DonaMarta e da velhaNamia estiverem completo, pode seguir p outros txts
                    indexAtual++;
                else
                {
                    indexAtual = 1; // se o tut da velhanamia estiver completo mas o da donamarta não, fica no txt pos 1, que é explicando o que fazer
                    CompletarTutorial();
                }
            }
            
            if (indexAtual == dt.Count && tutCompleto) { indexAtual = 2; } // fica rodando entre os textos não relacionados a completar o tut
        }
        if (!JogadorController.Instance.estaAndando)
            Falar(dt[indexAtual]);

    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }

    void CompletarTutorial()
    {
        tutCompleto = true;
        Debug.Log("entregando ervas");
    }
}
