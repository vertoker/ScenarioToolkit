#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

[InitializeOnLoad]
public class CameraOrientation : UnityEditor.Editor
{
    private static bool wasMovedByMouse;
    private static double middleClickTime;
    private static Vector2 middleClickPos;

    private const double clickDeltaTime = 0.1;

    private static readonly Dictionary<Direction, ViewDirection> ViewDirections =
        new Dictionary<Direction, ViewDirection>()
        {
            {
                Direction.Front,
                new ViewDirection(Quaternion.Euler(0, 180, 0), Direction.Top, Direction.Down, Direction.Left,
                    Direction.Right)
            },
            {
                Direction.Back,
                new ViewDirection(Quaternion.Euler(0, 0, 0), Direction.Top, Direction.Down, Direction.Right,
                    Direction.Left)
            },
            {
                Direction.Left,
                new ViewDirection(Quaternion.Euler(0, 90, 0), Direction.Top, Direction.Down, Direction.Back,
                    Direction.Front)
            },
            {
                Direction.Right,
                new ViewDirection(Quaternion.Euler(0, -90, 0), Direction.Top, Direction.Down, Direction.Front,
                    Direction.Back)
            },
            {
                Direction.Top,
                new ViewDirection(Quaternion.Euler(90, 0, 0), Direction.Front, Direction.Back, Direction.Right,
                    Direction.Left)
            },
            {
                Direction.Down,
                new ViewDirection(Quaternion.Euler(-90, 0, 0), Direction.Back, Direction.Front, Direction.Right,
                    Direction.Left)
            },
        };

    static CameraOrientation()
    {
        SceneView.duringSceneGui += SceneView_OnDuringSceneGui;
    }

    private static void SceneView_OnDuringSceneGui(SceneView sceneView)
    {
        var current = Event.current;
        var delta = current.delta;
        var eventType = current.type;
        var currentButton = current.button;
        var currentAlt = current.alt;

        if (eventType is EventType.Repaint or EventType.Layout)
        {
            return;
        }

        if (eventType == EventType.MouseDown && currentButton == 2)
        {
            middleClickTime = EditorApplication.timeSinceStartup;
            middleClickPos = current.mousePosition;
        }

        if (wasMovedByMouse == false && eventType == EventType.MouseDrag && currentAlt && currentButton == 2)
        {
            wasMovedByMouse = true;
            
            var x = delta.x;
            var y = delta.y;
            var closestViewDirection = GetClosestViewDirection();
            var newDirection = x < y
                ? x > -y
                    ? closestViewDirection.UpDirection
                    : closestViewDirection.RightDirection
                : x > -y
                    ? closestViewDirection.LeftDirection
                    : closestViewDirection.DownDirection;

            MakeSceneViewCameraLookAtPivot(newDirection);
        }

        if (eventType == EventType.MouseUp)
        {
            wasMovedByMouse = false;

            if (currentAlt && currentButton == 2 &&
                EditorApplication.timeSinceStartup - middleClickTime < clickDeltaTime &&
                Vector2.Distance(current.mousePosition, middleClickPos) < 0.1)
            {
                ChangePerspectiveCamera();
            }
        }

        if (wasMovedByMouse)
        {
            current.Use();
        }
    }

    private static ViewDirection GetClosestViewDirection()
    {
        var camera = SceneView.lastActiveSceneView.camera;
        var currentView = camera.transform.rotation;

        var result = ViewDirections[Direction.Back];
        var minAngle = float.MaxValue;

        foreach (var (direction, viewDirection) in ViewDirections)
        {
            var delta = Quaternion.Angle(currentView, viewDirection.Rotation);

            if (delta < minAngle)
            {
                result = viewDirection;
                minAngle = delta;
            }
        }

        return result;
    }

    [Shortcut("Scene View Camera - Front view", KeyCode.Keypad1)]
    public static void FrontView()
    {
        MakeSceneViewCameraLookAtPivot(Direction.Front);
    }

    [Shortcut("Scene View Camera - Back view", KeyCode.Keypad1, ShortcutModifiers.Alt)]
    public static void BackView()
    {
        MakeSceneViewCameraLookAtPivot(Direction.Back);
    }

    [Shortcut("Scene View Camera - Left view", KeyCode.Keypad3)]
    public static void LeftView()
    {
        MakeSceneViewCameraLookAtPivot(Direction.Left);
    }

    [Shortcut("Scene View Camera - Right view", KeyCode.Keypad3, ShortcutModifiers.Alt)]
    public static void RightView()
    {
        MakeSceneViewCameraLookAtPivot(Direction.Right);
    }

    [Shortcut("Scene View Camera - Top view", KeyCode.Keypad7)]
    public static void TopView()
    {
        MakeSceneViewCameraLookAtPivot(Direction.Top);
    }

    [Shortcut("Scene View Camera - Down view", KeyCode.Keypad7, ShortcutModifiers.Alt)]
    public static void DownView()
    {
        MakeSceneViewCameraLookAtPivot(Direction.Down);
    }

    [Shortcut("Scene View Camera - Perspective Switch", KeyCode.Keypad5)]
    public static void ChangePerspectiveCamera()
    {
        SceneView.lastActiveSceneView.orthographic = !SceneView.lastActiveSceneView.orthographic;
    }

    [Shortcut("Scene View Camera - Opposite view", KeyCode.Keypad9)]
    public static void ToggleView()
    {
        var sceneView = SceneView.lastActiveSceneView;
        var camera = sceneView.camera;
        var currentView = camera.transform.rotation.eulerAngles;

        MakeSceneViewCameraLookAtPivot(currentView.magnitude < 0.00001
            ? Quaternion.Euler(0, 180, 0)
            : Quaternion.LookRotation(sceneView.camera.transform.position - sceneView.pivot));
    }

    private static void MakeSceneViewCameraLookAtPivot(Direction direction)
    {
        MakeSceneViewCameraLookAtPivot(ViewDirections[direction].Rotation);
    }

    private static void MakeSceneViewCameraLookAtPivot(Quaternion direction)
    {
        //introduce the scene window that we want to do sth to
        var sceneView = SceneView.lastActiveSceneView;

        //if there is no scene window do nothing
        if (sceneView == null) return;

        //Get the pivot
        var pivot = sceneView.pivot;


        sceneView.LookAt(pivot, direction);
    }

    private readonly struct ViewDirection
    {
        public readonly Quaternion Rotation;
        public readonly Direction UpDirection;
        public readonly Direction DownDirection;
        public readonly Direction RightDirection;
        public readonly Direction LeftDirection;

        public ViewDirection(Quaternion rotation, Direction upDirection, Direction downDirection,
            Direction rightDirection, Direction leftDirection)
        {
            Rotation = rotation;
            UpDirection = upDirection;
            DownDirection = downDirection;
            RightDirection = rightDirection;
            LeftDirection = leftDirection;
        }
    }

    private enum Direction
    {
        Front = 0,
        Back = 1,
        Left = 2,
        Right = 3,
        Top = 4,
        Down = 5,
    }
}
#endif