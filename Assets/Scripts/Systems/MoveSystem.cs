using Skibidi.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Services;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MovementEvent>> _moveFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _moveFilter.Value)
            {
                var pool = _moveFilter.Pools.Inc1;
                ref var moveEvent = ref pool.Get(entity);
                
                var view = moveEvent.View;
                var status = moveEvent.Status;
                
                pool.Del(entity);
                
                ref var unit = ref view.GetUnitCmpByView();

                var state = status == MoveStatus.Begin;
                var speed = state ? unit.MoveSpeed : 0;

                if (!state)
                {
                    TryLookAtTarget(unit.View);
                }
            
                unit.View.SetSpeed(speed);
                unit.View.ToggleWalkState(state);
                unit.State = state ? UnitState.Moving : UnitState.Idle;
            }
        }

        private void TryLookAtTarget(UnitView attacker)
        {
            var target = attacker.GetNearestTarget();

            if (target == null)
            {
                return;
            }
            
            attacker.LookAtTarget(target.transform);
        }
    }
}