using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Components.Events;
using Skibidi.Services;
using UnityEngine.SceneManagement;

namespace Skibidi.Systems
{
    public class UISystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<GameEndEvent>> _gameEndFilter = "events";
        private EcsCustomInject<PlayerService> _playerService;
        private EcsCustomInject<SceneService> _sceneService;
        private readonly EcsWorldInject _eventWorld = "events";
        private EcsPoolInject<UnitCmp> _unitCmpPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _gameEndFilter.Value)
            {
                var pool = _gameEndFilter.Pools.Inc1;
                ref var gameEndEvent = ref pool.Get(entity);
                var isWin = gameEndEvent.IsWin;

                if (isWin)
                {
                    SendDefendEndEvent();
                }

                var popup = _sceneService.Value.PopupView;

                var text = isWin ? "You Win!" : "You Lose :(";
                    
                popup.SetStatusText(text);
                popup.StartButton.onClick.RemoveAllListeners();
                popup.StartButton.onClick.AddListener(StatAgain);
                popup.Show();
            }
        }

        private void StatAgain()
        {
            var popup = _sceneService.Value.PopupView;
            popup.Hide();
            
            /*var entity = _eventWorld.Value.NewEntity();
            _eventWorld.Value.GetPool<GameStartEvent>().Add(entity);*/

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SendDefendEndEvent()
        {
            var mainHero = _unitCmpPool.Value.
                Get(_playerService.Value.PackedEntityWithWorld.Id);
            
            var entity = _eventWorld.Value.NewEntity();
            ref var eventComponent = ref _eventWorld.Value.GetPool<DefendEvent>().Add(entity);
            eventComponent.View = mainHero.View;
            eventComponent.IsActive = false;
        }
    }
}