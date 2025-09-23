using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe base para objetos interagíveis opcionais.
/// Implementa detecção por trigger e lógica de interação genérica.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class OptionalInteractableObjects : MonoBehaviour, IInteractable, IVisualEffects, ISoundEffects
{
    public HashSet<string> CollisionTags { get; set; }
    public bool VFXIsPlaying { get; set; }
    public bool VFXIsPaused { get; set; }
    public bool SFXIsPlaying { get; set; }
    public bool SFXIsPaused { get; set; }
    public bool PreviouslyInteracted { get; set; }

    public virtual void Interaction()
    {
        // Lógica de interação
        Debug.Log($"Interação acionada objeto: {this.gameObject.name}");
    }

    public void OnIsIntereactable()
    {
        Debug.Log($"Interação disponível objeto: {this.gameObject.name}");
    }

    public void OnIsNotIntereactable()
    {
        Debug.Log($"Interação não disponível objeto: {this.gameObject.name}");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (CollisionTags.Contains(other.tag)) 
            {
                InteractableAgent agent = other.GetComponent<InteractableAgent>();
                if (agent != null)
                {
                    //Inscrever-se na lista de objetos interativos no momento no próprio other
                    agent.AddInteractable(this);
                }
                #if UNITY_EDITOR
                else
                {
                    Debug.LogWarning($"O objeto {other.name} de tag: {other.tag}, não possui o componente InteractableAgent essencial para a execução de interações");
                }
                #endif
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (CollisionTags.Contains(other.tag))
            {
                InteractableAgent agent = other.GetComponent<InteractableAgent>();
                if (agent != null)
                {
                    //Excluir-se na lista de objetos interativos no momento no próprio other
                    agent.ExcludeInteractable(this);
                }
            }
        }
    }

    public virtual void VFXPause()
    {
        Debug.Log($"VFX pausado de: {this.gameObject.name}");
    }

    public virtual void VFXPlay()
    {
        Debug.Log($"VFX tocando de: {this.gameObject.name}");
    }

    public virtual void VFXStop()
    {
        Debug.Log($"VFX parado de: {this.gameObject.name}");
    }

    public virtual void SFXPause()
    {
        Debug.Log($"SFX pausado de: {this.gameObject.name}");
    }

    public virtual void SFXPlay()
    {
        Debug.Log($"SFX tocando de: {this.gameObject.name}");
    }

    public virtual void SFXStop()
    {
        Debug.Log($"SFX parado de: {this.gameObject.name}");
    }

    public virtual void InitializeInteractableState() => Debug.Log($"Inicializando o estado do interativo {this.gameObject.name}");
}
