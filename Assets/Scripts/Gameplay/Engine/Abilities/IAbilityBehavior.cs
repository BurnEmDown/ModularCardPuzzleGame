using System.Collections.Generic;

namespace Gameplay.Engine.Abilities
{
    public interface IAbilityBehavior
    {
        bool IsAbilityAvailable { get; }

        IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context);
    }
}