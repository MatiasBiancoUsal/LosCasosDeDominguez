using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Inventario : MonoBehaviour
{
    [SerializeField] private GameObject Inventarios;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            bool abierto = Inventarios.activeSelf;

            Inventarios.SetActive(!abierto);
            Inventarios.SetActive(!abierto);

            Time.timeScale = !abierto ? 0f : 1f;
        }
    }
}