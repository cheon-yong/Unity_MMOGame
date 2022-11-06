using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoomList : UI_Base
{
	public List<UI_RoomList_Item> rooms { get; } = new List<UI_RoomList_Item>();

	public List<RoomInfo> roomInfos { get; set; } = new List<RoomInfo>();

	public override void Init()
	{
		Clear();
		RequestRoomInfos();
		//RefreshUI();
	}

    private void OnDisable()
    {
		Clear();
    }

    private void OnEnable()
    {
		RequestRoomInfos();
    }

    public void RequestRoomInfos()
    {
		C_RequestRooms requestPacket = new C_RequestRooms();
		Managers.Network.Send(requestPacket);
    }

	public void Clear()
    {
		rooms.Clear();
		roomInfos.Clear();

		GameObject layout = transform.Find("ItemGrid").gameObject;
		foreach (Transform child in layout.transform)
			Destroy(child.gameObject);
	}

	public void RefreshUI()
	{
		if (roomInfos.Count == 0)
			return;
		
		GameObject grid = transform.Find("ItemGrid").gameObject;
		for (int i = 0; i < roomInfos.Count; i++)
		{
			GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_RoomList_Item", grid.transform);
			UI_RoomList_Item item = go.GetOrAddComponent<UI_RoomList_Item>();
			item.SetItem(roomInfos[i]);
			rooms.Add(item);
		}
	}
}
