using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Skibidi.Services;
using UnityEngine;

namespace Skibidi.Systems
{
    public class CameraSystem : IEcsRunSystem
    {
        private EcsCustomInject<SceneService> _sceneService;
        private Vector3 _currentVelocity;
        
        public void Run(IEcsSystems systems)
        {
            var target = _sceneService.Value.Player.transform;
            var transform = _sceneService.Value.MainCamera.transform;
            
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                target.position + new Vector3(0, 13, -20), 
                ref _currentVelocity, 
                .25f
            );
        }
    }
}