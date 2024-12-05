using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventarioAtivo : MonoBehaviour
{
    private int indexSlotAtivo = 0;

    private JogadorControls jogadorControls;

    HashSet<string> cenasComArmaDesativada = new HashSet<string>
    {
        "01_comunidade",
        "02_comunidade",
        "T03_comunidade",
        "01_saci"
    };

    private void Awake()
    {
        jogadorControls = new JogadorControls();
    }

    private void Start()
    {
        jogadorControls.Inventory.Keyboard.performed += ctx => AtivarEspaco((int)ctx.ReadValue<float>());

        AtivarEspacoHighlight(0);
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

        MudarArmaAtiva();
    }

    void MudarArmaAtiva()
    {
        //Debug.Log(transform.GetChild(indexSlotAtivo).GetComponent<EspacoInventario>().PegarArmaInfo().armaPrefab.name);

        if (ArmaAtiva.Instance.ArmaAtivaAtual != null)
        {
            Destroy(ArmaAtiva.Instance.ArmaAtivaAtual.gameObject);
        }

        if (!transform.GetChild(indexSlotAtivo).GetComponentInChildren<EspacoInventario>())
        {
            ArmaAtiva.Instance.ArmaNull();
            return;
        }

        GameObject armaParaInstanciar = transform.GetChild(indexSlotAtivo).GetComponentInChildren<EspacoInventario>().PegarArmaInfo().armaPrefab;
        GameObject novaArma = Instantiate(armaParaInstanciar, ArmaAtiva.Instance.transform.position, Quaternion.identity);

        if (cenasComArmaDesativada.Contains(SceneManager.GetActiveScene().name))
        {
            DesativarArma();
        }

        ArmaAtiva.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        novaArma.transform.parent = ArmaAtiva.Instance.transform;

        ArmaAtiva.Instance.NovaArma(novaArma.GetComponent<MonoBehaviour>());
    }

    public void DesativarArma()
    {
        //ArmaAtiva.Instance.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        ArmaAtiva.Instance.podeAtacar = false;
        //ArmaAtiva.Instance.DesativarArma();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        //gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    public void AtivarArma()
    {
        ArmaAtiva.Instance.podeAtacar = true;
        ArmaAtiva.Instance.AtivarArma();
        ArmaAtiva.Instance.desativarAux = false;
        transform.GetChild(0).gameObject.SetActive(true);
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(true);
        //}
    }
}