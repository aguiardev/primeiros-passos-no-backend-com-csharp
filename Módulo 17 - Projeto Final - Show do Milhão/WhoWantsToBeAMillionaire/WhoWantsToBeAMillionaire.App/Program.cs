// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using System.Media;
using WhoWantsToBeAMillionaire.App;
using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Events;
using WhoWantsToBeAMillionaire.App.Events.Enums;
using WhoWantsToBeAMillionaire.App.Services;

var backgroundSongService = new BackgroundSongService(new SoundPlayer());

backgroundSongService.PlayOpening();

PrintMenu();

GetConfigValues(out int helpCount, out int skipCount);

var awards = LoadAwards();

var game = new GameService(
    LoadQuestions(awards.Count),
    awards,
    GetPlayerName(),
    helpCount,
    skipCount);

game.OnStarted += OnStarted;
game.OnNextQuestion += OnNextQuestion;
game.OnQuestionAswered += OnQuestionAswered;
game.Start();

Console.ReadLine();

#region Events

void OnStarted(object sender, StartedArgs args)
{
    Console.WriteLine($"Vamos começar o jogo, {args.PlayerName}!");

    ShowProgressBar();

    Console.Clear();
}

void OnNextQuestion(object sender, NextQuestionArgs args)
{
    Console.Clear();

    backgroundSongService.PlayQuestionSelection();
    backgroundSongService.PlayThriller();

    PrintHeader(args);
    PrintQuestion(args.Question);

    var optionSelected = string.Empty;

    while (true)
    {
        optionSelected = Console.ReadLine() ?? string.Empty;
        if (GameService.IsValidOption(optionSelected))
        {
            game.OptionSelected = optionSelected;
            break;
        }

        Console.Clear();
        Console.WriteLine("Opção inválida! Tente novamente.");
        Console.WriteLine();

        PrintHeader(args);
        PrintQuestion(args.Question);
    }
}

void OnQuestionAswered(object sender, QuestionAsweredArgs args)
{
    switch (args.QuestionAswered)
    {
        case QuestionAswered.Right:
            Console.Clear();
            Console.Write("Certa resposta! Aguarde, carregando próxima pergunta...");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            break;
        case QuestionAswered.Wrong:
            Console.Clear();
            Console.WriteLine($"Resposta errada! Você ganhou {args.Award:C}!");
            break;
        default:
            if (args.Stopped)
            {
                Console.Clear();
                Console.WriteLine($"Você decidiu parar e recebeu {args.Award:C}!");
            }
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
    Console.WriteLine($"Pergunta: {question.Description}");
    Console.WriteLine();

    foreach (var option in question.Options)
    {
        Console.WriteLine($"{option.Alias}) {option.Description}");
    }

    Console.WriteLine();
    Console.Write("Digite a opção desejada e tecle [ENTER]: ");
}

void PrintHeader(NextQuestionArgs args)
{
    Console.WriteLine($"{args.PlayerName} até agora você ganhou {args.CurrentAward:C}.");
    Console.WriteLine();
    Console.WriteLine($">> Digite [{Constants.HELP}] para receber ajuda.");
    Console.WriteLine($">> Digite [{Constants.SKIP}] para pular para outra pergunta.");
    Console.WriteLine($">> Digite [{Constants.STOP}] para parar.");
    Console.WriteLine();
    Console.WriteLine($"Você tem {args.SkipCount} pulos e {args.HelpCount} pedidos de ajuda.");
    Console.WriteLine($"Acertar: {args.Award.Correct:C} - Parar: {args.Award.Stop:C} - Errar: {args.Award.Wrong:C}");
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

        if (string.IsNullOrEmpty(name))
        {
            Console.Clear();
            Console.Write("Nome inválido! Digite novamente: ");
        }
        else
        {
            Console.Clear();
            break;
        }
    }

    return name;
}

List<Question> LoadQuestions(int questionCount)
{
    var question = new List<Question>()
    {
        new Question()
        {
            Description = "Qual a capital da China?",
            Options = new List<Option>()
            {
                new Option("1", "Kyoto"),
                new Option("2", "Pequim", true),
                new Option("3", "Taiwan"),
                new Option("4", "Brasilia")
            }
        },
        new Question()
        {
            Description = "Qual o nome do terceiro planeta do sistema solar?",
            Options = new List<Option>()
            {
                new Option("1", "Marte"),
                new Option("2", "Plutão"),
                new Option("3", "Terra", true),
                new Option("4", "Mercúrio")
            }
        },
        new Question()
        {
            Description = "Qual o ponto mais alto do Brasil?",
            Options = new List<Option>()
            {
                new Option("1", "Pico da Bandeira"),
                new Option("2", "Pico do Calçado"),
                new Option("3", "Pico 31 de Março"),
                new Option("4", "Pico da Neblina", true)
            }
        }
    };

    return question.Take(questionCount).ToList();
}

List<Award> LoadAwards()
{
    return new List<Award>()
    {
        new Award(1, 1000m, 0m, 0m),
        new Award(2, 2000m, 1000m, 500m),
    };
}