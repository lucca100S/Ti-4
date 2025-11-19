using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAddMaterial : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private List<Renderer> _renders = new List<Renderer>();

    [SerializeField] private float _duration = 1;

    private Material[] _materials = new Material[2];

    private void Start()
    {
        float count = _renders.Count;
        for (int i = 0; i < count; i++)
        {
            _materials[0] = _renders[i].materials[0];
            _materials[1] = _renders[i].materials[1];

            Material[] materialsNew = new Material[1];
            materialsNew[0] = _materials[0];
            _renders[i].materials = materialsNew;
        }
    }

    public void Activate()
    {
        StartCoroutine(ChangeState(true));
    }

    public void Disactivate()
    {
        StartCoroutine(ChangeState(false));
    }

    private IEnumerator ChangeState(bool active)
    {
        float count = _renders.Count;
        for (int i = 0; i < count; i++)
        {
            _renders[i].materials = _materials;
            yield return new WaitForSeconds(_duration / count);
        }
    }
}
    