using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaciDialog : MonoBehaviour
{
    #region Parametros de Dialogo
    [Header("Dialogo")]
    [SerializeField] Sprite perfil;
    [SerializeField] private DialogoController dialogoController;
    [SerializeField] private List<DialogoTexto> dt;
    int indexAtual = 0;
    #endregion

    private void Update()
    {
        if (Mouse.current.middleButton.wasPressedThisFrame && Etapas.PrimeiroEncontroSaci)
            Interagir();
    }

    //public void Interagir()
    //{
    //    Falar(dt[indexAtual]);
    //}
    public void Interagir(int index = 0)
    {
        Falar(dt[index]);
    }

    public void Falar(DialogoTexto dialogoTexto)
    {
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }
}
