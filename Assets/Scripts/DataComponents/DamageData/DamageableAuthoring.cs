using Unity.Entities;
using UnityEngine;

namespace DataComponents
{
    public class DamageableAuthoring : MonoBehaviour , IConvertGameObjectToEntity
    {

        [SerializeField] private int startingHealth = 100;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<Damage>(entity);
            dstManager.AddComponentData(entity, new HealthData {Value = startingHealth});
        }
    }
}