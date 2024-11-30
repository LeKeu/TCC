using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogosGerais : MonoBehaviour
{
    [SerializeField] private List<DialogoTexto> dt;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && Prologo.procurandoBola && !JogadorController.Instance.acabouDialogo)
            Interagir_CelebracaoCutscene();

    }

    public  void Interagir()
    {
        

        if (!JogadorController.Instance.estaAndando)
            Falar(dt[indexAtual]);
    }

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