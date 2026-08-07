using UnityEngine;

public class ControladorInventario : MonoBehaviour
{
    [SerializeField] private GameObject inventario;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            inventario.SetActive(!inventario.activeSelf);
        }
    }
}