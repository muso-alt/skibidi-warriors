using Skibidi.Views;

namespace Skibidi.Components
{
    public struct UnitCmp
    {
        public bool IsMoving;
        
        public UnitType Type;
        public UnitView View;
        
        public int Damage;
        public int Health;

        public float MoveSpeed;
        
        public float LastPunch;
        public float PunchInterval;
        public float PunchDuration;
    }
}