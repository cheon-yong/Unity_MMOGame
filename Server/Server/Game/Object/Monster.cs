using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    class Monster : GameObject
    {
        public Monster()
        {
            ObjectType = GameObjectType.Monster; 
        }
    }
}
