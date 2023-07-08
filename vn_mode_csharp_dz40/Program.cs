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

    private const string MenuTitle = "Меню управления игроками:";
    private const string AddPlayerOption = "1. Добавить игрока";
    private const string BanPlayerOption = "2. Забанить игрока";
    private const string UnbanPlayerOption = "3. Разбанить игрока";
    private const string RemovePlayerOption = "4. Удалить игрока";
    private const string ExitOption = "5. Выход";
    private const string PlayerListTitle = "Список игроков:";
    private const string AddNewPlayerPrompt = "Введите ник и уровень игрока:";
    private const string PlayerIdPrompt = "Введите уникальный ID игрока:";
    private const string InvalidOptionMessage = "Неверный выбор. Попробуйте снова.";
    private const string InvalidInputMessage = "Некорректный ввод. Попробуйте снова.";
    private const string PlayerAddedMessage = "Игрок успешно добавлен.";
    private const string PlayerBannedMessage = "Игрок забанен.";
    private const string PlayerUnbannedMessage = "Игрок разбанен.";
    private const string PlayerRemovedMessage = "Игрок удален.";

    static void Main(string[] args)
    {
        Database database = new Database();
        int uniqueIdentifierCounter = 1;
        bool isApplicationRunning = true;

        while (isApplicationRunning)
        {
            Console.Clear();
            Console.WriteLine(MenuTitle);
            Console.WriteLine($"{AddPlayerOption}\n{BanPlayerOption}\n{UnbanPlayerOption}\n{RemovePlayerOption}\n{ExitOption}");

            bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int command);

            if (isParseSuccessful == false)
            {
                Console.WriteLine(InvalidOptionMessage);
                continue;
            }

            switch (command)
            {
                case AddPlayerCommand:
                    Player newPlayer = CreateNewPlayer(uniqueIdentifierCounter);

                    if (newPlayer != null && database.AddPlayer(newPlayer))
                    {
                        Console.WriteLine(PlayerAddedMessage);
                        uniqueIdentifierCounter++;
                    }
                    else
                    {
                        Console.WriteLine(InvalidInputMessage);
                    }
                    break;

                case BanPlayerCommand:
                    Console.WriteLine(PlayerListTitle);
                    database.DisplayPlayers();

                    if (database.BanPlayer(GetPlayerIdFromUser()))
                    {
                        Console.WriteLine(PlayerBannedMessage);
                    }
                    else
                    {
                        Console.WriteLine(InvalidInputMessage);
                    }
                    break;

                case UnbanPlayerCommand:
                    Console.WriteLine(PlayerListTitle);
                    database.DisplayPlayers();

                    if (database.UnbanPlayer(GetPlayerIdFromUser()))
                    {
                        Console.WriteLine(PlayerUnbannedMessage);
                    }
                    else
                    {
                        Console.WriteLine(InvalidInputMessage);
                    }
                    break;

                case RemovePlayerCommand:
                    Console.WriteLine(PlayerListTitle);
                    database.DisplayPlayers();

                    if (database.RemovePlayer(GetPlayerIdFromUser()))
                    {
                        Console.WriteLine(PlayerRemovedMessage);
                    }
                    else
                    {
                        Console.WriteLine(InvalidInputMessage);
                    }
                    break;

                case ExitCommand:
                    isApplicationRunning = false;
                    break;

                default:
                    Console.WriteLine(InvalidOptionMessage);
                    break;
            }
        }
    }

    private static Player CreateNewPlayer(int uniqueIdentifier)
    {
        Console.WriteLine(AddNewPlayerPrompt);
        string nickname = Console.ReadLine();

        bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int level);

        if (isParseSuccessful)
        {
            return new Player(uniqueIdentifier, nickname, level);
        }

        return null;
    }

    private static int GetPlayerIdFromUser()
    {
        Console.WriteLine(PlayerIdPrompt);
        bool isParseSuccessful = int.TryParse(Console.ReadLine(), out int uniqueIdentifier);

        return isParseSuccessful ? uniqueIdentifier : -1;
    }
}

public class Player
{
    private int uniqueIdentifier;
    private bool isBanned;
    private string nickname;
    private int level;

    public Player(int uniqueIdentifier, string nickname, int level)
    {
        this.uniqueIdentifier = uniqueIdentifier;
        this.nickname = nickname;
        this.level = level;
        this.isBanned = false;
    }

    public int UniqueIdentifier => uniqueIdentifier;
    public string Nickname => nickname;
    public int Level => level;
    public bool IsBanned => isBanned;

    public void Ban()
    {
        isBanned = true;
    }

    public void Unban()
    {
        isBanned = false;
    }
}

public class Database
{
    private List<Player> players;

    public Database()
    {
        players = new List<Player>();
    }

    public bool AddPlayer(Player player)
    {
        if (player != null)
        {
            players.Add(player);
            return true;
        }

        return false;
    }

    public bool RemovePlayer(int uniqueIdentifier)
    {
        Player player = players.FirstOrDefault(p => p.UniqueIdentifier == uniqueIdentifier);

        if (player != null)
        {
            players.Remove(player);
            return true;
        }

        return false;
    }

    public bool BanPlayer(int uniqueIdentifier)
    {
        Player player = players.FirstOrDefault(p => p.UniqueIdentifier == uniqueIdentifier);
        if (player != null)
        {
            player.Ban();
            return true;
        }

        return false;
    }

    public bool UnbanPlayer(int uniqueIdentifier)
    {
        Player player = players.FirstOrDefault(p => p.UniqueIdentifier == uniqueIdentifier);
        if (player != null)
        {
            player.Unban();
            return true;
        }

        return false;
    }

    public void DisplayPlayers()
    {
        foreach (var player in players)
        {
            Console.WriteLine($"ID: {player.UniqueIdentifier}, Ник: {player.Nickname}, Уровень: {player.Level}, Забанен: {player.IsBanned}");
        }
    }
}
