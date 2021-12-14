using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CreateAccountPacketReq
{
    public string AccountName;
    public string Password;
}

public class CreateAccountPacketRes
{
    public bool CreateOk;
}

public class LoginAccountPacketReq
{
    public string AccountName;
    public string Password;
}

public class ServerInfo
{
    public string Name;
    public string IpAddress;
    public int Port;
    public int BusyScore;
}

public class LoginAccountPacketRes
{
    public bool LoginOk { get; set; }
    public int AccountId { get; set; }
    public int Token { get; set; }
    public List<ServerInfo> ServerList { get; set; } = new List<ServerInfo>();
}