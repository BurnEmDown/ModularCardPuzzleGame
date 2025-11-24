using System.Collections.Generic;
using Gameplay.Engine.Abilities;

namespace Gameplay.Engine.Tiles
{
    public interface IAbilityTile
    {
        bool IsAbilityAvailable { get; }

        bool CanUseAbility(AbilityContext context);

        IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context);
    }
}