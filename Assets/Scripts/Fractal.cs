using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    struct FractalPart
    {
        public Vector3 direction;
        public Quaternion rotation;
        public Transform transform;
    }

    FractalPart[][] parts;

    [SerializeField, Range(1, 8)] int depth = 4;
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    static Vector3[] directions =
    {
        Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
    };

    static Quaternion[] rotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
    };

    private void Awake()
    {
        parts = new FractalPart[depth][];
        parts[0] = new FractalPart[1];
        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5)
        {
            parts[i] = new FractalPart[length];
        }

        float scale = 1f;
        parts[0][0] = CreatePart(0, 0, scale);
        for (int levelIterator = 1; levelIterator < parts.Length; levelIterator++)
        {
            scale *= 0.5f;
            FractalPart[] levelParts = parts[levelIterator];
            for (int fpi = 0; fpi < levelParts.Length; fpi += 5)
            {
                for (int ci = 0; ci < 5; ci++)
                {
                    levelParts[fpi + ci] = CreatePart(levelIterator, ci, scale);
                }
            }
        }
    }

    private FractalPart CreatePart(int levelIndex, int childIndex, float scale)
    {
        var go = new GameObject("FractalPart L" + levelIndex + " C" + childIndex);
        go.transform.SetParent(transform, false);
        go.transform.AddComponent<MeshFilter>().mesh = mesh;
        go.transform.AddComponent<MeshRenderer>().material = material;
        go.transform.localScale = scale * Vector3.one;
        return new FractalPart
        {
            direction = directions[childIndex],
            rotation = rotations[childIndex],
            transform = go.transform
        };
    }

    private void Update()
    {
        for (int levelIterator = 1; levelIterator < parts.Length; levelIterator++)
        {
            /// iterates over all levels and all their parts
            FractalPart[] parentParts = parts[levelIterator - 1];
            FractalPart[] levelParts = parts[levelIterator];
            for(int fpi = 0; fpi < levelParts.Length; fpi++)
            {
                Transform parentTransform = parentParts[fpi / 5].transform;
                FractalPart part = levelParts[fpi];
                part.transform.localRotation = parentTransform.localRotation * part.rotation;
                part.transform.localPosition =
                    parentTransform.localPosition +
                    parentTransform.localRotation *
                     (1.5f * part.transform.localScale.x * part.direction);
            }
        }
    }
}
