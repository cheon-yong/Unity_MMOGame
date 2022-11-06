using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_Stat StatUI { get; private set; }
    public UI_Inventory InvenUI { get; private set; }

    public UI_RoomList roomList { get; private set; }

    public override void Init()
	{
        base.Init();

        StatUI = GetComponentInChildren<UI_Stat>();
        InvenUI = GetComponentInChildren<UI_Inventory>();
        roomList = GetComponentInChildren<UI_RoomList>();

        StatUI.gameObject.SetActive(false);
        InvenUI.gameObject.SetActive(false);
        roomList.gameObject.SetActive(false);
	}
}
