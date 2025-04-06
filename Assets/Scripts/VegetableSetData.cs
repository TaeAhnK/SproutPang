using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "VegetableSetData", menuName = "ScriptableObjects/VegetableSet")]
public class VegetableSetData : ScriptableObject
{
    [SerializeField] public SerializedDictionary<VegetableType, GameObject> VegetableList;
}
