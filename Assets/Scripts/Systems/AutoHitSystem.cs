using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Components.Events;
using Skibidi.Services;
using Skibidi.Views;
using UnityEngine;

namespace Skibidi.Systems
{
    public class AutoHitSystem : IEcsRunSystem
    {
        private EcsPoolInject<UnitCmp> _unitCmpPool;
        private EcsFilterInject<Inc<UnitCmp>> _unitCmpFilter;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitCmpFilter.Value)
            {
                ref var unit = ref _unitCmpPool.Value.Get(entity);

                if (unit.Type == UnitType.Hero || !unit.IsAllowToPunch())
                {
                    continue;
                }
                
                unit.LastPunch = Time.time;
                SendPunchEvent(ref unit);
            }
        }

        private void SendPunchEvent(ref UnitCmp unit)
        {
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<PunchEvent>().Add(entity);
            eventComponent.View = unit.View;
        }
    }
}