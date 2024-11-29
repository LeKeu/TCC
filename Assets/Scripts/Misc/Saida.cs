using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saida : MonoBehaviour
{
    [SerializeField] private string proxCena;
    [SerializeField] private string cenaTransicaoNome;

    LuzesCiclo luzesCiclo;

    private void Start()
    {
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>())
        {
            SceneManagement.Instance.SetTransicaoNome(cenaTransicaoNome);
            if (proxCena != "")
                StartCoroutine(MudarCena());
        }
    }

    IEnumerator MudarCena()
    {
        luzesCiclo.MudarCorAmbiente(Color.black, 10f);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(proxCena);
    }
}
