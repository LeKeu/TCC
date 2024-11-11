using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorDialogo : Singleton<JogadorDialogo>
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;

    public void Interagir_CelebracaoCutscene(int index = 0)
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
