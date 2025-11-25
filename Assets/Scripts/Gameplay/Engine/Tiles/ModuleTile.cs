using System.Collections.Generic;
using Gameplay.Engine.Abilities;
using Gameplay.Engine.Moves;

namespace Gameplay.Engine.Tiles
{
    public abstract class ModuleTile : Tile, IMovingTile, IAbilityTile
    {
        protected readonly IMovementBehavior movementBehavior;
        protected readonly IAbilityBehavior abilityBehavior;
    
        protected ModuleTile(
            int id,
            string typeKey,
            IMovementBehavior movementBehavior,
            IAbilityBehavior abilityBehavior)
            : base(id, typeKey)
        {
            this.movementBehavior = movementBehavior;
            this.abilityBehavior = abilityBehavior;
        }

        // IMovingTile
        public IReadOnlyList<MoveOption> GetAvailableMoves(MoveContext context)
            => movementBehavior.GetAvailableMoves(context);

        // IAbilityTile
        public bool IsAbilityAvailable => abilityBehavior.IsAbilityAvailable;

        public IReadOnlyList<AbilityOption> GetAbilityOptions(AbilityContext context)
            => abilityBehavior.GetAbilityOptions(context);

        // public void UseAbility(AbilityContext context, AbilityOption option)
        //    => abilityBehavior.UseAbility(context, option);
    }
}