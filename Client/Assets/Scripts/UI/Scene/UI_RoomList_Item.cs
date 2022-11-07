using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomList_Item : UI_Base
{
	enum Texts
    {
		IdText,
		PvpText,
    }

	public int roomId { get; private set; }
	public int mapId { get; private set; }
	public bool pvp { get; private set; }

	public override void Init()
	{
		Bind<Text>(typeof(Texts));

		gameObject.BindEvent((e) =>
		{
			Debug.Log("Click Room");
			Debug.Log($"Target Room Id = {roomId}");
			C_LeaveGame leavePacket = new C_LeaveGame();
			leavePacket.PlayerId = Managers.Object.MyPlayer.Id;
			leavePacket.TargetRoom = roomId;
			leavePacket.Change = true;
			Managers.Network.Send(leavePacket);
		});
	}

	public void SetItem(RoomInfo info)
	{
		if (info == null)
			return;

		roomId = info.RoomId;
		mapId = info.MapId;
		pvp = info.Pvp;

		GetText((int)Texts.IdText).text = info.RoomId.ToString();
		GetText((int)Texts.PvpText).text = pvp ? "PvP On" : "PvP Off";
	}
}
