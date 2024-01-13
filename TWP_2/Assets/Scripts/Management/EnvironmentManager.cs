using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private List<GameObject> activeObjects = new List<GameObject>();

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            GenerateObjects();
        }
        if (Input.GetButton("Fire2"))
        {
            ClearActiveObject();
        }
    }
    public void GenerateObjects()
    {
        for (int i = 0; i < GameManager.inst.ObjCount; ++i)
        {
            Vector3 spawnPosition = Random.insideUnitCircle.normalized;
            spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);

            activeObjects.Add(Instantiate(GameManager.inst.ObjectPrefabs.ElementAt(Random.Range(0, GameManager.inst.ObjectPrefabs.Count - 1)),
                                transform.position + new Vector3(spawnPosition.x, 2f, spawnPosition.y),
                                Quaternion.Euler(0, Random.Range(0, 360), 0),
                                this.transform));
        }
    }

    public void ClearActiveObject()
    {
        for (int i = 0; i < activeObjects.Count; ++i)
        {
            Destroy(activeObjects[i]);
        }
        activeObjects.Clear();
    }

    private float FindSquareFloatPos()
    {
        float cord = Random.value > 0.5f ?
        Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist) :
        Random.Range(-GameManager.inst.MinDist, -GameManager.inst.MaxDist);
        return cord;
    }
}
