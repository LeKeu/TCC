using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioAtivo : MonoBehaviour
{
    private int indexSlotAtivo = 0;

    private JogadorControls jogadorControls;

    private void Awake()
    {
        jogadorControls = new JogadorControls();
    }

    private void Start()
    {
        jogadorControls.Inventory.Keyboard.performed += ctx => AtivarEspaco((int)ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    private void AtivarEspaco(int numValue) // ativar espaço ativo
    {
        AtivarEspacoHighlight(numValue - 1);
    }

    private void AtivarEspacoHighlight(int indexNum)
    {
        indexSlotAtivo = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
    }
}
