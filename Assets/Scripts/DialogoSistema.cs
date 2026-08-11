using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "Objeto", menuName = "NuevoDialogo")]
public class DialogoSistema : ScriptableObject 
    
    
{
    //public string nombre;
    //[TextAreaAttribute(3, 10)] public  string [] dialogo;
    //public Sprite img;
    public Dialog[] dialogos;

}

[System.Serializable]
public class Dialog
{
    [TextAreaAttribute(3, 10)] public string dialogo;
    public string nombre;
    public Sprite cuadrodialogo;
    public Sprite personaje;
    public Sprite fondo;
   
}
