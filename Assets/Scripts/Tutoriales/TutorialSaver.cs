using UnityEngine;

public class TutorialSaver : MonoBehaviour
{
    public string _tkey = "TutorialCompleted";
    int _tvalue = 0;
    public ControladorTutorial _controlTutorial;

    private void Awake()
    {
        _tvalue = PlayerPrefs.GetInt(_tkey, 0);

        if(_tvalue > 0)
        {
            _controlTutorial.usarTutorial = ControladorTutorial.MostrarTuotial.no;
        }

        Invoke(nameof(SaveTutorial), 5);
    }

    void SaveTutorial()
    {
        PlayerPrefs.SetInt(_tkey, 1);
        PlayerPrefs.Save();
    }
}
