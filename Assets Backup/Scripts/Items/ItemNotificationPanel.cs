using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNotificationPanel : MonoBehaviour
{

    public float notifLifetime;
    [SerializeField] private float notifMaxLifetime;
    [SerializeField] private AnimationCurve hoverCurve;
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject panelObject;

    public ItemNotificationManager manager;

    private List<ItemNotificationItem> notificationItems = new List<ItemNotificationItem>();



    void Update()
    {
        notifLifetime += Time.deltaTime;
        var yIncrease = Mathf.Lerp(1.8f,0.15f,hoverCurve.Evaluate(notifLifetime/notifMaxLifetime));

        transform.position += new Vector3(0,yIncrease,0)*Time.deltaTime;
    }

    public void Reposition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    public void NewItem(Sprite icon, int count)
    {
        transform.position = new Vector2((Player.Instance.transform.position.x-0.8f) + (Player.Instance.MovementDirection/-1*1.5f), Player.Instance.transform.position.y + 1f);

        notifLifetime = 0;

        var notif = Instantiate(notificationPrefab, transform.position, Quaternion.identity, panelObject.transform).GetComponent<ItemNotificationItem>();

        notif.Setup(icon, count, notifMaxLifetime, this);

        notificationItems.Add(notif);

    }

    public void CheckEmpty(ItemNotificationItem itemNotification)
    {

        notificationItems.Remove(itemNotification);

        if(notificationItems.Count > 0)
        {
            Reposition();
        }
        else
        {
            manager.notifications.Remove(this);
            Destroy(gameObject);
        }

    }

}
