using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPCs : MonoBehaviour, IInteractble
{
    [SerializeField] private SpriteRenderer _interagirSprite;
    [SerializeField] public Tutorial tutorial_script;
    private const float DISTANCIA = .7f;
    public static int primeiraConversa = 0;

    void Update()
    {
        if (JogadorController.Instance.podeFalar)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && DaParaInteragir())
            {//Keyboard.current.eKey.wasPressedThisFrame
                Interagir();
            }

            if (_interagirSprite.gameObject.activeSelf && !DaParaInteragir())
                _interagirSprite.gameObject.SetActive(false);
            else if (!_interagirSprite.gameObject.activeSelf && DaParaInteragir())
            {
                primeiraConversa++;
                if(primeiraConversa == 1)
                    tutorial_script.IniciarTutorial_PararTempo("Parar interagir com outras pessoas, aperte 'E' quando estiver próximo delas.", KeyCode.E);
                
                _interagirSprite.gameObject.SetActive(true);
            }
        }
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
