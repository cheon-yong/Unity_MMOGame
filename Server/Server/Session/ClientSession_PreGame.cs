using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DB;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class ClientSession : PacketSession
    {
		public int AccountDbId { get; set; }
		public List<LobbyPlayerInfo> LobbyPlayers { get; set; } = new List<LobbyPlayerInfo>();
        public void HandleLogin(C_Login loginPacket)
        {
			// TODO : 보안체크
			if (ServerState != PlayerServerState.ServerStateLogin)
				return;

			// TODO : 문제가 많다.
			// - 동시에 다른 사람이 같은 UniqueId을 보낸다면?
			// - 악의적으로 여러번 보낸다면
			// - 엉뚱한 타이멩에 이 패킷을 보낸다면?

			LobbyPlayers.Clear();

			using (AppDbContext db = new AppDbContext())
			{
				AccountDb findAccount = db.Accounts
					.Include(a => a.Players)
					.Where(a => a.AccountName == loginPacket.UniqueId).FirstOrDefault();

				if (findAccount != null)
				{
					// AccountId를 메모리에 기억
					AccountDbId = findAccount.AccountDbId;

					S_Login loginOk = new S_Login() { LoginOk = 1 };
					foreach (PlayerDb playerDb in findAccount.Players)
                    {
						LobbyPlayerInfo lobbyPlayer = new LobbyPlayerInfo()
						{
							Name = playerDb.PlayerName,
							StatInfo = new StatInfo()
							{
								Level = playerDb.Level,
								Hp = playerDb.Hp,
								MaxHp = playerDb.MaxHp,
								Speed = playerDb.Speed,
								Attack = playerDb.Attack,
								TotalExp = playerDb.TotalExp,
							}
						};

						// 메모리에도 들고 있다 -> DB 접근 최소화
						LobbyPlayers.Add(lobbyPlayer);

						// 패킷에 넣어준다
						loginOk.Players.Add(lobbyPlayer);
                    }
					Send(loginOk);

					ServerState = PlayerServerState.ServerStateLobby;
				}
				else
				{
					AccountDb newAccount = new AccountDb() { AccountName = loginPacket.UniqueId };
					db.Accounts.Add(newAccount);
					db.SaveChanges();

					// AccountId를 메모리에 기억
					AccountDbId = findAccount.AccountDbId;

					S_Login loginOk = new S_Login() { LoginOk = 1 };
					Send(loginOk);
				}
			}
		}

		public void HandleEnterGame (C_EnterGame enterGamepacket)
        {
			if (ServerState != PlayerServerState.ServerStateLobby)
				return;

			LobbyPlayerInfo playerInfo = LobbyPlayers.Find(p => p.Name == enterGamepacket.Name);
			if (playerInfo == null)
				return;

			MyPlayer = ObjectManager.Instance.Add<Player>();
			{
				MyPlayer.Info.Name = playerInfo.Name;
				MyPlayer.Info.PosInfo.State = CreatureState.Idle;
				MyPlayer.Info.PosInfo.MoveDir = MoveDir.Down;
				MyPlayer.Info.PosInfo.PosX = 0;
				MyPlayer.Info.PosInfo.PosY = 0;

				MyPlayer.Stat.MergeFrom(playerInfo.StatInfo);

				MyPlayer.Session = this;
			}

			ServerState = PlayerServerState.ServerStateGame;

			GameRoom room = RoomManager.Instance.Find(1);
			room.Push(room.EnterGame, MyPlayer);
		}

		public void HandleCreatePlayer (C_CreatePlayer createPacket)
        {
			// TODO : 보안체크
			if (ServerState != PlayerServerState.ServerStateLobby)
				return;

			using (AppDbContext db = new AppDbContext())
            {
				PlayerDb findPlayer = db.Players
					.Where(p => p.PlayerName == createPacket.Name).FirstOrDefault();

				if (findPlayer != null)
                {
					// 이름이 겹친다
					Send(new S_CreatePlayer());
                }
				else
                {
					// 1레벨 스탯 정보 추출
					StatInfo stat = null;
					DataManager.StatDict.TryGetValue(1, out stat);

					// DB에 플레이어 만들어줘야 함
					PlayerDb newPlayerDb = new PlayerDb()
					{
						PlayerName = createPacket.Name,
						Level = stat.Level,
						Hp = stat.Hp,
						MaxHp = stat.MaxHp,
						Attack = stat.Attack,
						Speed = stat.Speed,
						TotalExp = 0,
						AccountDbId = AccountDbId
					};

					db.Players.Add(newPlayerDb);
					db.SaveChanges(); // TODO : ExceptionHandling

					// 메모리에 추가
					LobbyPlayerInfo lobbyPlayer = new LobbyPlayerInfo()
					{
						Name = createPacket.Name,
						StatInfo = new StatInfo()
						{
							Level = stat.Level,
							Hp = stat.Hp,
							MaxHp = stat.MaxHp,
							Speed = stat.Speed,
							Attack = stat.Attack,
							TotalExp = stat.TotalExp,
						}
					};

					// 메모리에도 들고 있다 -> DB 접근 최소화
					LobbyPlayers.Add(lobbyPlayer);

					// 패킷에 넣어준다
					S_CreatePlayer newPlayer = new S_CreatePlayer() { Player = new LobbyPlayerInfo() };
					newPlayer.Player.MergeFrom(lobbyPlayer);

					Send(newPlayer);
				}
            }
        }
    }
}
