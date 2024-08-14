using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedrinho : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dtTutorial;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    public override void Interagir()
    {
        Falar(dtTutorial[indexAtual]);

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            indexAtual++;
            if (indexAtual == dtTutorial.Count) { indexAtual = 0; }
        }
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }
}
