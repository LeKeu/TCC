using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material flashBrancoMaterial;
    [SerializeField] private float tempoDeRestorar = .2f;

    private Material matPadrao;
    private SpriteRenderer spriteRenderer;
    private InimigoVida inimigoVida;

    void Awake()
    {
        inimigoVida = GetComponent<InimigoVida>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        matPadrao = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashBrancoMaterial;
        yield return new WaitForSeconds(tempoDeRestorar);
        spriteRenderer.material = matPadrao;
        inimigoVida.ChecarMorte();
    }
}
