using Leopotam.EcsLite;

namespace Skibidi.Services
{
    public class PlayerService
    {
        public bool GameOver { get; set; }
        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; set; }
    }
}