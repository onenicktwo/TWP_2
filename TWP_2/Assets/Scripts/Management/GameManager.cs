using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    [SerializeField]
    private List<GameObject> objectPrefabs = new List<GameObject>();
    [SerializeField]
    private float minDist = 0f;
    [SerializeField]
    private float maxDist = 0f;
    [SerializeField]
    private int objCount = 0;

    public List<GameObject> ObjectPrefabs { get => objectPrefabs; }
    public float MinDist { get => minDist; set => minDist = value; }
    public float MaxDist { get => maxDist; set => maxDist = value; }
    public int ObjCount { get => objCount; set => objCount = value; }

    private void Awake()
    {
        inst = this;
    }
}
