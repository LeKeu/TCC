using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventarioAtivo : MonoBehaviour
{
    private int indexSlotAtivo = 0;

    private JogadorControls jogadorControls;
    public bool armasAtivas;

    private void Awake()
    {
        jogadorControls = new JogadorControls();
    }
    private void Start()
    {
        DesativarArma();
        jogadorControls.Inventory.Keyboard.performed += ctx => AtivarEspaco((int)ctx.ReadValue<float>());

        AtivarEspacoHighlight(0);
    }

    void DesativarArma()
    {
        string[] nomesCenas = { "01_comunidade", "02_comunidade", "03_comunidade", "01_saci" };
        if (SceneManager.GetActiveScene().name.ContainsAny(nomesCenas))
        { // se for nas cenas de comunidade ou saci1, não tem como mudar nem utilizar a arma
            armasAtivas = false;
            gameObject.SetActive(false);
        } else armasAtivas = true;
    }

    public void AtivarArma1(bool acao)
    {
        armasAtivas = acao;
        gameObject.SetActive(acao);
    }


    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    private void AtivarEspaco(int numValue) // ativar espaço ativo
    {
        if(armasAtivas)
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

        ArmaAtiva.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        novaArma.transform.parent = ArmaAtiva.Instance.transform;
        if (armasAtivas)
            ArmaAtiva.Instance.NovaArma(novaArma.GetComponent<MonoBehaviour>());
    }
}
