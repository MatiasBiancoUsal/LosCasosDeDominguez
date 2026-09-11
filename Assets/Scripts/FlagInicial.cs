using UnityEngine;

public class FlagInicial : MonoBehaviour
{
    [Header("Flag que el jugador tendrá desde el inicio")]
    [SerializeField] private GameFlag flag;

    private void Awake()
    {
        if (flag == null || string.IsNullOrEmpty(flag.Id))
        {
            Debug.LogWarning("No hay una GameFlag asignada.");
            return;
        }

        PlayerPrefs.SetInt(flag.Id, 1);
        PlayerPrefs.Save();

        Debug.Log("Flag inicial obtenida: " + flag.Id);
    }
}