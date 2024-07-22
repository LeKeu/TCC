using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arco : MonoBehaviour, IArma
{
    public void Atacar()
    {
        Debug.Log("arco");
        ArmaAtiva.Instance.ToggleEstaAtacando(false);
    }
}
