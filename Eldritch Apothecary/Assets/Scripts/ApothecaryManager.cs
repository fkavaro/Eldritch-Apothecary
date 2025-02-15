using UnityEngine;

public class ApothecaryManager : MonoBehaviour
{
    public static ApothecaryManager Instance { get; private set; }
    public Transform target1, target2;

    void Awake()
    {
        // Creates one instance if there isn't any (Singleton)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
