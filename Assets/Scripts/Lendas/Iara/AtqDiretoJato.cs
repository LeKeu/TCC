using UnityEngine;

public class AtqDiretoJato : MonoBehaviour
{
    float timerTamanho = 0f;
    bool podeRotacionar;

    //int qntdRot = 0;

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
            transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 50);
    }

    void EsticarBala()
    {
        float x = 1 * timerTamanho * 50;

        if (x <= 50)
        {
            transform.GetChild(0).GetComponent<Transform>().localScale = new Vector3(x, 1, 1);
            transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(x, 1, 1);
        }
        else podeRotacionar = true;
    }

    public void DestruirArea() => Destroy(gameObject);

    public bool CompletouRot()
    {
        if (transform.localRotation.eulerAngles.z >= 359)
            return true;
        return false;
    } 
}
