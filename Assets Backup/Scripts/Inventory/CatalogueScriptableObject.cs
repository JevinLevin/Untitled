using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Catalogue", menuName = "ScriptableObjects/Catalogues")]
public class CatalogueScriptableObject : ScriptableObject
{

    public List<InventoryItem> Catalogue;

}
