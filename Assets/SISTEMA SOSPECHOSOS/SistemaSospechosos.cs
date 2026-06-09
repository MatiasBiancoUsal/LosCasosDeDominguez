using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic; 

public class MostrarPistasSospechoso : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<GameObject> pistasDelSospechoso = new List<GameObject>();

    void Start()
    {
        foreach (GameObject pista in pistasDelSospechoso)
        {
            if (pista != null) pista.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (GameObject pista in pistasDelSospechoso)
        {
            if (pista != null) pista.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject pista in pistasDelSospechoso)
        {
            if (pista != null) pista.SetActive(false);
        }
    }
}
