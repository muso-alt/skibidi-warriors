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
            if (Input.GetKeyDown(KeyCode.A))
            {
                SendPunchEvent();
            }
        }
        
        private void SendPunchEvent()
        {
            var player = _playerService.Value.PackedEntityWithWorld;

            if (!player.Unpack(out var world, out var playerEntity))
            {
                return;
            }

            var pool = world.GetPool<UnitCmp>();
            ref var unit = ref pool.Get(playerEntity);
            
            if (!unit.IsAllowToPunch())
            {
                return;
            }
                
            unit.LastPunch = Time.time;
            
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<PunchEvent>().Add(entity);
            eventComponent.View = unit.View;
        }
    }
}