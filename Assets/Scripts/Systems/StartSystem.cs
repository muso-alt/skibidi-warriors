using Skibidi.Components;
using Skibidi.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Services;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class StartSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsFilterInject<Inc<GameStartEvent>> _gameStartFilter = "events";
        
        private EcsCustomInject<SceneService> _sceneService;
        private EcsCustomInject<PlayerService> _playerService;
        private EcsCustomInject<TokenService> _tokenService;
        
        private EcsPoolInject<UnitCmp> _unitCmpPool;
        private UnitView _playerView;

        public void Init(IEcsSystems systems)
        {
            StartGame();
        }
        
        /*public void Run(IEcsSystems systems)
        {
            foreach (var entity in _gameStartFilter.Value)
            {
                var pool = _gameStartFilter.Pools.Inc1;
                pool.Del(entity);
                StartGame();
            }
        }*/

        private void StartGame()
        {
            Restore();

            _playerService.Value.GameOver = false;
            
            _playerView = _sceneService.Value.Player;
            
            InitUnit(_playerView, UnitType.Hero);
            
            _playerService.Value.PackedEntityWithWorld = _playerView.PackedEntityWithWorld;
            
            InitEnemyOfFirstIteration();
            
            SendTaskStartEvent();
        }

        private void Restore()
        {
            foreach (var row in _sceneService.Value.EnemyByRows)
            {
                foreach (var unit in row.Units)
                {
                    unit.Restore();
                    
                    RemoveEntity(unit.PackedEntityWithWorld);
                }
            }

            var player = _sceneService.Value.Player;
            
            player.Restore();
            RemoveEntity(player.PackedEntityWithWorld);
        }

        private void RemoveEntity(EcsPackedEntityWithWorld pew)
        {
            if (!pew.Unpack(out var world, out var entity))
            {
                return;
            }

            world.DelEntity(entity);
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
            
            _tokenService.Value.TokensByEntity.Add(entity, null);

            view.EcsEventWorld = _eventWorld.Value;
            view.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(entity);
            view.SetHealthValue(view.Health);
            
            unit.View = view;
            unit.Type = unitType;
            unit.MoveSpeed = view.Speed;
            
            unit.Damage = view.Damage;
            unit.Health = view.Health;
            unit.PunchInterval = view.PunchInterval;
            unit.PunchDuration = view.PunchDuration;
        }

        private void SendTaskStartEvent()
        {
            var entity = _eventWorld.Value.NewEntity();
            _eventWorld.Value.GetPool<TaskEvent>().Add(entity);
        }
    }
}