using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Dialogo/Novo Dialogo Container")]

public class DialogoTexto : ScriptableObject
{
    //public string nome; // nome do npc

    [TextArea(5, 10)]
    public string[] paragrafos;

    public Sprite perfilNPC;
}
