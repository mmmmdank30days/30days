using Unity.Entities;
using UnityEngine;

public class BuildingAuthoring : MonoBehaviour
{
    public float health = 100f;

    class Baker : Baker<BuildingAuthoring>
    {
        public override void Bake(BuildingAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new BuildingTag());
            AddComponent(entity, new BuildingHealth { Value = authoring.health });
        }
    }
}

public struct BuildingTag : IComponentData { }
public struct BuildingHealth : IComponentData
{
    public float Value;
}