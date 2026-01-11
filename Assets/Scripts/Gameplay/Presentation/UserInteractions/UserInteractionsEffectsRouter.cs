using System;
using System.Collections.Generic;
using System.Linq;
using UnityCoreKit.Runtime.UserInteractions;

namespace Gameplay.Presentation.UserInteractions
{
    
    /// <summary>
    /// Routes user interactions to registered effect pipelines based on:
    /// (InteractionKey, UserInteractionType).
    /// Note that for now this class won't be used but it might be used in the future
    /// </summary>
    public sealed class UserInteractionsEffectsRouter : IDisposable
    {
        private readonly IUserInteractions interactions;

        private readonly Dictionary<(string key, UserInteractionType type), List<IUserInteractionEffect>>
            routes = new();
        
        public UserInteractionsEffectsRouter(IUserInteractions interactions)
        {
            this.interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
            interactions.Subscribe(this, OnInteraction);
        }
        
        public void Dispose()
        {
            interactions.Unsubscribe(this, OnInteraction);
        }

        public void Register(string interactionKey, UserInteractionType type, IUserInteractionEffect effect)
        {
            if(interactionKey == null) throw new ArgumentNullException(nameof(interactionKey));
            if(effect == null) throw new ArgumentNullException(nameof(effect));
            
            var k = (interactionKey, type);
            if (!routes.TryGetValue(k, out var list))
            {
                list = new List<IUserInteractionEffect>();
                routes[k] = list;
            }

            list.Add(effect);
        }

        private void OnInteraction(UserInteractionEvent evt)
        {
            if (evt.Target == null)
                return;
            
            var key = (evt.Target.InteractionKey, evt.Type);
            if (!routes.TryGetValue(key, out var effects))
                return;
            
            foreach (var effect in effects.Where(effect => effect.CanApply(evt)))
            {
                effect.Apply(evt);
            }
        }
    }
}