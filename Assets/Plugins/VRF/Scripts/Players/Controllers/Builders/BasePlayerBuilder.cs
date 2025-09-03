using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Components.Players.Views.Player;
using Zenject;

namespace VRF.Players.Controllers.Builders
{
    /// <summary>
    /// Базовый для всех сервисов игрока класс, заставляющий в себя прокидывать нужные элементы.
    /// Все эти элементы определяют поведение сервиса.
    /// Как и чем будут управлять сервисы определяется внутри самого view
    /// </summary>
    /// <typeparam name="TView">View объект игрока для взаимодействия с Unity объектами игрока</typeparam>
    /// <typeparam name="TControlScheme">Выбранная схема управления</typeparam>
    /// <typeparam name="TConfig">Настройки для контроллеров</typeparam>
    public abstract class BasePlayerBuilder<TView, TControlScheme, TConfig> : IPlayerBuilder
    
        where TView : BasePlayerView
        where TControlScheme : IInputActionCollection2
        where TConfig : ScriptableObject
    {
        public readonly TView View;
        public readonly TControlScheme ControlScheme;
        public readonly TConfig Config;
        
        public bool IsInitialized { get; private set; }
        
        private readonly List<IInitializable> initializables = new();
        private readonly List<IDisposable> disposables = new();
        private readonly List<ITickable> tickables = new();
        private readonly List<IFixedTickable> fixedTickables = new();
        private readonly List<ILateTickable> lateTickables = new();
        
        protected BasePlayerBuilder(TView view, TControlScheme controlScheme, TConfig config)
        {
            View = view;
            ControlScheme = controlScheme;
            Config = config;
        }

        protected void AddExecutors(params object[] executors)
        {
            foreach (var executor in executors)
                AddExecutor(executor);
        }
        protected void AddExecutor(object executor)
        {
            if (executor is IInitializable initializable)
                initializables.Add(initializable);
            if (executor is IDisposable disposable)
                disposables.Add(disposable);
            if (executor is ITickable tickable)
                tickables.Add(tickable);
            if (executor is IFixedTickable fixedTickable)
                fixedTickables.Add(fixedTickable);
            if (executor is ILateTickable lateTickable)
                lateTickables.Add(lateTickable);
        }

        public virtual void BuilderInitialize()
        {
            foreach (var initializable in initializables)
                initializable.Initialize();
            IsInitialized = true;
        }
        public virtual void BuilderDispose()
        {
            IsInitialized = false;
            foreach (var disposable in disposables)
                disposable.Dispose();
        }

        public void Tick()
        {
            if (!IsInitialized) return;
            
            foreach (var tickable in tickables)
                tickable.Tick();
        }
        public void FixedTick()
        {
            if (!IsInitialized) return;
            
            foreach (var fixedTickable in fixedTickables)
                fixedTickable.FixedTick();
        }
        public void LateTick()
        {
            if (!IsInitialized) return;
            
            foreach (var lateTickable in lateTickables)
                lateTickable.LateTick();
        }
    }
}