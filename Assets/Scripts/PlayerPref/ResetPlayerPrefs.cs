using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("¡Memoria de PlayerPrefs borrada! Puedes probar los diálogos de nuevo.");
        }
    }
}