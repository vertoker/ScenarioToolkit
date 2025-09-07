using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Editor.Utilities
{
    public static class EditorWindowUtils
    {
        public enum Anchor
        {
            Left,
            Top,
            Right,
            Bottom,
            Center,
            LeftTop,
            RightTop,
            LeftBottom,
            RightBottom,
        }
        
        /// <summary>
        /// Docks the second window to the first window at the given position
        /// TODO WORKS ONLY WITH SINGLE WINDOWS
        /// </summary>
        public static void Dock(this EditorWindow parent, EditorWindow child, Anchor position)
        {
            var mousePosition = GetFakeMousePosition(parent, position);

            var parentWrapper = new _EditorWindow(parent);
            var childWrapper = new _EditorWindow(child);
            var dockArea = new _DockArea(parentWrapper.m_Parent);
            var containerWindow = new _ContainerWindow(dockArea.window);
            var splitView = new _SplitView(containerWindow.rootSplitView);
            var dropInfo = splitView.DragOver(child, mousePosition);
            dockArea.s_OriginalDragSource = childWrapper.m_Parent;
            splitView.PerformDrop(child, dropInfo, mousePosition);
        }
        public static void Dock(this EditorWindow parent, EditorWindow child, Anchor position, Rect parentRect)
        {
            var mousePosition = GetFakeMousePosition(position, parentRect);

            var parentWrapper = new _EditorWindow(parent);
            var childWrapper = new _EditorWindow(child);
            var dockArea = new _DockArea(parentWrapper.m_Parent);
            var containerWindow = new _ContainerWindow(dockArea.window);
            var splitView = new _SplitView(containerWindow.rootSplitView);
            var dropInfo = splitView.DragOver(child, mousePosition);
            dockArea.s_OriginalDragSource = childWrapper.m_Parent;
            splitView.PerformDrop(child, dropInfo, mousePosition);
        }

        private static Vector2 GetFakeMousePosition(EditorWindow parent, Anchor position)
            => GetFakeMousePosition(position, parent.position);
        private static Vector2 GetFakeMousePosition(Anchor position, Rect parentRect)
        {
            var relativeMousePosition = Vector2.zero;
            // The 20 is required to make the docking work.
            // Smaller values might not work when faking the mouse position.
            const float offset = 20f;
            const float offsetCorner = 10f;
            
            relativeMousePosition = position switch
            {
                Anchor.Left => new Vector2(offset, parentRect.size.y / 2),
                Anchor.Top => new Vector2(parentRect.size.x / 2, offset),
                Anchor.Right => new Vector2(parentRect.size.x - offset, parentRect.size.y / 2),
                Anchor.Bottom => new Vector2(parentRect.size.x / 2, parentRect.size.y - offset),
                
                // TODO не всегда работает в угол
                Anchor.LeftTop => new Vector2(offsetCorner, offsetCorner),
                Anchor.RightTop => new Vector2(parentRect.size.x - offsetCorner, offsetCorner),
                Anchor.LeftBottom => new Vector2(offsetCorner, parentRect.size.y - offsetCorner),
                Anchor.RightBottom => new Vector2(parentRect.size.x - offsetCorner, parentRect.size.y - offsetCorner),
                
                _ => relativeMousePosition
            };
            
            //Debug.Log(parentRect.position);
            //Debug.Log(parentRect.size);
            //Debug.Log(relativeMousePosition);

            return parentRect.position + relativeMousePosition;
        }
        
        public static Vector2 GetAnchorPoint(this EditorWindow parent, Anchor position, float offset = 0f)
        {
            return GetAnchorPoint(parent.position, position, offset);
        }
        public static Vector2 GetLocalAnchorPoint(this EditorWindow parent, Anchor position, float offset = 0f)
        {
            return GetLocalAnchorPoint(parent.position, position, offset);
        }
        
        public static Vector2 GetAnchorPoint(Rect parentRect, Anchor position, float offset = 0f)
        {
            return parentRect.position + GetLocalAnchorPoint(parentRect, position, offset);
        }
        public static Vector2 GetLocalAnchorPoint(Rect parentRect, Anchor position, float offset = 0f)
        {
            var relativeMousePosition = Vector2.zero;
            
            relativeMousePosition = position switch
            {
                Anchor.Center => new Vector2(parentRect.size.x / 2, parentRect.size.y / 2),
                
                Anchor.Left => new Vector2(offset, parentRect.y / 2),
                Anchor.Top => new Vector2(parentRect.size.x / 2, offset),
                Anchor.Right => new Vector2(parentRect.size.x - offset, parentRect.size.y / 2),
                Anchor.Bottom => new Vector2(parentRect.size.x / 2, parentRect.size.y - offset),
                
                Anchor.LeftTop => new Vector2(offset, offset),
                Anchor.RightTop => new Vector2(parentRect.size.x - offset, offset),
                Anchor.LeftBottom => new Vector2(offset, parentRect.size.y - offset),
                Anchor.RightBottom => new Vector2(parentRect.size.x - offset, parentRect.size.y - offset),
                
                _ => relativeMousePosition
            };
            
            //Debug.Log(parentRect.position);
            //Debug.Log(parentRect.size);
            //Debug.Log(relativeMousePosition);

            return relativeMousePosition;
        }
        
        /*public static void DockEditorWindow(Vector2 mousePos, EditorWindow parent, EditorWindow child)
        {
            Vector2 screenPoint = GUIUtility.GUIToScreenPoint(mousePos);
            var area = parent.m_Parent as DockArea;
            var sv = area.m_parent as SplitView;
            var di = sv.DragOver(child, screenPoint);
            DockArea.s_OriginalDragSource = child.m_Parent;
            sv.PerformDrop(child, di, screenPoint);
        }*/
    }
}