using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using System.Collections;

public class VegetableParticleManager : MonoBehaviour
{
    private static VegetableParticleManager instance;
    public static VegetableParticleManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindAnyObjectByType<VegetableParticleManager>();
                if (!instance)
                {
                    var go = new GameObject(typeof(VegetableParticleManager).Name + " Auto-generated");
                    instance = go.AddComponent<VegetableParticleManager>();
                }
            }
            return instance;
        }
    }
    
    [SerializeField] private VegetableSetData vegetableSetData;
    private SerializedDictionary<VegetableType, GameObject> VegetableList;
    
    private Dictionary<VegetableType, ObjectPool> ParticleObjectPools = new Dictionary<VegetableType, ObjectPool>();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        instance = this;
        
        SetParticleObjectPools();
    }

    private void SetParticleObjectPools()
    {
        VegetableList = vegetableSetData.VegetableList;
        foreach (KeyValuePair<VegetableType, GameObject> vegObj in VegetableList)
        {
            if (vegObj.Value.TryGetComponent(out Vegetable veg))
            {
                ParticleObjectPools.Add(vegObj.Key, new ObjectPool(veg.particle, 7));
            }
        }
    }
    
    public void PlayParticle(VegetableType vegetableType, Vector3 position, Quaternion rotation)
    {
        GameObject particleObject = ParticleObjectPools[vegetableType].GetObject();
        particleObject.transform.position = position;
        particleObject.transform.rotation = rotation;

        StartCoroutine(EraseParticle(vegetableType, particleObject, 1f));
    }

    private IEnumerator EraseParticle(VegetableType vegType, GameObject particleObject, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ParticleObjectPools[vegType].ReturnObject(particleObject);
    }
}
