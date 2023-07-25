using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Database database = new Database();
        bool isApplicationRunning = true;

        while (isApplicationRunning)
        {
            DisplayMenu();

            if (!int.TryParse(Console.ReadLine(), out int command))
            {
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                continue;
            }

            switch (command)
            {
                case Database.AddPlayerCommand:
                    database.HandleAddPlayer();
                    break;

                case Database.BanPlayerCommand:
                    database.HandleBanPlayer();
                    break;

                case Database.UnbanPlayerCommand:
                    database.HandleUnbanPlayer();
                    break;

                case Database.RemovePlayerCommand:
                    database.HandleRemovePlayer();
                    break;

                case Database.ExitCommand:
                    isApplicationRunning = false;
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine($"Меню управления игроками:\n" +
                          $"{Database.AddPlayerCommand}. Добавить игрока\n" +
                          $"{Database.BanPlayerCommand}. Забанить игрока\n" +
                          $"{Database.UnbanPlayerCommand}. Разбанить игрока\n" +
                          $"{Database.RemovePlayerCommand}. Удалить игрока\n" +
                          $"{Database.ExitCommand}. Выход");
    }
}

public class Player
{
    public Player(int uniqueIdentifier, string nickname, int level)
    {
        UniqueIdentifier = uniqueIdentifier;
        Nickname = nickname;
        Level = level;
        IsBanned = false;
    }

    public int UniqueIdentifier { get; }
    public string Nickname { get; }
    public int Level { get; }
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
    private List<Player> _players = new List<Player>();
    private int _lastIdentifier = 1;

    public const int AddPlayerCommand = 1;
    public const int BanPlayerCommand = 2;
    public const int UnbanPlayerCommand = 3;
    public const int RemovePlayerCommand = 4;
    public const int ExitCommand = 5;

    public void HandleAddPlayer()
    {
        if (AddPlayer())
        {
            Console.WriteLine("Игрок успешно добавлен.");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
    }

    public void HandleBanPlayer()
    {
        HandlePlayerBanStatus(true);
    }

    public void HandleUnbanPlayer()
    {
        HandlePlayerBanStatus(false);
    }

    public void HandleRemovePlayer()
    {
        if (RemovePlayer())
        {
            Console.WriteLine("Игрок удален.");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
    }

    private bool AddPlayer()
    {
        Console.WriteLine("Введите ник и уровень игрока:");
        string nickname = Console.ReadLine();
        bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int level);

        if (isParseSuccessful)
        {
            _players.Add(new Player(_lastIdentifier++, nickname, level));
            return true;
        }

        return false;
    }

    private bool RemovePlayer()
    {
        Player playerToRemove = FindPlayerById();

        if (playerToRemove != null)
        {
            _players.Remove(playerToRemove);
            return true;
        }

        return false;
    }

    private bool BanPlayer()
    {
        Player playerToBan = FindPlayerById();

        if (playerToBan != null)
        {
            playerToBan.Ban();
            return true;
        }

        return false;
    }

    private bool UnbanPlayer()
    {
        Player playerToUnban = FindPlayerById();

        if (playerToUnban != null)
        {
            playerToUnban.Unban();
            return true;
        }

        return false;
    }

    private Player FindPlayerById()
    {
        int uniqueIdentifier = GetPlayerIdFromUser();

        foreach (Player player in _players)
        {
            if (player.UniqueIdentifier == uniqueIdentifier)
            {
                return player;
            }
        }

        return null;
    }

    private void HandlePlayerBanStatus(bool isBanning)
    {
        Console.WriteLine("Список игроков:");
        DisplayPlayers();

        if (isBanning ? BanPlayer() : UnbanPlayer())
        {
            Console.WriteLine(isBanning ? "Игрок забанен." : "Игрок разбанен.");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
    }

    public void DisplayPlayers()
    {
        foreach (Player player in _players)
        {
            Console.WriteLine($"ID: {player.UniqueIdentifier}, Ник: {player.Nickname}, Уровень: {player.Level}, Забанен: {player.IsBanned}");
        }
    }

    private int GetPlayerIdFromUser()
    {
        Console.WriteLine("Введите уникальный ID игрока:");
        int.TryParse(Console.ReadLine(), out int uniqueIdentifier);
        return uniqueIdentifier;
    }
}
