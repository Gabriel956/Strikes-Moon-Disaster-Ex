using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    private enum AlignAxis
    {
        Up,
        Forward
    }

    [SerializeField] private Transform ufoTransform;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float maxDistance = 200f;
    [SerializeField] private float heightOffset = 0.02f;
    [SerializeField] private AlignAxis alignAxis = AlignAxis.Forward;
    [SerializeField] private bool disableColliders = true;

    private Renderer[] cachedRenderers;
    private Transform ufo;

    private void Awake()
    {
        cachedRenderers = GetComponentsInChildren<Renderer>(true);

        if (ufoTransform != null)
            ufo = ufoTransform;
        else
        {
            PlayerMovementController player = UnityEngine.Object.FindFirstObjectByType<PlayerMovementController>();
            if (player != null) ufo = player.transform;
        }

        if (disableColliders)
        {
            foreach (Collider col in GetComponentsInChildren<Collider>(true))
                col.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (ufo == null) { SetVisible(false); return; }

        Ray ray = new Ray(ufo.position, Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, groundMask, QueryTriggerInteraction.Ignore);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool found = false;
        foreach (RaycastHit h in hits)
        {
            if (h.transform.IsChildOf(ufo)) continue;
            transform.position = h.point + Vector3.up * heightOffset;
            transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            SetVisible(true);
            found = true;
            break;
        }

        if (!found) SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (cachedRenderers == null || cachedRenderers.Length == 0) return;
        foreach (Renderer r in cachedRenderers)
            if (r != null) r.enabled = visible;
    }
}
