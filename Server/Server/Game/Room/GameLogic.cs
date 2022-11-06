using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	public class GameLogic : JobSerializer
	{
		public static GameLogic Instance { get; } = new GameLogic();

		Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
		//Dictionary<int, PvpRoom> _pvpRooms = new Dictionary<int, PvpRoom>();

		int _roomId = 1;

		public void Update()
		{
			Flush();

			foreach (PveRoom room in _rooms.Values)
			{
				room.Update();
			}

		}
		public GameRoom Add(int mapId, bool pvp = false)
		{
			GameRoom gameRoom = new GameRoom();
			gameRoom.Pvp = pvp;
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

		public GameRoom Find(int roomId)
		{
			GameRoom room = null;
			if (_rooms.TryGetValue(roomId, out room))
				return room;

			return null;
		}

		public List<GameRoom> GetGameRooms()
        {
			return new List<GameRoom>(_rooms.Values);
        }
	}
}
