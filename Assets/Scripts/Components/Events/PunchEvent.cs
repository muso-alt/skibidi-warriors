using Skibidi.Views;

namespace Skibidi.Components.Events
{
    public struct PunchEvent : ITimerComponent
    {
        public UnitView View;
        public float Time { get; }
    }
}