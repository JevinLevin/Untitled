using UnityEngine;

public class SpriteTexturePadding : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;

    public void AddPaddingToSprite(int paddingAmount)
    {
        if (targetSpriteRenderer == null)
        {
            Debug.LogWarning("No target sprite renderer assigned.");
            return;
        }

        Sprite sprite = targetSpriteRenderer.sprite;
        if (sprite == null)
        {
            Debug.LogWarning("Target sprite renderer does not have a sprite.");
            return;
        }

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
        targetSpriteRenderer.sprite = newSprite;
    }
}