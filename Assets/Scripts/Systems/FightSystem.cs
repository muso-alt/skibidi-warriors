using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Skibidi.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Services;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class FightSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PunchEvent>> _punchEvent = "events";
        private EcsCustomInject<TokenService> _tokenService;

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

            ToggleUnitState(attackerView, UnitState.Fighting);
            attackerView.AttackAnimation();
            attackerView.LookAtTarget(targetView.transform);
            
            var entity = attackerView.PackedEntityWithWorld.Id;

            if (_tokenService.Value.TokensByEntity[entity] != null)
            {
                return;
            }

            var token = new CancellationTokenSource();
            _tokenService.Value.TokensByEntity[entity] = token;

            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token.Token);

            Attack(attackerView, targetView);
        }

        private void Attack(UnitView attackerView, UnitView targetView)
        {
            ref var target = ref targetView.GetUnitCmpByView();
            ref var attacker = ref attackerView.GetUnitCmpByView();
            
            _tokenService.Value.DisposeByEntity(attacker.View.PackedEntityWithWorld.Id);

            if(target.State != UnitState.Defending)
            {
                target.Health -= attacker.Damage;
                target.View.HitAnimation();
            }
            
            ToggleUnitState(attackerView, UnitState.Idle);
        }

        private void ToggleUnitState(UnitView view, UnitState state)
        {
            ref var unit = ref view.GetUnitCmpByView();
            unit.State = state;
        }
    }
}