using Skibidi.Components;
using Skibidi.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Services;
using UnityEngine;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class StartSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsWorldInject _defaultWorld = default;
        
        private EcsCustomInject<SceneService> _sceneService;
        private EcsCustomInject<PlayerService> _playerService;
        private UnitView _playerView;

        public void Init(IEcsSystems systems)
        {
            _playerView = _sceneService.Value.Player;
            
            InitUnit(_playerView, UnitType.Hero);
            _playerService.Value.PackedEntityWithWorld = _playerView.PackedEntityWithWorld;
            Debug.Log(_playerService.Value.PackedEntityWithWorld.Id);
            InitEnemyOfFirstIteration();
            
            SendTaskStartEvent();
        }

        private void InitEnemyOfFirstIteration()
        {
            foreach (var row in _sceneService.Value.EnemyByRows)
            {
                foreach (var unitView in row.Units)
                {
                    InitUnit(unitView, UnitType.Enemy);
                }
            }
        }

        private void InitUnit(UnitView view, UnitType unitType)
        {
            var entity = _defaultWorld.Value.NewEntity();

            var unitPool = _defaultWorld.Value.GetPool<UnitCmp>();
            ref var unit = ref unitPool.Add(entity);

            view.EcsEventWorld = _eventWorld.Value;
            view.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(entity);
            
            unit.View = view;
            unit.Type = unitType;
            unit.MoveSpeed = view.Speed;
            
            unit.Damage = view.Damage;
            unit.Health = view.Health;
            unit.PunchInterval = view.PunchInterval;
            unit.PunchDuration = view.PunchDuration;
        }

        public void Run(IEcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var entity = _eventWorld.Value.NewEntity();
                ref var eventComponent = ref _eventWorld.Value.GetPool<PunchEvent>().Add(entity);
                eventComponent.View = _playerView;
            }
        }

        private void SendTaskStartEvent()
        {
            var entity = _eventWorld.Value.NewEntity();
            _eventWorld.Value.GetPool<TaskEvent>().Add(entity);
        }
    }
}