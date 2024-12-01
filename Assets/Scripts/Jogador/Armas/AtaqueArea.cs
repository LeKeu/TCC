using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueArea : MonoBehaviour
{
    [SerializeField] GameObject atqAreaPrefab;
    [SerializeField] int AtqAreaCoolDown;
    [SerializeField] int danoAtq;
    [SerializeField] float alcanceExpl = 3f;
    public bool estaAtacando;

    TremerCamera tremerCamera;

    private void Start()
    {
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
    }

    public void CriarAtaqueArea()
    {
        if (!estaAtacando)  // se não está atacando
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, alcanceExpl);
            Instantiate(atqAreaPrefab, transform.position, Quaternion.identity);
            tremerCamera.TremerCameraFunc();


            foreach (Collider2D c in colliders)
            {
                if (c.GetComponent<InimigoVida>())
                {
                    c.GetComponent<InimigoVida>().ReceberDano(danoAtq);
                }
            }
            StartCoroutine(AtaqueAreaRoutine());
        }
        
    }

    IEnumerator AtaqueAreaRoutine()
    {
        estaAtacando = true;
        atqAreaPrefab.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        atqAreaPrefab.SetActive(false);
        yield return new WaitForSeconds(AtqAreaCoolDown);
        estaAtacando = false;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcanceExpl);
    }


}
