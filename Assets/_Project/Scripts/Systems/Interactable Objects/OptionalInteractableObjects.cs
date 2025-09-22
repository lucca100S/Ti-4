using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe base para objetos interagíveis opcionais.
/// Implementa detecção por esfera e lógica de interação genérica.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class OptionalInteractableObjects : MonoBehaviour, IInteractable
{
    public HashSet<string> CollisionTags { get; set; }

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
}
