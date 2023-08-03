using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtOYoungSkins
{
    internal static class Util
    {
        public static Sprite CreateFlippedSprite(Sprite originalSprite)
        {
            if (originalSprite == null)
            {
                Debug.LogError("Original sprite is null!");
                return null;
            }

            // Create a new Texture2D object with the same dimensions as the original sprite
            Texture2D flippedTexture = new Texture2D((int)originalSprite.rect.width, (int)originalSprite.rect.height);

            // Get the pixels from the original sprite
            Color[] originalPixels = originalSprite.texture.GetPixels((int)originalSprite.textureRect.x,
                                                                      (int)originalSprite.textureRect.y,
                                                                      (int)originalSprite.textureRect.width,
                                                                      (int)originalSprite.textureRect.height);

            // Flip the pixels horizontally and copy them to the new Texture2D object
            for (int y = 0; y < originalSprite.rect.height; y++)
            {
                for (int x = 0; x < originalSprite.rect.width; x++)
                {
                    flippedTexture.SetPixel((int)originalSprite.rect.width - x - 1, y, originalPixels[x + y * (int)originalSprite.rect.width]);
                }
            }

            // Apply the changes to the new Texture2D
            flippedTexture.Apply();

            // Create a new sprite using the flipped Texture2D
            Sprite flippedSprite = Sprite.Create(flippedTexture,
                                                 new Rect(0, 0, flippedTexture.width, flippedTexture.height),
                                                 new Vector2(0.5f, 0.5f),
                                                 originalSprite.pixelsPerUnit,
                                                 0,
                                                 SpriteMeshType.FullRect);

            return flippedSprite;
        }
    }
}
