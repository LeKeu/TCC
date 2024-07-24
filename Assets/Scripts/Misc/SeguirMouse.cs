using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirMouse : MonoBehaviour
{
    private void Update()
    {
        OlharParaMouse();
    }

    private void OlharParaMouse()
    {
        Vector3 posMouse = Input.mousePosition;
        posMouse = Camera.main.ScreenToWorldPoint(posMouse);

        Vector2 direcao = transform.position - posMouse;

        transform.right = -direcao;
    }
}
