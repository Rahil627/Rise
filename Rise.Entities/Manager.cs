﻿using System;
namespace Rise.Entities
{
    public class Manager
    {
        public event Action OnAdded;
        public event Action<Scene> OnRemoved;
        public event Action<Component> OnComponentAdded;
        public event Action<Component> OnComponentRemoved;
        public event Action OnCleanup;

        public Scene Scene { get; private set; }

        bool cleanup;
        bool changeLock;

        internal void ComponentAdded(Component component)
        {
            OnComponentAdded?.Invoke(component);
        }

        internal void ComponentRemoved(Component component)
        {
            OnComponentRemoved?.Invoke(component);
        }

        internal void Added(Scene scene)
        {
            if (changeLock)
                throw new Exception("Cannot add a manager in its add/remove callbacks.");

            changeLock = true;

            Scene = scene;
            OnAdded?.Invoke();

            changeLock = false;
        }

        internal void Removed()
        {
            if (changeLock)
                throw new Exception("Cannot remove a manager in its add/remove callbacks.");
            
            changeLock = true;

            var scene = Scene;
            Scene = null;
            OnRemoved?.Invoke(scene);

            changeLock = false;
        }

        protected void TriggerCleanup()
        {
            if (!cleanup)
            {
                cleanup = true;
                Scene.TriggerCleanupManagers();
            }
        }

        internal void Cleanup()
        {
            if (cleanup)
            {
                cleanup = false;
                OnCleanup?.Invoke();
            }
        }
    }
}
