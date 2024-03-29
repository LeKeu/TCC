using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoIA : MonoBehaviour
{
    private enum Estado
    {
        Andando
    }

    Estado estado;
    private InimigoPathFinding InimigoPathFinding;

    private void Awake()
    {
        InimigoPathFinding = GetComponent<InimigoPathFinding>();
        estado = Estado.Andando;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AndandoOutLine());
    }

    private IEnumerator AndandoOutLine()
    {
        while(estado == Estado.Andando)
        {
            Vector2 andandoPos = GetAndandoPos();
            InimigoPathFinding.IrPara(andandoPos);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private Vector2 GetAndandoPos()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
