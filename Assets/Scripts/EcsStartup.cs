using Skibidi.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Di;
using Skibidi.Services;
using UnityEngine;
using UnityEngine;

public sealed class EcsStartup : MonoBehaviour
{
    [SerializeField] private SceneService _sceneService;
    private EcsWorld _world;
    private IEcsSystems _systems;

    private void Awake()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems
            .Add (new StartSystem())
            .Add (new MoveSystem())
            .Add (new FightSystem())
            .Add (new HealthSystem())
            .Add (new CameraSystem())
            .Add (new LevelSystem())
            .AddWorld (new EcsWorld(), "events")
            .Inject(_sceneService)
            .Inject(new PlayerService())
            .Init();
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy() 
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }
            
        if (_world != null) 
        {
            _world.Destroy();
            _world = null;
        }
    }
}