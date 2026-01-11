using UnityCoreKit.Runtime.UserInteractions;

namespace Gameplay.Presentation.UserInteractions
{
    public interface IUserInteractionEffect
    {
        bool CanApply(UserInteractionEvent evt);
        void Apply(UserInteractionEvent evt);
    }
}