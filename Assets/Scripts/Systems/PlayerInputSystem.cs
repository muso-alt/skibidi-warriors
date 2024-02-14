using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Components.Events;
using Skibidi.Services;
using UnityEngine;

namespace Skibidi.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _eventWorld = "events";
        private EcsCustomInject<PlayerService> _playerService;
        
        public void Run(IEcsSystems systems)
        {
            if (_playerService.Value.GameOver)
            {
                return;
            } 
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                SendPunchEvent();
            }

            if (Input.GetKey(KeyCode.D))
            {
                SendDefendEvent(true);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                SendDefendEvent(false);
            }
        }
        
        private void SendPunchEvent()
        {
            ref var unit = ref GetPlayerUnit();
            
            if (!unit.IsAllowToPunch())
            {
                return;
            }
                
            unit.LastPunch = Time.time;
            
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<PunchEvent>().Add(entity);
            eventComponent.View = unit.View;
        }

        private void SendDefendEvent(bool toggleValue)
        {
            ref var unit = ref GetPlayerUnit();
            
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<DefendEvent>().Add(entity);
            eventComponent.View = unit.View;
            eventComponent.IsActive = toggleValue;
        }

        private ref UnitCmp GetPlayerUnit()
        {
            var player = _playerService.Value.PackedEntityWithWorld;

            if (!player.Unpack(out var world, out var playerEntity))
            {
                throw new System.NullReferenceException();
            }

            var pool = world.GetPool<UnitCmp>();
            return ref pool.Get(playerEntity);
        }
    }
}