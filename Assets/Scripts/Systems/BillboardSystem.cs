using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Components;
using Skibidi.Services;
using UnityEngine;

namespace Skibidi.Systems
{
    public class BillboardSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsPoolInject<UnitCmp> _unitCmpPool;
        private EcsFilterInject<Inc<UnitCmp>> _unitCmpFilter;
        private readonly EcsWorldInject _eventWorld = "events";
        private EcsCustomInject<SceneService> _sceneService;

        private Camera _mainCamera;
        
        public void Init(IEcsSystems systems)
        {
            _mainCamera = _sceneService.Value.MainCamera;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitCmpFilter.Value)
            {
                var unitCmp = _unitCmpPool.Value.Get(entity);

                var view = unitCmp.View;

                view.UpdateSliderBillboard(_mainCamera.transform.forward);
            }
        }
    }
}