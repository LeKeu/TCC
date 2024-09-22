using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtqDiretoJato : MonoBehaviour
{
    float timerTamanho = 0f;
    bool podeRotacionar;

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
        if(podeRotacionar)
            transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime * -2);
    }

    void EsticarBala()
    {
        float x = 1 * timerTamanho * 10;
        if (x <= 50)
            transform.GetChild(0).GetComponent<Transform>().localScale = new Vector3(x, 1, 1);
        else podeRotacionar = true;
    }

    public void DestruirArea() => Destroy(gameObject);

    public bool CompletouRot()
    {
        if (transform.rotation.z > 0)
            return true;
        return false;
    } 
}
