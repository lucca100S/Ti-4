using UnityEngine;

public class GrassInteraction : MonoBehaviour
{
    Material grassMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grassMaterial = GameObject.FindWithTag("Grass").GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        grassMaterial.SetVector("_PlayerPosition", transform.position);
    }
}
