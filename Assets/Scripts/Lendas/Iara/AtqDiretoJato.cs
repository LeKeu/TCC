using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtqDiretoJato : MonoBehaviour
{
    float timerTamanho = 0f;

    private void Update()
    {
        AtaqueDireto();
        timerTamanho += Time.deltaTime;
    }

    void AtaqueDireto()
    {
        Rotacionar(); EsticarBala();
    }

    void Rotacionar()
    {
        transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime * -2);
    }

    void EsticarBala()
    {
        var teste = new Vector3(1 * timerTamanho * 10, 1, 1);
        Debug.Log(teste);
        transform.GetChild(0).GetComponent<Transform>().localScale = teste;
    }

    public void DestruirArea() => Destroy(gameObject);

    public bool CompletouRot()
    {
        if (transform.rotation.z > 0)
            return true;
        return false;
    } 
}
