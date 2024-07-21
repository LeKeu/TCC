using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPCs : MonoBehaviour, IInteractble
{
    [SerializeField] private SpriteRenderer _interagirSprite;
    private const float DISTANCIA = 1f;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && DaParaInteragir())
        {
            Interagir();
        }

        if (_interagirSprite.gameObject.activeSelf && !DaParaInteragir())
            _interagirSprite.gameObject.SetActive(false);
        else if (!_interagirSprite.gameObject.activeSelf && DaParaInteragir())
            _interagirSprite.gameObject.SetActive(true);

    }
    public abstract void Interagir();

    bool DaParaInteragir()
    {
        //Debug.Log(Vector2.Distance(JogadorController.Instance.transform.position, transform.position));
        if (Vector2.Distance(JogadorController.Instance.transform.position, transform.position) < DISTANCIA)
            return true;
        else return false;
    }
}
