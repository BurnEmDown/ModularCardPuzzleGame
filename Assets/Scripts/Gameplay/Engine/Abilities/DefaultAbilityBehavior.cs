using System.Collections.Generic;

namespace Gameplay.Engine.Abilities
{
    public class DefaultAbilityBehavior : IAbilityBehavior
    {
        public bool IsAbilityAvailable { get; }
        
        public DefaultAbilityBehavior()
        {
            
        }
        
        public IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}