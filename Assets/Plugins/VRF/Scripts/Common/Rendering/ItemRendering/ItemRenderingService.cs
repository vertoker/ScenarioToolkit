using System.Collections.Generic;
using UnityEngine;
using VRF.Inventory.Scriptables;

namespace VRF.Utils.Rendering.ItemRendering
{
    public class ItemRenderingService
    {
        private Dictionary<InventoryItemConfig, Texture2D> itemsPreviews = new();
        
        private readonly Camera camera;
        private readonly Vector2Int textureSize;
        private readonly Vector3 cameraPosition;

        private readonly int layer = 1 << LayerMask.NameToLayer("Item");

        public ItemRenderingService(Camera camera, Vector2Int textureSize, Vector3 cameraPosition)
        {
            this.camera = camera;
            this.textureSize = textureSize;
            this.cameraPosition = cameraPosition;
        }
        
        public Texture2D GetItemPreview(InventoryItemConfig itemConfig)
        {
            if (itemsPreviews.TryGetValue(itemConfig, out var createdPreview))
            {
                return createdPreview;
            }

            var rt = new RenderTexture(textureSize.x, textureSize.y, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = rt;
            
            var itemView = itemConfig.ItemView;
            var item = Object.Instantiate(itemView);
            var itemTransform = item.transform;
            itemTransform.rotation *= Quaternion.Euler(itemConfig.LocalRotationOffset);

            var itemGameObject = item.gameObject;
            var itemBounds = GetObjectBounds(itemGameObject);
            var itemSize = itemBounds.size;
            var horizontalFitDistance = CalculateFitDistance(itemSize.x, camera.fieldOfView * camera.aspect);
            var verticalFitDistance = CalculateFitDistance(itemSize.y, camera.fieldOfView);
            var distance = Mathf.Max(horizontalFitDistance, verticalFitDistance);
            var position = cameraPosition
                + new Vector3(0, 0, distance + itemSize.z * 0.75f)
                - itemBounds.center + itemTransform.position
                + itemConfig.PositionOffset;

            itemTransform.position = position;
            
            var cameraTransform = camera.transform;

            cameraTransform.rotation = Quaternion.identity;
            cameraTransform.position = cameraPosition;
            camera.cullingMask = layer;
            camera.Render();
            
            itemGameObject.SetActive(false);
            Object.Destroy(itemGameObject);

            var preview = ToTexture2D(rt);
            
            itemsPreviews.Add(itemConfig, preview);
            
            return preview;
        }

        private static float CalculateFitDistance(float size, float angle) =>
            size / 2 / Mathf.Tan(Mathf.Deg2Rad * angle / 2);
        
        private static Bounds GetObjectBounds(GameObject gameObject)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
            {
                return new Bounds();
            }
            
            var result = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                result.Encapsulate(renderer.bounds);
            }

            return result;
        }
        
        private static Texture2D ToTexture2D(RenderTexture rTex)
        {
            var tex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false);
            var currentRenderTexture = RenderTexture.active;
            RenderTexture.active = rTex;

            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();

            RenderTexture.active = currentRenderTexture;
            return tex;
        }
    }
}