using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNotificationManager : MonoBehaviour
{

    //[SerializeField] private GameObject notificationList;

    public List<ItemNotificationPanel> notifications = new List<ItemNotificationPanel>();
    public GameObject notificationPanelPrefab;

    [SerializeField] private float newPanelTimer;



    public void NewItem(Sprite icon, int count)
    {
        bool newPanel = true;

        foreach(ItemNotificationPanel panel in notifications)
        {
            if(panel.notifLifetime < newPanelTimer)
            {
                newPanel = false;
                panel.NewItem(icon, count);
            }
        }
        if (newPanel)
        {
        var notif = Instantiate(notificationPanelPrefab, transform.position, Quaternion.identity, transform).GetComponent<ItemNotificationPanel>();

        notif.manager = this;

        notif.NewItem(icon, count); 

        notifications.Add(notif);
        }

    }

}
