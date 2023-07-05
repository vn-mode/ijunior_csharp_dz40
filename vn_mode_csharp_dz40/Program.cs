using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public int UniqueId { get; private set; }
    public string Nickname { get; set; }
    public int Level { get; set; }
    public bool IsBanned { get; private set; }

    public Player(int uniqueId, string nickname, int level)
    {
        UniqueId = uniqueId;
        Nickname = nickname;
        Level = level;
        IsBanned = false;
    }

    public void Ban()
    {
        IsBanned = true;
    }

    public void Unban()
    {
        IsBanned = false;
    }
}

public class Database
{
    private List<Player> players;

    public Database()
    {
        players = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void RemovePlayer(int uniqueId)
    {
        var player = players.FirstOrDefault(p => p.UniqueId == uniqueId);
        if (player != null)
        {
            players.Remove(player);
        }
    }

    public void BanPlayer(int uniqueId)
    {
        var player = players.FirstOrDefault(p => p.UniqueId == uniqueId);
        if (player != null)
        {
            player.Ban();
        }
    }

    public void UnbanPlayer(int uniqueId)
    {
        var player = players.FirstOrDefault(p => p.UniqueId == uniqueId);
        if (player != null)
        {
            player.Unban();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Database database = new Database();
        Player player1 = new Player(1, "Player1", 10);
        Player player2 = new Player(2, "Player2", 20);

        database.AddPlayer(player1);
        database.AddPlayer(player2);
        database.BanPlayer(1);
        database.UnbanPlayer(1);
        database.RemovePlayer(2);
    }
}
