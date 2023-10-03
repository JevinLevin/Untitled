using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector; 
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/Item")]
[System.Serializable]
public class ItemScriptableObject : ScriptableObject
{

    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    [HorizontalGroup("Split", 60)]
    [SerializeField] public Sprite ItemSprite;

    [VerticalGroup("Split/Right"),LabelWidth(120)]
    public string ItemID;
    [VerticalGroup("Split/Right"),LabelWidth(120)]
    [SerializeField] private string itemName;

    public void Drop(Vector3 position, int count)
    {
        ItemObject spawnedItem = Instantiate(Player.Instance.ItemPrefab, position, Quaternion.identity).GetComponent<ItemObject>();

        spawnedItem.itemID = ItemID;

        spawnedItem.Spawn(count);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemScriptableObject))]
public class ItemEditor : Editor
{
    public override Texture2D RenderStaticPreview(
        string assetPath, Object[] subAssets, int width, int height
    )
    {
        var item = (ItemScriptableObject) target;

        if (item == null || item.ItemSprite == null)
        {
            return null;
        }

        var texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(item.ItemSprite.texture, texture);
        texture.filterMode = FilterMode.Point;
        return texture;
    }

}
#endif