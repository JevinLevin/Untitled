using UnityEditor;
using UnityEngine;

public class SpriteAssetTexurePadding : Editor
{
    [MenuItem("Custom/Batch Add Padding to Sprites")]
    private static void BatchAddPaddingToSprites()
    {
        // Find all sprite assets in the "Sprites" folder
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });

        foreach (string spriteGuid in spriteGuids)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(spriteGuid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            // Apply padding to the sprite
            ApplyPaddingToSprite(sprite);
        }

        // Refresh the Asset Database to save changes
        AssetDatabase.Refresh();
    }

    public void AddPaddingToSprite(Sprite sprite)
    {

        Texture2D originalTexture = sprite.texture;
        if (originalTexture == null)
        {
            Debug.LogWarning("Sprite does not have a texture.");
            return;
        }

        // Calculate new texture dimensions
        int newWidth = originalTexture.width + 2 * paddingAmount;
        int newHeight = originalTexture.height + 2 * paddingAmount;

        // Create a new texture with the same settings as the original
        Texture2D newTexture = new Texture2D(newWidth, newHeight, originalTexture.format, originalTexture.mipmapCount > 1);

        // Preserve filter mode and compression settings
        newTexture.filterMode = originalTexture.filterMode;
        newTexture.anisoLevel = originalTexture.anisoLevel;
        newTexture.wrapMode = originalTexture.wrapMode;

        // Clear the new texture to transparent
        Color[] transparentPixels = new Color[newWidth * newHeight];
        for (int i = 0; i < transparentPixels.Length; i++)
        {
            transparentPixels[i] = Color.clear;
        }
        newTexture.SetPixels(transparentPixels);

        // Copy the original texture to the center of the new texture
        for (int y = 0; y < originalTexture.height; y++)
        {
            for (int x = 0; x < originalTexture.width; x++)
            {
                Color pixel = originalTexture.GetPixel(x, y);
                newTexture.SetPixel(x + paddingAmount, y + paddingAmount, pixel);
            }
        }

        // Apply changes to the new texture
        newTexture.Apply();

        // Update the sprite with the new texture
        Rect newRect = new Rect(sprite.rect.x, sprite.rect.y, newWidth, newHeight);
        Vector2 newPivot = new Vector2((sprite.pivot.x + paddingAmount) / newWidth, (sprite.pivot.y + paddingAmount) / newHeight);
        Sprite newSprite = Sprite.Create(newTexture, newRect, newPivot, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);
    }
}