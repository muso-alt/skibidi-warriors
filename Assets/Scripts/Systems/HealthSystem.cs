using Skibidi.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components.Events;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class HealthSystem : IEcsRunSystem
    {
        private EcsPoolInject<UnitCmp> _unitCmpPool;
        private EcsFilterInject<Inc<UnitCmp>> _unitCmpFilter;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitCmpFilter.Value)
            {
                var unitCmp = _unitCmpPool.Value.Get(entity);
                
                if (unitCmp.Health > 0)
                {
                    continue;
                }

                unitCmp.View.Disable();
                unitCmp.View.DieAnimation();
                SendDieEvent(unitCmp.View);
            }
        }

        private void SendDieEvent(UnitView view)
        {
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<DieEvent>().Add(entity);
            eventComponent.View = view;
        }
    }
}