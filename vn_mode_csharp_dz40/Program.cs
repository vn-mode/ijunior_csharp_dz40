using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private const int AddPlayerCommand = 1;
    private const int BanPlayerCommand = 2;
    private const int UnbanPlayerCommand = 3;
    private const int RemovePlayerCommand = 4;
    private const int ExitCommand = 5;
    private const string Menu = "1. Добавить игрока\n2. Забанить игрока\n3. Разбанить игрока\n4. Удалить игрока\n5. Выход";
    private const string AddNewPlayer = "Введите ник и уровень игрока:";
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
        bool isWorking = true;

        while (isWorking)
        {
            Console.Clear();
            Console.WriteLine(Menu);

            bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int option);

            if (isParseSuccessful == false)
            {
                Console.WriteLine(InvalidOption);
                continue;
            }

            switch (option)
            {
                case AddPlayerCommand:
                    uniqueIdCounter = AddPlayer(database, uniqueIdCounter);
                    Console.WriteLine(PlayerAdded);
                    break;
                case BanPlayerCommand:
                    ExecuteAction(database, database.BanPlayer);
                    Console.WriteLine(PlayerBanned);
                    break;
                case UnbanPlayerCommand:
                    ExecuteAction(database, database.UnbanPlayer);
                    Console.WriteLine(PlayerUnbanned);
                    break;
                case RemovePlayerCommand:
                    ExecuteAction(database, database.RemovePlayer);
                    Console.WriteLine(PlayerRemoved);
                    break;
                case ExitCommand:
                    isWorking = false;
                    break;
                default:
                    Console.WriteLine(InvalidOption);
                    break;
            }
        }
    }

    static int AddPlayer(Database database, int uniqueId)
    {
        Console.WriteLine(AddNewPlayer);
        string nickname = Console.ReadLine();

        bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int level);

        if (isParseSuccessful == false)
        {
            Console.WriteLine(InvalidInput);
            return uniqueId;
        }

        database.AddPlayer(new Player(uniqueId, nickname, level));
        return uniqueId + 1;
    }

    static void ExecuteAction(Database database, Action<int> action)
    {
        database.DisplayPlayers();

        Console.WriteLine(BanUnbanRemove);

        bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int uniqueId);

        if (isParseSuccessful == false)
        {
            Console.WriteLine(InvalidInput);
            return;
        }

        action(uniqueId);
    }
}

public class Player
{
    private int _uniqueId;
    private bool _isBanned;

    public Player(int uniqueId, string nickname, int level)
    {
        _uniqueId = uniqueId;
        Nickname = nickname;
        Level = level;
        _isBanned = false;
    }

    public int UniqueId => _uniqueId;
    public string Nickname { get; set; }
    public int Level { get; set; }
    public bool IsBanned { get; private set; }

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
    private List<Player> _players;

    public Database()
    {
        _players = new List<Player>();
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

    private bool TryGetPlayer(int uniqueId, out Player player)
    {
        player = _players.FirstOrDefault(playerInList => playerInList.UniqueId == uniqueId);
        return player != null;
    }
}
