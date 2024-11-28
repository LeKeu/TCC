using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenasManip : MonoBehaviour
{
    LuzesCiclo luzesCiclo;

    private void Awake()
    {
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();
        
    }

    void Start()
    {

        if(SceneManager.GetActiveScene().name == "01_comunidade")
        {
            TudoEscuro();
            ClarearTela();
        }

    }

    public void EscurecerTela() => luzesCiclo.MudarCorAmbiente(Color.black, 5);
    public void ClarearTela() => luzesCiclo.MudarCorAmbiente(Color.white, 5);
    public void TudoEscuro() => luzesCiclo.MudarCorAmbiente(Color.black);
}
