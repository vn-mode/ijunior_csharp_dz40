using System;
using System.Collections.Generic;
using System.Linq;

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
                case 1:
                    HandleAddPlayer(database);
                    break;

                case 2:
                    HandleBanPlayer(database);
                    break;

                case 3:
                    HandleUnbanPlayer(database);
                    break;

                case 4:
                    HandleRemovePlayer(database);
                    break;

                case 5:
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
        string menuTitle = "Меню управления игроками:";
        string addPlayerOption = "1. Добавить игрока";
        string banPlayerOption = "2. Забанить игрока";
        string unbanPlayerOption = "3. Разбанить игрока";
        string removePlayerOption = "4. Удалить игрока";
        string exitOption = "5. Выход";

        Console.Clear();
        Console.WriteLine($"{menuTitle}\n{addPlayerOption}\n{banPlayerOption}\n{unbanPlayerOption}\n{removePlayerOption}\n{exitOption}");
    }

    private static void HandleAddPlayer(Database database)
    {
        if (database.AddPlayer())
        {
            Console.WriteLine("Игрок успешно добавлен.");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
    }

    private static void HandleBanPlayer(Database database)
    {
        HandlePlayerBanStatus(database, true);
    }

    private static void HandleUnbanPlayer(Database database)
    {
        HandlePlayerBanStatus(database, false);
    }

    private static void HandleRemovePlayer(Database database)
    {
        if (database.RemovePlayer())
        {
            Console.WriteLine("Игрок удален.");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
    }

    private static void HandlePlayerBanStatus(Database database, bool isBanning)
    {
        Console.WriteLine("Список игроков:");
        database.DisplayPlayers();

        if (isBanning ? database.BanPlayer() : database.UnbanPlayer())
        {
            Console.WriteLine(isBanning ? "Игрок забанен." : "Игрок разбанен.");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
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
    private List<Player> players = new List<Player>();
    private int uniqueIdentifierCounter = 1;

    public bool AddPlayer()
    {
        Console.WriteLine("Введите ник и уровень игрока:");
        string nickname = Console.ReadLine();
        bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int level);

        if (isParseSuccessful)
        {
            players.Add(new Player(uniqueIdentifierCounter++, nickname, level));
            return true;
        }

        return false;
    }

    public bool RemovePlayer()
    {
        int uniqueIdentifier = GetPlayerIdFromUser();
        Player playerToRemove = players.FirstOrDefault(player => player.UniqueIdentifier == uniqueIdentifier);

        if (playerToRemove != null)
        {
            players.Remove(playerToRemove);
            return true;
        }

        return false;
    }

    public bool BanPlayer()
    {
        int uniqueIdentifier = GetPlayerIdFromUser();
        Player playerToBan = players.FirstOrDefault(player => player.UniqueIdentifier == uniqueIdentifier);

        if (playerToBan != null)
        {
            playerToBan.Ban();
            return true;
        }

        return false;
    }

    public bool UnbanPlayer()
    {
        int uniqueIdentifier = GetPlayerIdFromUser();
        Player playerToUnban = players.FirstOrDefault(player => player.UniqueIdentifier == uniqueIdentifier);

        if (playerToUnban != null)
        {
            playerToUnban.Unban();
            return true;
        }

        return false;
    }

    public void DisplayPlayers()
    {
        foreach (Player player in players)
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
