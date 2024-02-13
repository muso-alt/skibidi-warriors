using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;

namespace Skibidi.Systems
{
    public class AutoHitSystem : IEcsRunSystem
    {
        private EcsPoolInject<UnitCmp> _unitCmpPool;
        private EcsFilterInject<Inc<UnitCmp>> _unitCmpFilter;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            
        }
    }
}