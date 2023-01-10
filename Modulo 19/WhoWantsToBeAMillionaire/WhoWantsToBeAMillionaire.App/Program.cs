// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using System.Media;
using WhoWantsToBeAMillionaire.Core;
using WhoWantsToBeAMillionaire.Core.Enums;
using WhoWantsToBeAMillionaire.Core.Events;
using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services;
using WhoWantsToBeAMillionaire.Data;
using WhoWantsToBeAMillionaire.Data.Repositories;

internal class Program
{
    private static readonly BackgroundSongService backgroundSongService = new(new SoundPlayer());
    private static readonly GameService? gameService = ConfigGame();

    private static void Main(string[] args)
    {
        while (true)
        {
            backgroundSongService.PlayOpening();

            PrintMenu();

            gameService.Start(GetPlayerName());

            Console.WriteLine("Deseja jogar novamente? Digite S/N.");

            string optionSelected = Console.ReadLine() ?? string.Empty;

            if (string.Equals(optionSelected.Trim(), Constants.NO, StringComparison.OrdinalIgnoreCase))
                break;

            Console.Clear();
        }
    }

    #region Events

    private static void OnStarted(object sender, StartedArgs args)
    {
        Console.WriteLine($"Vamos começar o jogo, {args.PlayerName}!");

        //ShowProgressBar();

        Console.Clear();
    }

    private static void OnNextQuestion(object sender, NextQuestionArgs args)
    {
        Console.Clear();

        if (!args.CallHelp)
        {
            backgroundSongService.PlayQuestionSelection();
            backgroundSongService.PlayThriller();
        }

        PrintHeader(args);
        PrintQuestion(args.Question);

        // TODO: verificar possível bug que retorna pergunta repetida quando pulamos

        while (true)
        {
            string optionSelected = Console.ReadLine() ?? string.Empty;

            if (!gameService.IsValidOption(
                optionSelected, args.CallHelp, out var message))
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
    }

    private static void OnRightAnswer(object sender, RightAswerArgs args)
    {
        Console.Clear();
        Console.Write("Certa resposta! Aguarde...");
        Thread.Sleep(TimeSpan.FromSeconds(2));
    }

    private static void OnGameOver(object sender, GameOverArgs args)
    {
        Console.Clear();

        //TODO: tocar uma música diferente ao terminar o jogo

        switch (args.GameOverReason)
        {
            case GameOverReason.Lost:
                Console.WriteLine($"Resposta errada! Você ganhou {args.Award:C}!");
                break;
            case GameOverReason.Stopped:
                Console.Clear();
                Console.WriteLine($"Você decidiu parar e ganhou {args.Award:C}!");
                break;
            case GameOverReason.Won:
                Console.WriteLine($"Parabéns! Você acertou todas as perguntas e ganhou {args.Award:C}!");
                break;
        }
    }

    #endregion

    #region Prints Methods

    private static void PrintMenu()
    {
        Console.WriteLine("Bem-vindo(a) ao Show do Milhão!");
        Console.WriteLine("");
    }

    private static void PrintQuestion(QuestionModel question)
    {
        Console.WriteLine($"Pergunta {question.Number}: {question.Description}");
        Console.WriteLine();

        int number = 1;
        foreach (var option in question.Options.Where(w => !w.Hidden))
        {
            Console.WriteLine($"{number}) {option.Description}");

            option.Number = number;
            number++;
        }

        Console.WriteLine();
        Console.Write("Digite a opção desejada e tecle [ENTER]: ");
    }

    private static void PrintHeader(NextQuestionArgs args)
    {

        Console.WriteLine($"Acertar: {args.Award.Correct:C} - Parar: {args.Award.Stop:C} - Errar: {args.Award.Wrong:C}");
        Console.WriteLine();
        Console.WriteLine($"Você tem {args.SkipCount} pulos e {args.HelpCount} pedidos de ajuda.");
        Console.WriteLine();
        Console.WriteLine($">> Digite [{Constants.HELP}] para ajuda.");
        Console.WriteLine($">> Digite [{Constants.SKIP}] para pular.");
        Console.WriteLine($">> Digite [{Constants.STOP}] para parar.");
        Console.WriteLine();
        Console.WriteLine($"{args.PlayerName} até agora você ganhou {args.CurrentAward:C}.");
        Console.WriteLine();
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

        var awardRepository = new AwardRepository(connection);
        var questionRepository = new QuestionRepository(connection);
        var optionsRepository = new OptionsRepository(connection);

        var questionService = new QuestionService(optionsRepository, questionRepository);
        var awardService = new AwardService(awardRepository);

        var game = new GameService(
            questionService,
            awardService,
            settings.HelpCount,
            settings.SkipCount);

        game.OnStarted += OnStarted;
        game.OnNextQuestion += OnNextQuestion;
        game.OnRightAnswer += OnRightAnswer;
        game.OnGameOver += OnGameOver;

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

        string name;

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