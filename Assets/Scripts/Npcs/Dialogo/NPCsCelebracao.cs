using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPCsCelebracao : MonoBehaviour, IInteractble
{
    [SerializeField] SpriteRenderer _interagirSprite;
    private const float DISTANCIA = .7f;

    void Update()
    {
        if (JogadorController.Instance.podeFalar)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && DaParaInteragir() && !Etapas.BrigaCelebracao)
            {//Keyboard.current.eKey.wasPressedThisFrame
                Interagir();
            }
            if (Keyboard.current.eKey.wasPressedThisFrame && Etapas.PrimeiroEncontroSaci && !JogadorController.Instance.acabouDialogo)
            {//Keyboard.current.eKey.wasPressedThisFrame
                Interagir();
            }

            if (Keyboard.current.eKey.wasPressedThisFrame && Etapas.BossSaci && !JogadorController.Instance.acabouDialogo)
            {//Keyboard.current.eKey.wasPressedThisFrame
                Interagir();
            }


            if (_interagirSprite.gameObject.activeSelf && !DaParaInteragir())
                _interagirSprite.gameObject.SetActive(false);
            else if (!_interagirSprite.gameObject.activeSelf && DaParaInteragir())
                _interagirSprite.gameObject.SetActive(true);
        }
    }
    public abstract void Interagir();

    public abstract void Interagir_CelebracaoCutscene(int index=0);

    bool DaParaInteragir()
    {
        //Debug.Log(Vector2.Distance(JogadorController.Instance.transform.position, transform.position));
        if (Vector2.Distance(JogadorController.Instance.transform.position, transform.position) < DISTANCIA)
            return true;
        else return false;
    }

    public void PassarDialogoAutomaticamente()
    {
        Interagir();
    }
}
