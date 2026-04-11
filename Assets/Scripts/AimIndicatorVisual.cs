using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AimIndicatorVisual : MonoBehaviour
{
    [SerializeField] private Color color = new Color(1f, 0.1f, 0.1f, 0.85f);
    [SerializeField] private int textureSize = 256;
    [SerializeField, Range(0.05f, 0.49f)] private float ringRadius = 0.35f;
    [SerializeField, Range(0.005f, 0.2f)] private float ringThickness = 0.04f;
    [SerializeField, Range(0.05f, 0.45f)] private float lineLength = 0.2f;
    [SerializeField, Range(0.005f, 0.2f)] private float lineThickness = 0.03f;
    [SerializeField, Range(0f, 0.1f)] private float edgeSoftness = 0.02f;
    [SerializeField] private Shader overrideShader;

    private Texture2D generatedTexture;
    private Material runtimeMaterial;

    private void OnEnable()
    {
        Apply();
    }

    private void OnValidate()
    {
        Apply();
    }

    private void OnDisable()
    {
        Cleanup();
    }

    private void Apply()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            return;
        }

        textureSize = Mathf.Clamp(textureSize, 32, 1024);

        Shader shader = overrideShader;
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Transparent");
        }
        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }
        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }
        if (shader == null)
        {
            Debug.LogWarning("AimIndicatorVisual: No suitable shader found.");
            return;
        }

        if (runtimeMaterial == null || (shader != null && runtimeMaterial.shader != shader))
        {
            CleanupMaterial();
            runtimeMaterial = new Material(shader);
            runtimeMaterial.name = "AimIndicator_Mat (Runtime)";
        }

        if (generatedTexture == null || generatedTexture.width != textureSize || generatedTexture.height != textureSize)
        {
            CleanupTexture();
            generatedTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            generatedTexture.wrapMode = TextureWrapMode.Clamp;
            generatedTexture.filterMode = FilterMode.Bilinear;
        }

        BuildTexture(generatedTexture);

        if (runtimeMaterial.HasProperty("_BaseMap"))
        {
            runtimeMaterial.SetTexture("_BaseMap", generatedTexture);
            runtimeMaterial.SetColor("_BaseColor", Color.white);
        }
        else
        {
            runtimeMaterial.mainTexture = generatedTexture;
            runtimeMaterial.color = Color.white;
        }

        renderer.sharedMaterial = runtimeMaterial;

        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
    }

    private void BuildTexture(Texture2D texture)
    {
        int size = texture.width;
        Color32[] pixels = new Color32[size * size];

        float center = (size - 1) * 0.5f;
        float ringRadiusPx = size * ringRadius;
        float ringHalfThickness = size * ringThickness * 0.5f;
        float lineHalfThickness = size * lineThickness * 0.5f;
        float lineHalfLength = size * lineLength;
        float softnessPx = size * edgeSoftness;

        Color32 transparent = new Color32(0, 0, 0, 0);
        Color baseColor = color;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                float dist = Mathf.Sqrt((dx * dx) + (dy * dy));

                float ringDelta = Mathf.Abs(dist - ringRadiusPx);
                float ringAlpha = 0f;
                if (ringDelta <= ringHalfThickness + softnessPx)
                {
                    float edge = Mathf.Max(0.0001f, ringHalfThickness);
                    float t = Mathf.Clamp01((ringDelta - edge) / Mathf.Max(0.0001f, softnessPx));
                    ringAlpha = 1f - t;
                }

                float lineAlpha = 0f;
                if (Mathf.Abs(dx) <= lineHalfThickness && Mathf.Abs(dy) <= lineHalfLength)
                {
                    lineAlpha = 1f;
                }
                if (Mathf.Abs(dy) <= lineHalfThickness && Mathf.Abs(dx) <= lineHalfLength)
                {
                    lineAlpha = Mathf.Max(lineAlpha, 1f);
                }

                float alpha = Mathf.Max(ringAlpha, lineAlpha);
                if (alpha <= 0f)
                {
                    pixels[(y * size) + x] = transparent;
                    continue;
                }

                Color pixelColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * alpha);
                pixels[(y * size) + x] = pixelColor;
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply(false, false);
    }

    private void Cleanup()
    {
        CleanupTexture();
        CleanupMaterial();
    }

    private void CleanupTexture()
    {
        if (generatedTexture == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(generatedTexture);
        }
        else
        {
            DestroyImmediate(generatedTexture);
        }

        generatedTexture = null;
    }

    private void CleanupMaterial()
    {
        if (runtimeMaterial == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(runtimeMaterial);
        }
        else
        {
            DestroyImmediate(runtimeMaterial);
        }

        runtimeMaterial = null;
    }
}
