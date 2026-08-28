using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public static class BuscarMissingScripts
{
    [MenuItem("Herramientas/Buscar Missing Scripts en todo el proyecto")]
    public static void BuscarEnTodoElProyecto()
    {
        int total = 0;

        total += BuscarEnObjetosCargados();
        total += BuscarEnPrefabs();
        total += BuscarEnAnimators();

        if (total == 0)
        {
            Debug.Log(
                "No se encontraron Missing Scripts en escenas, prefabs ni Animator Controllers."
            );
        }
        else
        {
            Debug.LogWarning(
                "BÚSQUEDA TERMINADA. Total de referencias perdidas: " + total
            );
        }
    }

    private static int BuscarEnObjetosCargados()
    {
        int total = 0;

        Transform[] objetos = Object.FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (Transform transformEncontrado in objetos)
        {
            GameObject objeto = transformEncontrado.gameObject;

            int faltantes =
                GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(objeto);

            if (faltantes > 0)
            {
                total += faltantes;

                Debug.LogWarning(
                    "MISSING EN OBJETO CARGADO: " +
                    ObtenerRuta(transformEncontrado),
                    objeto
                );
            }
        }

        return total;
    }

    private static int BuscarEnPrefabs()
    {
        int total = 0;
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in guids)
        {
            string rutaPrefab = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = PrefabUtility.LoadPrefabContents(rutaPrefab);

            Transform[] objetos =
                prefab.GetComponentsInChildren<Transform>(true);

            foreach (Transform objeto in objetos)
            {
                int faltantes =
                    GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(
                        objeto.gameObject
                    );

                if (faltantes > 0)
                {
                    total += faltantes;

                    Debug.LogWarning(
                        "MISSING EN PREFAB: " +
                        rutaPrefab +
                        " → " +
                        ObtenerRuta(objeto),
                        AssetDatabase.LoadAssetAtPath<GameObject>(rutaPrefab)
                    );
                }
            }

            PrefabUtility.UnloadPrefabContents(prefab);
        }

        return total;
    }

    private static int BuscarEnAnimators()
    {
        int total = 0;
        string[] guids = AssetDatabase.FindAssets("t:AnimatorController");

        foreach (string guid in guids)
        {
            string ruta = AssetDatabase.GUIDToAssetPath(guid);

            AnimatorController controller =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(ruta);

            foreach (AnimatorControllerLayer layer in controller.layers)
            {
                total += RevisarStateMachine(
                    layer.stateMachine,
                    ruta,
                    controller
                );
            }
        }

        return total;
    }

    private static int RevisarStateMachine(
        AnimatorStateMachine maquina,
        string ruta,
        AnimatorController controller
    )
    {
        int total = 0;

        foreach (ChildAnimatorState estadoHijo in maquina.states)
        {
            AnimatorState estado = estadoHijo.state;

            foreach (StateMachineBehaviour comportamiento in estado.behaviours)
            {
                if (comportamiento == null)
                {
                    total++;

                    Debug.LogWarning(
                        "MISSING STATEMACHINE BEHAVIOUR EN ANIMATOR: " +
                        ruta +
                        " → Estado: " +
                        estado.name,
                        controller
                    );
                }
            }
        }

        foreach (
            ChildAnimatorStateMachine maquinaHija in maquina.stateMachines
        )
        {
            total += RevisarStateMachine(
                maquinaHija.stateMachine,
                ruta,
                controller
            );
        }

        return total;
    }

    private static string ObtenerRuta(Transform objeto)
    {
        string ruta = objeto.name;
        Transform padre = objeto.parent;

        while (padre != null)
        {
            ruta = padre.name + "/" + ruta;
            padre = padre.parent;
        }

        return ruta;
    }
}