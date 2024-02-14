using Skibidi.Components;
using Leopotam.EcsLite;
using Skibidi.Views;
using UnityEngine;

namespace Skibidi.Services
{
    public static class Extensions
    {
        public static ref UnitCmp GetUnitCmpByView(this UnitView view)
        {
            if (!view.PackedEntityWithWorld.Unpack(out var world, out var targetEntity))
            {
                throw new System.NullReferenceException();
            }
                
            var unitPool = world.GetPool<UnitCmp>();
            return ref unitPool.Get(targetEntity);
        }

        public static bool IsAllowToPunch(this ref UnitCmp unit)
        {
            if (unit.State == UnitState.Moving)
            {
                return false;
            }
            
            return !(Time.time - unit.LastPunch < unit.PunchInterval);
        }

        public static UnitView GetNearestTarget(this UnitView attackerView)
        {
            var results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(attackerView.transform.position, 10f, results);
            
            Debug.Log(size);

            for (var index = 0; index < size; index++)
            {
                var hit = results[index];
                
                Debug.Log(hit.name);
                var targetView = hit.GetComponent<UnitView>();

                if (targetView == null)
                {
                    continue;
                }

                ref var target = ref targetView.GetUnitCmpByView();
                ref var attacker = ref attackerView.GetUnitCmpByView();

                //In case if allies attacked each other. Maybe i'll think of something better
                if (target.Type == UnitType.Enemy && attacker.Type == UnitType.Enemy
                    || target.Type == UnitType.Hero && attacker.Type == UnitType.Hero)
                {
                    continue;
                }

                return targetView;
            }

            return null;
        }
    }
}