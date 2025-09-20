using UnityEngine;

/// <summary>
/// Contrato para detecção em formato de esfera.
/// Permite definir origem, raio e lógica de detecção.
/// </summary>
public interface ISurroundingsSphereDetection : ICollisionFilterDetection
{
    float Radius { get; set; }
    Vector3 SphereOrigin { get; set; }

    /// <summary>Executa a lógica de detecção de objetos próximos.</summary>
    bool SurroundingsSphereDetection();

    /// <summary>Desenha a gizmo da esfera no editor para depuração.</summary>
    void OnDrawGizmos();
}
