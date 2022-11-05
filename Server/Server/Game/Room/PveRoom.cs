using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Game
{
	public partial class PveRoom : GameRoom
	{
        public override void EnterGame(GameObject gameObject, bool randomPos)
        {
            base.EnterGame(gameObject, randomPos);
        }

    }
}
