using System;
using EPOOutline;
using JetBrains.Annotations;
using KBCore.Refs;
using NaughtyAttributes;
using SimpleUI.Core;
using UnityEngine;
using UnityEngine.Rendering;
using VRF.BNG_Framework.Scripts.Components;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Players.Controllers;
using VRF.Players.Models;
using VRF.Players.Models.Player;
using VRF.Players.Scriptables;
using VRF.Utils.Colliders;
using Zenject;

namespace VRF.Components.Players.Views.Player
{
    /// <summary>
    /// Представление обычного локального игрока.
    /// Над ним никто не стоит, самый самостоятельный
    /// компонент среди игроков.
    /// Работает и создаётся всегда, даже если сеть отключена
    /// </summary>
    public abstract class BasePlayerView : BaseView, IControlPlayerView
    {
        [Header("Base")]
        [SerializeField, Self] private Transform self;
        [SerializeField] private Transform cameraPivot;
        [SerializeField, ReadOnly] private Camera cam;
        [SerializeField] private Transform itemSpawnPoint;
        
        [SerializeField] private Volume volume;
        [SerializeField] private ScreensManagerInstance gameUI;
        [SerializeField] private GroundProvider groundProvider;
        [SerializeField] private ScreenFader screenFader;
        
        [Header("Items Scripts")]
        [SerializeField] private PlayerTeleport playerTeleport;
        [SerializeField, ReadOnly] private Outliner[] outliners;
        
        [Header("Other")]
        [SerializeField] private ScreensManagerInstance infoUI;
        [SerializeField] private ScreensManagerInstance timerUI;
        [SerializeField] private ScreensManagerInstance cameraUI;
        [SerializeField] private SnapZone headEquipmentZone;
        
        public Transform Self => self;
        public Transform CameraPivot => cameraPivot;
        public Camera Camera => cam;
        public Transform ItemSpawnPoint => itemSpawnPoint;
        public Volume Volume => volume;
        public ScreensManagerInstance GameUI => gameUI;
        public GroundProvider GroundProvider => groundProvider;
        public ScreenFader ScreenFader => screenFader;
        
        public PlayerTeleport PlayerTeleport => playerTeleport;
        public Outliner[] Outliners => outliners;
        public SnapZone HeadEquipmentZone => headEquipmentZone;
        
        [CanBeNull] public ScreensManagerInstance InfoUI => infoUI;
        [CanBeNull] public ScreensManagerInstance TimerUI => timerUI;
        [CanBeNull] public ScreensManagerInstance CameraUI => cameraUI;
        
        protected override void OnValidate()
        {
            base.OnValidate();
            // pivot для камеры скорее нужен для разных переопределений разных видов камеры (WASD и VR)
            cam = cameraPivot ? cameraPivot.GetComponentInChildren<Camera>() : null;
            outliners = GetComponentsInChildren<Outliner>();
        }
        
        public bool IsNetPlayer => false;
        public PlayerIdentityConfig Identity { get; private set; }
        public PlayerControlModes CurrentMode { get; private set; }

        /// <summary>
        /// Инициализация локального игрока (да в View)
        /// </summary>
        public void Construct(PlayerIdentityConfig identity, PlayerControlModes currentMode)
        {
            Identity = identity;
            CurrentMode = currentMode;
        }
        public void Initialize()
        {
            InitializeInternal();
        }
        
        // Одна из особенностей этого View в том, что каждая его реализация сама решает
        // что и как будет этим view управлять, поэтому при создании игрока
        // вызывается Bind в Zenject контейнер тех систем, которые ему нужны для работы
        
        /// <summary>
        /// Бинд всех управляющих систем локального игрока
        /// </summary>
        public void BindView(DiContainer container, PlayerSpawnConfiguration configuration)
        {
            // Бинд самого себя
            //container.Bind(GetSelfType).FromInstance(this).AsSingle();
            // Бинд самого себя, но как базового типа
            //container.Bind<BasePlayerView>().FromInstance(this).AsSingle().NonLazy();
            // Бинд схемы управления
            container.Bind(GetSelfSchemeType).AsSingle();
            // Бинд конфига контроллеров
            container.Bind(GetControllersConfigType)
                .FromInstance(configuration.PlayerConfig).AsSingle();
            
            // Бинд всех систем для управления этим view
            container.Bind(GetBuilderType).AsSingle().WithArguments(this);
            // Так как происходит Resolve этого класса во время Initialize,
            // на нём отказываются работать все Zenject интерфейсы, Bind достаточно
        }
        
        protected abstract Type GetSelfSchemeType { get; }
        protected abstract Type GetControllersConfigType { get; }
        public abstract Type GetBuilderType { get; }
    }
}