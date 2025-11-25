using System.Collections.Generic;
using Gameplay.Engine.Abilities;

namespace Gameplay.Engine.Tiles
{
    public interface IAbilityTile
    {
        IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context);
    }
}