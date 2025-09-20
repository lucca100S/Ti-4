using UnityEngine;

/// <summary>
/// Contrato para detec��o em formato de esfera.
/// Permite definir origem, raio e l�gica de detec��o.
/// </summary>
public interface ISurroundingsSphereDetection : ICollisionFilterDetection
{
    float Radius { get; set; }
    Vector3 SphereOrigin { get; set; }

    /// <summary>Executa a l�gica de detec��o de objetos pr�ximos.</summary>
    bool SurroundingsSphereDetection();

    /// <summary>Desenha a gizmo da esfera no editor para depura��o.</summary>
    void OnDrawGizmos();
}
