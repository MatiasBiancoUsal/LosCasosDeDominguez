using UnityEngine;

[CreateAssetMenu(fileName = "NuevaBandera", menuName = "Juego/Game Flag")]
public class GameFlag : ScriptableObject
{
    // Retorna el nombre del archivo de asset como clave única
    public string Id => name;
}