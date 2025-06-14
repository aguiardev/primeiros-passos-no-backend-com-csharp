﻿// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using System.Media;
using WhoWantsToBeAMillionaire.Core;
using WhoWantsToBeAMillionaire.Core.Enums;
using WhoWantsToBeAMillionaire.Core.Events;
using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services;
using WhoWantsToBeAMillionaire.Data;
using WhoWantsToBeAMillionaire.Data.Repositories;

public class Program
{
    private static readonly BackgroundSongService _backgroundSongService = new BackgroundSongService(
        new SoundPlayerService(new SoundPlayer()));

    private static readonly GameService _gameService = ConfigGame();

    //TODO: configurar DpUp para rodar as migrations
    
    private static void Main(string[] args)
    {
        Console.Title = "Show do Milhão";

        PrintMainMenu();
    }

    #region Prints Methods

    private static void PrintMainMenu()
    {
        _backgroundSongService.PlayOpenning();

        while (true)
        {
            Console.WriteLine("Bem-vindo(a) ao Show do Milhão!");
            Console.WriteLine("");
            Console.WriteLine("Selecione a opção desejada:");
            Console.WriteLine("");
            Console.WriteLine("1 - Começar novo jogo");
            Console.WriteLine("2 - Visualizar ranking");
            Console.WriteLine("3 - Sair");
            Console.WriteLine("");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    _gameService.Start(GetPlayerName());
                    continue;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Clear();
                    PrintRanking();
                    continue;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Opção inválida!");
                    Console.WriteLine("");
                    continue;
            }
        }
    }

    private static void PrintQuestion(QuestionsModel question)
    {
        Console.WriteLine($"Pergunta {question.Number}: {question.Description}");
        Console.WriteLine();

        int number = 1;
        foreach (var option in question.Options.Where(w => !w.Hidden))
        {
            Console.WriteLine($"{number}) {option.Description}");

            option.Number = number; //TODO: ainda preciso desse atributo?
            number++;
        }

        Console.WriteLine();
        Console.Write("Digite a opção desejada e tecle [ENTER]: ");
    }

    private static void PrintHeader(NextQuestionArgs args)
    {

        Console.WriteLine($"Acertar: {args.Award.RightAnswer:C} - Parar: {args.Award.Stop:C} - Errar: {args.Award.WrongAnswer:C}");
        Console.WriteLine();
        Console.WriteLine($"Você pode pular {args.SkipCount} perguntas");
        Console.WriteLine();
        Console.WriteLine($">> Digite [{Constants.SKIP}] para pular.");
        Console.WriteLine($">> Digite [{Constants.STOP}] para parar.");
        Console.WriteLine();
        Console.WriteLine($"{args.PlayerName} até agora você ganhou {args.CurrentAward:C}.");
        Console.WriteLine();
    }

    //TODO: transformar numa classe estática
    private static void PrintRanking()
    {
        const int widthColumn1 = 20;
        const int widthColumn2 = 12;
        const int widthColumn3 = 11;
        const int widthColumn4 = 20;
        const string templateHeader = "|{0}|{1}|{2}|{3}|";
        const string templateDivider = "+{0}+{1}+{2}+{3}+";

        string Repeat(char c, int count) => new(c, count);

        void PrintHeader()
        {
            var header = string.Format(
                templateHeader,
                "Jogador".PadRight(widthColumn1),
                "Qtde. Ajuda".PadRight(widthColumn2),
                "Qtde. Pulo".PadRight(widthColumn3),
                "Prêmio".PadRight(widthColumn4));

            Console.WriteLine(header);
        }

        void PrintDivider()
        {
            var divider = string.Format(
                templateDivider,
                Repeat('-', widthColumn1),
                Repeat('-', widthColumn2),
                Repeat('-', widthColumn3),
                Repeat('-', widthColumn4));

            Console.WriteLine(divider);
        }

        PrintDivider();
        PrintHeader();
        PrintDivider();

        foreach (var ranking in _gameService.GetTopFiveRanking())
            Console.WriteLine(ranking.ToString(
                widthColumn1, widthColumn2, widthColumn3, widthColumn4));

        PrintDivider();

        Console.WriteLine("");
        Console.Write("Pressione qualquer tecla para voltar ao menu principal...");
        Console.ReadLine();
        Console.Clear();
    }

    #endregion

    private static GameService ConfigGame()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

        var settings = config.Get<AppSettings>();

        var connection = new Connection(settings.ConnectionString);

        var awardRepository = new AwardsRepository(connection);
        var questionRepository = new QuestionsRepository(connection);
        var optionsRepository = new OptionsRepository(connection);
        var rankingRepository = new RankingsRepository(connection);

        var questionService = new QuestionsService(optionsRepository, questionRepository);
        var awardService = new AwardService(awardRepository);
        var rankingService = new RankingService(rankingRepository);

        var game = new GameService(
            questionService,
            awardService,
            settings.SkipCount,
            rankingService);

        game.OnStarted += (sender, args) =>
        {
            Console.WriteLine($"Vamos começar o jogo, {args.PlayerName}!");

            //ShowProgressBar();

            Console.Clear();
        };

        game.OnNextQuestion += (sender, args) =>
        {
            Console.Clear();

            _backgroundSongService.PlayQuestionSelection();
            _backgroundSongService.PlayThriller();

            PrintHeader(args);
            PrintQuestion(args.Question);

            // TODO: verificar possível bug que retorna pergunta repetida quando pulamos

            while (true)
            {
                string optionSelected = Console.ReadLine() ?? string.Empty;

                if (!_gameService.IsValidOption(optionSelected, out var message))
                {
                    Console.Clear();
                    Console.WriteLine(message);
                    Console.WriteLine();

                    PrintHeader(args);
                    PrintQuestion(args.Question);
                    continue;
                }

                break;
            }
        };
        
        game.OnRightAnswer += (sender, args) =>
        {
            Console.Clear();
            Console.Write("Certa resposta! Aguarde...");
            Thread.Sleep(TimeSpan.FromSeconds(2));
        };
        
        game.OnGameOver += (sender, args) =>
        {
            Console.Clear();

            switch (args.GameOverReason)
            {
                case GameOverReason.Lost:
                    Console.WriteLine($"Resposta errada! Você ganhou {args.Award:C}!");
                    break;
                case GameOverReason.Stopped:
                    Console.WriteLine($"Você decidiu parar e ganhou {args.Award:C}!");
                    break;
                case GameOverReason.Won:
                    Console.WriteLine($"Parabéns! Você acertou todas as perguntas e ganhou {args.Award:C}!");
                    break;
            }

            _backgroundSongService.PlayGameOver();

            Console.Clear();

            _backgroundSongService.PlayOpenning();
        };

        return game;
    }

    private static void ShowProgressBar()
    {
        for (int progress = 0; progress <= 100; progress++)
        {
            Console.Clear();
            Console.WriteLine($"Carregando... {progress}%");
            Thread.Sleep(25);
        }
    }

    private static string GetPlayerName()
    {
        Console.Write("Digite o seu nome: ");

        string? name;

        while (true)
        {
            name = Console.ReadLine();

            Console.Clear();
            if (!string.IsNullOrEmpty(name))
                break;

            Console.Write("Nome inválido! Digite novamente: ");
        }

        return name;
    }
}