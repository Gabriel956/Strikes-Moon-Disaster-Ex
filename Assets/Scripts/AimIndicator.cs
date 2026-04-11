using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    private enum AlignAxis
    {
        Up,
        Forward
    }

    [SerializeField] private Transform aimSource;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float maxDistance = 200f;
    [SerializeField] private float heightOffset = 0.02f;
    [SerializeField] private bool hideWhenNoHit = true;
    [SerializeField] private AlignAxis alignAxis = AlignAxis.Forward;
    [SerializeField] private bool disableColliders = true;

    private Renderer[] cachedRenderers;

    private void Awake()
    {
        if (aimSource == null && Camera.main != null)
        {
            aimSource = Camera.main.transform;
        }

        if (hideWhenNoHit)
        {
            cachedRenderers = GetComponentsInChildren<Renderer>(true);
        }

        if (disableColliders)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>(true);
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (aimSource == null)
        {
            return;
        }

        Ray ray = new Ray(aimSource.position, aimSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point + (hit.normal * heightOffset);
            Vector3 fromAxis = alignAxis == AlignAxis.Forward ? Vector3.forward : Vector3.up;
            transform.rotation = Quaternion.FromToRotation(fromAxis, hit.normal);

            if (hideWhenNoHit)
            {
                SetVisible(true);
            }
        }
        else if (hideWhenNoHit)
        {
            SetVisible(false);
        }
    }

    private void SetVisible(bool visible)
    {
        if (cachedRenderers == null || cachedRenderers.Length == 0)
        {
            return;
        }

        foreach (Renderer renderer in cachedRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = visible;
            }
        }
    }
}
