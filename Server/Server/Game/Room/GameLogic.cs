using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	public class GameLogic : JobSerializer
	{
		public static GameLogic Instance { get; } = new GameLogic();

		Dictionary<int, PveRoom> _rooms = new Dictionary<int, PveRoom>();
		int _roomId = 1;

		public void Update()
		{
			Flush();

			foreach (PveRoom room in _rooms.Values)
			{
				room.Update();
			}
		}

		public PveRoom Add(int mapId)
		{
			PveRoom gameRoom = new PveRoom();
			gameRoom.Push(gameRoom.Init, mapId, 10);

			gameRoom.RoomId = _roomId;
			_rooms.Add(_roomId, gameRoom);
			_roomId++;

			return gameRoom;
		}

		public bool Remove(int roomId)
		{
			return _rooms.Remove(roomId);
		}

		public PveRoom Find(int roomId)
		{
			PveRoom room = null;
			if (_rooms.TryGetValue(roomId, out room))
				return room;

			return null;
		}
	}
}
