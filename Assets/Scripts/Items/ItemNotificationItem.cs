using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemNotificationItem : MonoBehaviour
{

    public ItemNotificationPanel panel;

    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI text;

    private float lifetime;
    private float maxLifetime;
    [SerializeField] private float despawnDelay;


    public void Setup(Sprite icon, int count, float time, ItemNotificationPanel parent)
    {
        panel = parent;

        maxLifetime = time;

        sprite.sprite = icon;
        text.text = "+" + count.ToString();

    }

    void Update()
    {
        lifetime += Time.deltaTime;

        if(lifetime > despawnDelay)
        {
            sprite.color = new Color(1.0f,1.0f,1.0f, Mathf.Lerp(1,0, (lifetime-despawnDelay)/(maxLifetime-despawnDelay)));
            text.alpha = Mathf.Lerp(1,0, (lifetime-despawnDelay)/(maxLifetime-despawnDelay));
        }

        if(lifetime >= maxLifetime)
        {
            Remove();
        }
    }

    void Remove()
    {
        panel.CheckEmpty(this);

        Destroy(gameObject);
    }

}
