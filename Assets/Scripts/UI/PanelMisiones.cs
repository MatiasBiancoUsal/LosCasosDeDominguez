using UnityEngine;

public class PanelMisiones : MonoBehaviour
{
    public GameObject _panel;
    UI_Input_Base uiib;


    public GameObject lineaNuevaMision;

    private void Awake()
    {
        uiib = new UI_Input_Base();
        uiib.UIPrimerPlano.Misiones.performed += _ => Panel();
    }

    private void OnEnable() => uiib.Enable();

    private void OnDisable() => uiib.Disable();

    public void Panel()
    {
        if(!EstadoPanel)
        {
            EstadoPanel = true;
        }
        else
        {
            EstadoPanel = false;
        }
    }

    public bool EstadoPanel
    {
        get { return _panel.activeInHierarchy; }
        set { _panel.SetActive(value); }
    }

    public void DesbloquearMision()
    {
        if (lineaNuevaMision != null)
        {
            lineaNuevaMision.SetActive(true);
            Debug.Log("Nueva línea de misión activada en la libreta.");
        }
    }
}