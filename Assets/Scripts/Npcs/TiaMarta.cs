using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TiaMarta : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    string nome = "Dona Marta";
        int aaa = 0;
    [SerializeField] private Sprite perfil;

    public override void Interagir()
    {
        //var aux = dt[indexAtual].paragrafos[aaa].ToString().Split("_");
        //string[] auxDt = new string[1];
        //DialogoTexto omg = new DialogoTexto();
        //if (aux.Length > 1)
        //{
        //    omg.nome = aux[0];
        //    auxDt = new string[] { aux[1] };
        //    omg.paragrafos = auxDt;
        //}


        Falar(dt[indexAtual]);
        aaa++;
        //Debug.Log(aaa);
        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            indexAtual++;
            if (indexAtual == dt.Count) { indexAtual = 0; }
        }
        
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        //dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }

    /*
     Debug.Log(dialogoTexto.paragrafos.Length);
            var aux = dialogoTexto.paragrafos[0].ToString().Split("_");
            string[] auxDt = new string[1];
            if (aux.Length > 1)
            {
                NPCNomeTexto.text = aux[0];
                auxDt = new string[] { aux[1] };
            }
            else
            {
                auxDt = dialogoTexto.paragrafos;
            }

            //dtOriginal.perfilNPC = perfil;

            DialogoTexto dtTeste = new DialogoTexto()
            {
                nome = NPCNomeTexto.text,
                perfilNPC = dialogoTexto.perfilNPC,
                paragrafos = dialogoTexto.paragrafos
            };
            Debug.Log("fuck");
     */
}
