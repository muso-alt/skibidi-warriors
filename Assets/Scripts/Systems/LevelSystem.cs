using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Components.Events;
using Skibidi.Services;
using Skibidi.Views;

namespace Skibidi.Systems
{
    public class LevelSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsFilterInject<Inc<DieEvent>> _dieFilter = "events";
        private readonly EcsFilterInject<Inc<TaskEvent>> _taskFilter = "events";
        private EcsCustomInject<PlayerService> _playerService;
        private EcsCustomInject<SceneService> _sceneService;
        private EcsPoolInject<UnitCmp> _unitCmpPool;

        private int _currentIndex = 0;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dieFilter.Value)
            {
                var pool = _dieFilter.Pools.Inc1;
                ref var dieEvent = ref pool.Get(entity);

                var view = dieEvent.View;

                pool.Del(entity);
                
                ref var unit = ref view.GetUnitCmpByView();

                if (unit.Type == UnitType.Hero)
                {
                    //Game over
                    continue;
                }
                
                _unitCmpPool.Value.Del(view.PackedEntityWithWorld.Id);
                MoveOn();
            }

            foreach (var entity in _taskFilter.Value)
            {
                var pool = _taskFilter.Pools.Inc1;
                pool.Del(entity);
                
                MoveOn();
            }
        }

        private void MoveOn()
        {
            var hero = _sceneService.Value.Player;
            
            var target = hero.GetNearestTarget();

            if (target != null)
            {
                hero.LookAtTarget(target.transform);
                return;
            }
                
            hero.ResetLook();
            
            if (_sceneService.Value.EnemyByRows.Length <= _currentIndex)
            {
                //Game over
                return;
            }
            
            SendPlayerMoveEvent(hero);
            StartEnemyMove();
        }

        private void SendPlayerMoveEvent(UnitView playerView)
        {
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<MovementEvent>().Add(entity);
            eventComponent.View = playerView;
            eventComponent.Status = MoveStatus.Begin;
        }

        private void StartEnemyMove()
        {
            foreach (var unit in _sceneService.Value.EnemyByRows[_currentIndex].Units)
            {
                var entity = _eventWorld.Value.NewEntity();
                ref var eventComponent = ref _eventWorld.Value.GetPool<MovementEvent>().Add(entity);
                eventComponent.View = unit;
                eventComponent.Status = MoveStatus.Begin;
            }

            _currentIndex++;
        }
    }
}