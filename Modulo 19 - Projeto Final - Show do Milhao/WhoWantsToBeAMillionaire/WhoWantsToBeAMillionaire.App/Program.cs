// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using System.Media;
using WhoWantsToBeAMillionaire.App;
using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Events;
using WhoWantsToBeAMillionaire.App.Enums;
using WhoWantsToBeAMillionaire.App.Services;

var backgroundSongService = new BackgroundSongService(new SoundPlayer());

backgroundSongService.PlayOpening();

PrintMenu();

GetConfigValues(out int helpCount, out int skipCount);

var game = new GameService(
    LoadQuestions(),
    LoadAwards(),
    GetPlayerName(),
    helpCount,
    skipCount);

game.OnStarted += OnStarted;
game.OnNextQuestion += OnNextQuestion;
game.OnRightAswer += OnRightAswer;
game.OnGameOver += OnGameOver;
game.Start();

// TODO: começar novo jogo

Console.ReadLine();

#region Events

void OnStarted(object sender, StartedArgs args)
{
    Console.WriteLine($"Vamos começar o jogo, {args.PlayerName}!");

    //ShowProgressBar();

    Console.Clear();
}

void OnNextQuestion(object sender, NextQuestionArgs args)
{
    Console.Clear();
    
    if (!args.CallHelp)
    {
        backgroundSongService.PlayQuestionSelection();
        backgroundSongService.PlayThriller();
    }

    PrintHeader(args);
    PrintQuestion(args.Question);

    var optionSelected = string.Empty;

    // TODO: verificar possível bug que retorna pergunta repetida quando pulamos

    while (true)
    {
        optionSelected = Console.ReadLine() ?? string.Empty;

        if (game.IsValidOption(
            optionSelected, args.CallHelp, out var message))
        {
            game.OptionNumberSelected = optionSelected;
            break;
        }

        Console.Clear();
        Console.WriteLine(message);
        Console.WriteLine();

        PrintHeader(args);
        PrintQuestion(args.Question);
    }
}

void OnRightAswer(object sender, RightAswerArgs args)
{
    Console.Clear();
    Console.Write("Certa resposta! Aguarde...");
    Thread.Sleep(TimeSpan.FromSeconds(2));
}

void OnGameOver(object sender, GameOverArgs args)
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

void PrintMenu()
{
    Console.WriteLine("Bem-vindo(a) ao Show do Milhão!");
    Console.WriteLine("");
}

void PrintQuestion(Question question)
{
    Console.WriteLine($"Pergunta {question.Number}: {question.Description}");
    Console.WriteLine();

    foreach (var option in question.Options)
    {
        Console.WriteLine($"{option.Number}) {option.Description}");
    }

    Console.WriteLine();
    Console.Write("Digite a opção desejada e tecle [ENTER]: ");
}

void PrintHeader(NextQuestionArgs args)
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

void GetConfigValues(out int helpCount, out int skipCount)
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();

    var settings = config.Get<AppSettings>();

    helpCount = settings.HelpCount;
    skipCount = settings.SkipCount;
}

void ShowProgressBar()
{
    for (int progress = 0; progress <= 100; progress++)
    {
        Console.Clear();
        Console.WriteLine($"Carregando... {progress}%");
        Thread.Sleep(25);
    }
}

string GetPlayerName()
{
    Console.Write("Digite o seu nome: ");

    var name = string.Empty;

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

List<Question> LoadQuestions()
{
    return new List<Question>()
    {
        new Question(
            1,
            "Qual a capital da China?",
            new List<Option>()
            {
                new Option(1, "Kyoto"),
                new Option(2, "Pequim", true),
                new Option(3, "Taiwan"),
                new Option(4, "Brasilia")
            }),
        new Question(
            2,
            "Qual o nome do terceiro planeta do sistema solar?",
            new List<Option>()
            {
                new Option(1, "Marte"),
                new Option(2, "Plutão"),
                new Option(3, "Terra", true),
                new Option(4, "Mercúrio")
            }),
        new Question(
            3,
            "Qual o ponto mais alto do Brasil?",
            new List<Option>()
            {
                new Option(1, "Pico da Bandeira"),
                new Option(2, "Pico do Calçado"),
                new Option(3, "Pico 31 de Março"),
                new Option(4, "Pico da Neblina", true)
            }),
        new Question(
            4,
            "Qual o dia da independência nos EUA?",
            new List<Option>()
            {
                new Option(1, "23 de Março"),
                new Option(2, "4 de Julho", true),
                new Option(3, "7 de Setembro"),
                new Option(4, "15 de Novembro")
            })
    };
}

List<Award> LoadAwards()
{
    return new List<Award>()
    {
        new Award(1, 1000m, 0m, 0m),
        new Award(2, 2000m, 1000m, 500m),
    };
}