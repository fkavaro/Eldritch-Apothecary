using System;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    /// <summary>
    /// Event triggered when object enters the trigger area
    /// </summary>
    public event Action<GameObject> CatOnPuddle;


    Alchemist alchemist;
    GameObject GO_alchemist;
    public GameObject cat;

    public List<Material> materials;
    void Awake()
    {
        GO_alchemist = GameObject.Find("Alchemist");
        alchemist = GO_alchemist.GetComponent<Alchemist>();
        cat = alchemist.cat;
        Renderer rendererCharco = GetComponent<Renderer>();
        if (rendererCharco != null && materials.Count > 0)
        {
            Material materialAleatorio = materials[UnityEngine.Random.Range(0, materials.Count)];
            rendererCharco.material = materialAleatorio;
        }
        else
        {
            Debug.LogWarning("No se encontró Renderer en el charco o no hay materiales asignados");
        }

        Destroy(gameObject, 18f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cat)
        {
            Debug.Log("Colision gato");
            //CatOnPuddle?.Invoke(other.gameObject);

            // Buscar el renderer en el hijo que contiene los materiales
            Renderer[] renderers = other.gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials; // Copia para evitar modificar materiales compartidos

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].name.Contains("Material.084"))
                    {
                        // Cambiar a un color aleatorio
                        materials[i].color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                    }
                }

                renderer.materials = materials; // Reasignar por seguridad
            }
        }

    }
}
