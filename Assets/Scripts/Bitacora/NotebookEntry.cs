using System;
using UnityEngine;

[Serializable]
public class NotebookEntry
{
    [Header("Condición")]
    [Tooltip("La información aparecerá en la Bitácora cuando esta bandera esté desbloqueada.")]
    public GameFlag flag;

    [Header("Información")]
    [TextArea(3, 8)]
    public string texto;
}