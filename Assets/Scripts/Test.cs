using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    struct Part
    {
        public Vector3 dir;
        public Vector3 pos;
        public Transform transform;
    }

    Part[][] jaggedParts;

    [SerializeField] int depth = 4;

    private void Awake()
    {
        jaggedParts = new Part[depth][];
        jaggedParts[0] = new Part[1];
        for (int i = 0, length = 1; i < jaggedParts.Length; i++, length *= 5)
        {
            jaggedParts[i] = new Part[length];
        }

        Debug.Log("Jagged Part Depth : " + jaggedParts.Length);

        for (int levelIterator = 1; levelIterator < jaggedParts.Length; levelIterator++)
        {
            Part[] levelParts = jaggedParts[levelIterator];
            for (int partIterator = 0; partIterator < levelParts.Length; partIterator++)
            {
                CreateGO(levelIterator, partIterator);
            }
        }
    }

    void CreateGO(int levelIndex, int childIndex)
    {
        var go = new GameObject("Part L" + levelIndex + " C" + childIndex);
        go.transform.SetParent(transform, false);
    }
}
