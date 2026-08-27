using UnityEngine;

[CreateAssetMenu(fileName = "NuevaPista", menuName = "Inventario/Info de Objeto")]
public class PistasScriptable : ScriptableObject
{
    public string nombreObjeto;
    public Sprite imagenObjeto;
    [TextArea(3, 20)] public string descripcionObjeto;
}
