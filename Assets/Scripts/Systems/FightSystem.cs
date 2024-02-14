using System;
using Cysharp.Threading.Tasks;
using Skibidi.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Services;
using UnityEngine;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class FightSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PunchEvent>> _punchEvent = "events";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _punchEvent.Value)
            {
                var pool = _punchEvent.Pools.Inc1;
                ref var punchEvent = ref pool.Get(entity);

                var view = punchEvent.View;
                
                pool.Del(entity);

                ref var unit = ref view.GetUnitCmpByView();
                
                TryAttackAsync(view, unit.PunchDuration).Forget();
            }
        }

        private async UniTask TryAttackAsync(UnitView attackerView, float duration)
        {
            var targetView = attackerView.GetNearestTarget();
            
            if (targetView == null)
            {
                attackerView.ResetLook();
                return;
            }

            attackerView.AttackAnimation();
            attackerView.LookAtTarget(targetView.transform);
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            Attack(attackerView, targetView);
        }

        private void Attack(UnitView attackerView, UnitView targetView)
        {
            ref var target = ref targetView.GetUnitCmpByView();
            ref var attacker = ref attackerView.GetUnitCmpByView();
            
            Debug.Log("ATTACKER: " + attacker.View.name);
            Debug.Log("TARGET: " + target.View.name);
            
            target.Health -= attacker.Damage;
            target.View.HitAnimation();
        }
    }
}