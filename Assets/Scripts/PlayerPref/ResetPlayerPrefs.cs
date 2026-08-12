using UnityEngine;
using UnityEngine.InputSystem; // Usamos el nuevo Input System para mantener coherencia

public class ResetPlayerPrefs : MonoBehaviour
{
    [Header("Testing")]
    [Tooltip("Si activás esta casilla, borrará el progreso automáticamente cada vez que le des a Play.")]
    [SerializeField] private bool borrarAlIniciar = false;

    private void Start()
    {
        if (borrarAlIniciar)
        {
            BorrarTodo();
        }
    }

    private void Update()
    {
        // Borrar presionando la tecla R en cualquier momento durante la partida
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            BorrarTodo();
        }
    }

    // Esta etiqueta permite que aparezca la opción con Clic Derecho en el Inspector
    [ContextMenu("Borrar PlayerPrefs Ahora")]
    public void BorrarTodo()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[ResetPlayerPrefs] ¡Memoria de PlayerPrefs borrada! Podés probar de nuevo.");
    }
}