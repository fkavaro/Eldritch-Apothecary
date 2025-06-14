using System;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{

    // Random materials for the puddle
    public List<Material> materials;
    void Awake()
    {
        MeshRenderer rendererCharco = GetComponent<MeshRenderer>();
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
        //If the triggers is called by the cat
        if (other.gameObject == ApothecaryManager.Instance.cat.gameObject)
        {
            // Searches renderer's child who has the cat materials
            Renderer[] renderers = other.gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials; // Saves each cat's material

                for (int i = 0; i < materials.Length; i++)
                {
                    // Looks for the body material
                    if (materials[i].name.Contains("Material.084"))
                    {
                        // Changes it to a random color
                        materials[i].color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                    }
                }
                //Reassign them again
                renderer.materials = materials; 
            }
        }

    }
}
