using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles
{
    public sealed class TileConfig
    {
        public string TypeKey { get; set; } = "";
        public string DisplayName { get; set; } = "";

        public MovementRules MovementRules { get; set; } = new();
        public IAbilityBehavior AbilityBehavior { get; set; } = new DefaultAbilityBehavior();
    }
}