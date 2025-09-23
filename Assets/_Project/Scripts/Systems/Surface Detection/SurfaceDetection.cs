using System.Collections.Generic;
using UnityEngine;
using Lugu.Utils.Debug;
using System;

/// <summary>
/// Classe responsável por detectar superfícies ao redor do objeto, determinando tipo, material e prioridade.
/// </summary>
public class SurfaceDetection : MonoBehaviour, ICollisionFilterDetection
{
    [Header("Interface (ICollisionFilterDetection)")]
    public LayerMask CollisionMask => raycastMask;
    public HashSet<string> CollisionTags { get; set; }

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private List<string> collisionTagsList;
    private HashSet<string> collisionTagsSet;

    [Header("Raycast Settings")]
    [SerializeField, Range(0f, 1f)] private float tolerance = 0.7f;
    [SerializeField] private bool drawDebug = true;

    [Header("Raycast Directions")]
    [SerializeField]
    private RayDirectionConfig[] rayDirectionsConfig = new RayDirectionConfig[]
    {
        new RayDirectionConfig { directionVector = RayDirectionConfig.Direction.Down, distance = 2f },
        new RayDirectionConfig { directionVector = RayDirectionConfig.Direction.Up, distance = 2f },
        new RayDirectionConfig { directionVector = RayDirectionConfig.Direction.Forward, distance = 2f },
    };

    public struct SurfaceHit
    {
        public SurfaceType type;
        public SurfaceMaterial material;
        public RaycastHit hit;

        public SurfaceHit(SurfaceType type, SurfaceMaterial material, RaycastHit hit)
        {
            this.type = type;
            this.material = material;
            this.hit = hit;
        }
    }

    private readonly Dictionary<SurfaceType, List<SurfaceHit>> detectedHits = new();
    public SurfaceHit? CurrentSurface { get; private set; } = null;
    private readonly Dictionary<string, SurfaceMaterial> materialMap = new();

    private void Awake()
    {
        InitializeDetectedHits();
        InitializeCollisionTags();
        InitializeMaterialMap();
    }

    private void Update()
    {
        CollectHits();
        ProcessSurface();
    }

    private void InitializeDetectedHits()
    {
        foreach (SurfaceType t in System.Enum.GetValues(typeof(SurfaceType)))
            detectedHits[t] = new List<SurfaceHit>();
    }

    private void InitializeCollisionTags()
    {
        collisionTagsSet = new HashSet<string>(collisionTagsList);
    }

    private void InitializeMaterialMap()
    {
        materialMap.Add("earth", SurfaceMaterial.Earth);
        materialMap.Add("stone", SurfaceMaterial.Stone);
        materialMap.Add("vines", SurfaceMaterial.Vines);
    }

    public Action<SurfaceHit> OnSurfaceChange;
    public Action<SurfaceHit> OnSurfaceHit;
    public Action OnSurfaceNull;
    private void CollectHits()
    {
        foreach (var list in detectedHits.Values)
            list.Clear();

        Vector3 origin = transform.position;

        foreach (var config in rayDirectionsConfig)
        {
            if (UnityEngine.Physics.Raycast(origin, config.direction, out RaycastHit hit, config.distance, CollisionMask))
            {
                SurfaceType type = ClassifySurface(hit.normal);
                SurfaceMaterial material = ClassifyMaterial(hit.collider?.tag);
                detectedHits[type].Add(new SurfaceHit(type, material, hit));

#if UNITY_EDITOR
                if (drawDebug)
                {
                    Debug.DrawRay(origin, config.direction * hit.distance, Color.green);
                    Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.yellow);
                }
#endif
            }
            else
            {
#if UNITY_EDITOR
                if (drawDebug)
                    Debug.DrawRay(origin, config.direction * config.distance, Color.red);
#endif
            }
        }
    }
    private void ProcessSurface()
    {
        SurfaceHit? priority = GetHighestPrioritySurface();

        if (priority.HasValue)
        {
            OnSurfaceHit?.Invoke(priority.Value);

            if (!CurrentSurface.HasValue ||
                priority.Value.type != CurrentSurface.Value.type ||
                priority.Value.material != CurrentSurface.Value.material)
            {
                CurrentSurface = priority;
                SurfaceNotifier(CurrentSurface.Value);
            }
        }
        else if (CurrentSurface.HasValue)
        {
            DebugLogger.Log("[SurfaceDetector] Saiu de qualquer superfície detectável.", "SurfaceDetection");
            CurrentSurface = null;
        }

        if(CurrentSurface == null) OnSurfaceNull?.Invoke();
    }

    private SurfaceHit? GetHighestPrioritySurface()
    {
        if (detectedHits[SurfaceType.Floor].Count > 0) return GetClosestHit(detectedHits[SurfaceType.Floor]);
        if (detectedHits[SurfaceType.Wall].Count > 0) return GetClosestHit(detectedHits[SurfaceType.Wall]);
        if (detectedHits[SurfaceType.Ceiling].Count > 0) return GetClosestHit(detectedHits[SurfaceType.Ceiling]);
        return null;
    }

    private SurfaceHit GetClosestHit(List<SurfaceHit> hits)
    {
        SurfaceHit closest = hits[0];
        float minDistance = hits[0].hit.distance;

        foreach (var h in hits)
        {
            if (h.hit.distance < minDistance)
            {
                minDistance = h.hit.distance;
                closest = h;
            }
        }
        return closest;
    }

    private void SurfaceNotifier(SurfaceHit surface)
    {
        OnSurfaceChange?.Invoke(surface);
        DebugLogger.Log($"[OnSurfaceChange] Tipo: {surface.type}, Material: {surface.material}, Ponto: {surface.hit.point}, Normal: {surface.hit.normal}", "SurfaceDetection");
    }
    private SurfaceType ClassifySurface(Vector3 normal)
    {
        if (normal.y > tolerance) return SurfaceType.Floor;
        if (normal.y < -tolerance) return SurfaceType.Ceiling;
        return SurfaceType.Wall;
    }

    private SurfaceMaterial ClassifyMaterial(string tag)
    {
        if (string.IsNullOrEmpty(tag)) return SurfaceMaterial.None;

        if (materialMap.TryGetValue(tag.ToLower(), out SurfaceMaterial mat))
            return mat;

        return SurfaceMaterial.None;
    }
}
