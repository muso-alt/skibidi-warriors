using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Components.Events;
using Skibidi.Services;
using UnityEngine;

namespace Skibidi.Systems
{
    public class DefendSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DefendEvent>> _defendFilter = "events";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _defendFilter.Value)
            {
                var pool = _defendFilter.Pools.Inc1;
                ref var defendEvent = ref pool.Get(entity);
                
                var view = defendEvent.View;
                var isActive = defendEvent.IsActive;
                
                pool.Del(entity);

                ref var unit = ref view.GetUnitCmpByView();

                if (unit.State == UnitState.Fighting || unit.State == UnitState.Moving)
                {
                    continue;
                }

                unit.State = isActive ? UnitState.Defending : UnitState.Idle;
                unit.View.Defend(isActive);
            }
        }
    }
}