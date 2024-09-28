using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agua : MonoBehaviour
{
    [SerializeField] ParticleSystem partRippleAgua;

    //private void Update()
    //{
    //    if (JogadorController.Instance.estaNaAgua && JogadorController.Instance.estaAndando)
    //        StartCoroutine(AguaRippleParticula());
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { collision.GetComponent<JogadorController>().estaNaAgua = true; }
        if(collision.gameObject.tag == "InimigoPadrao" || collision.gameObject.tag == "InimigoAtirador") { collision.GetComponent<InimigoPathFinding>().DiminuirVelocidade(); }
        if (collision.gameObject.tag == "InvocadoInimigo") { collision.GetComponent<InvocadoInimigo>().DiminuirVelocidade(); }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { collision.GetComponent<JogadorController>().estaNaAgua = false; }
        if(collision.gameObject.tag == "InimigoPadrao" || collision.gameObject.tag == "InimigoAtirador") { collision.GetComponent<InimigoPathFinding>().VoltarVelocidadeNormal(); }
        if(collision.gameObject.tag == "InvocadoInimigo") { collision.GetComponent<InvocadoInimigo>().VoltarVelocidadeNormal(); }

    }

    IEnumerator AguaRippleParticula()
    {
        ParticleSystem particula =  Instantiate(partRippleAgua, JogadorController.Instance.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(particula.duration);
        Destroy(particula.gameObject);
    }

}
