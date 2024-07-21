using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiaMarta : NPCs, ITalkable
{
    [SerializeField] private DialogoTexto dialogoTexto;
    [SerializeField] private DialogoController dialogoController;

    public override void Interagir()
    { 
        Falar(dialogoTexto);
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }
}
