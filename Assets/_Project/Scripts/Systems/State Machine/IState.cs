using UnityEngine;

/// <summary>
/// Interface que define os comportamentos gerais de todo estado dentro da State Machine: Ao entrar, ao permanecer, ao saire e orientador no procedimento a realizar a depender do SurfaceType.
/// </summary>
public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    void HandleSurface(SurfaceMaterial surfaceMaterial, SurfaceType surfaceType);
}
