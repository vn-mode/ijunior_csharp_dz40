using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private const string Menu = "1. Добавить игрока\n2. Забанить игрока\n3. Разбанить игрока\n4. Удалить игрока\n5. Выход";
    private const string AddPlayer = "Введите ник и уровень игрока:";
    private const string BanUnbanRemove = "Введите уникальный ID игрока:";
    private const string InvalidOption = "Неверный выбор. Попробуйте снова.";
    private const string InvalidInput = "Некорректный ввод. Попробуйте снова.";
    private const string PlayerAdded = "Игрок успешно добавлен.";
    private const string PlayerBanned = "Игрок забанен.";
    private const string PlayerUnbanned = "Игрок разбанен.";
    private const string PlayerRemoved = "Игрок удален.";

    static void Main(string[] args)
    {
        Database database = new Database();
        int uniqueIdCounter = 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(Menu);

            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine(InvalidOption);
                continue;
            }

            switch (option)
            {
                case 1:
                    uniqueIdCounter = AddPlayerMethod(database, uniqueIdCounter);
                    Console.WriteLine(PlayerAdded);
                    break;
                case 2:
                    BanUnbanRemoveMethod(database, database.BanPlayer);
                    Console.WriteLine(PlayerBanned);
                    break;
                case 3:
                    BanUnbanRemoveMethod(database, database.UnbanPlayer);
                    Console.WriteLine(PlayerUnbanned);
                    break;
                case 4:
                    BanUnbanRemoveMethod(database, database.RemovePlayer);
                    Console.WriteLine(PlayerRemoved);
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine(InvalidOption);
                    break;
            }
        }
    }

    static int AddPlayerMethod(Database database, int uniqueId)
    {
        Console.WriteLine(AddPlayer);
        string nickname = Console.ReadLine();
        if (!int.TryParse(Console.ReadLine(), out int level))
        {
            Console.WriteLine(InvalidInput);
            return uniqueId;
        }
        database.AddPlayer(new Player(uniqueId, nickname, level));
        return uniqueId + 1;
    }

    static void BanUnbanRemoveMethod(Database database, Action<int> action)
    {
        database.DisplayPlayers();
        Console.WriteLine(BanUnbanRemove);
        if (!int.TryParse(Console.ReadLine(), out int uniqueId))
        {
            Console.WriteLine(InvalidInput);
            return;
        }

        action(uniqueId);
    }
}

public class Player
{
    public int UniqueId { get; }
    public string Nickname { get; set; }
    public int Level { get; set; }
    private bool _isBanned;

    public Player(int uniqueId, string nickname, int level)
    {
        UniqueId = uniqueId;
        Nickname = nickname;
        Level = level;
        _isBanned = false;
    }

    public bool IsBanned => _isBanned;

    public void Ban()
    {
        _isBanned = true;
    }

    public void Unban()
    {
        _isBanned = false;
    }
}

public class Database
{
    private List<Player> _players;

    public Database()
    {
        _players = new List<Player>();
    }

    private bool TryGetPlayer(int uniqueId, out Player player)
    {
        player = _players.FirstOrDefault(playerInList => playerInList.UniqueId == uniqueId);
        return player != null;
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void RemovePlayer(int uniqueId)
    {
        if (TryGetPlayer(uniqueId, out Player player))
        {
            _players.Remove(player);
        }
    }

    public void BanPlayer(int uniqueId)
    {
        if (TryGetPlayer(uniqueId, out Player player))
        {
            player.Ban();
        }
    }

    public void UnbanPlayer(int uniqueId)
    {
        if (TryGetPlayer(uniqueId, out Player player))
        {
            player.Unban();
        }
    }

    public void DisplayPlayers()
    {
        Console.WriteLine("Список игроков:");
        foreach (var player in _players)
        {
            Console.WriteLine($"ID: {player.UniqueId}, Ник: {player.Nickname}, Уровень: {player.Level}");
        }
    }
}
