using System.Collections;
using UnityEngine;

public class AutoBlink : MonoBehaviour
{
    public SkinnedMeshRenderer faceMesh;  // wijs je Head-mesh toe
    public string blinkLeft = "EyeBlinkLeft";
    public string blinkRight = "EyeBlinkRight";
    int idxL = -1, idxR = -1;

    void Awake()
    {
        if (!faceMesh) faceMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        if (faceMesh && faceMesh.sharedMesh)
        {
            var mesh = faceMesh.sharedMesh;
            // Probeer exact
            idxL = mesh.GetBlendShapeIndex(blinkLeft);
            idxR = mesh.GetBlendShapeIndex(blinkRight);

            // Auto-detect fallback (zoek op substring "blink")
            if (idxL < 0 || idxR < 0)
            {
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    var n = mesh.GetBlendShapeName(i);
                    var lower = n.ToLowerInvariant();
                    if (idxL < 0 && lower.Contains("blink") && lower.Contains("left")) idxL = i;
                    if (idxR < 0 && lower.Contains("blink") && lower.Contains("right")) idxR = i;
                }
            }

            if (idxL < 0 || idxR < 0)
                Debug.LogWarning($"Blink blendshapes niet gevonden op {faceMesh.name}. Check import of RPM export.");
            else
                StartCoroutine(BlinkLoop());
        }
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 6f));
            yield return StartCoroutine(BlinkOnce(0.06f, 0.08f));
        }
    }

    IEnumerator BlinkOnce(float closeTime, float openTime)
    {
        if (idxL < 0 || idxR < 0) yield break;

        float t = 0f;
        while (t < closeTime)
        {
            t += Time.deltaTime;
            float w = Mathf.SmoothStep(0, 100, t / closeTime);
            faceMesh.SetBlendShapeWeight(idxL, w);
            faceMesh.SetBlendShapeWeight(idxR, w);
            yield return null;
        }
        t = 0f;
        while (t < openTime)
        {
            t += Time.deltaTime;
            float w = Mathf.SmoothStep(100, 0, t / openTime);
            faceMesh.SetBlendShapeWeight(idxL, w);
            faceMesh.SetBlendShapeWeight(idxR, w);
            yield return null;
        }
    }
}
