using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCleanMaterial : MonoBehaviour
{
    private Renderer m_renderer;

    [SerializeField] private float _startClean = 0;

    private List<Material> m_materials = new List<Material>();

    [SerializeField] private float _cleanDuration = 1f;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        for (int i = 0; i < m_renderer.materials.Length; i++)
        {
            m_materials.Add(m_renderer.materials[i]);
            m_materials[i].SetFloat("_MaterialBlend", _startClean);
        }
    }

    public void Clean()
    {
        for(int i = 0;i < m_materials.Count;i++)
        {
            Material mat = m_materials[i];
            mat.DOFloat(1, "_MaterialBlend", _cleanDuration);
        }
    }

}
