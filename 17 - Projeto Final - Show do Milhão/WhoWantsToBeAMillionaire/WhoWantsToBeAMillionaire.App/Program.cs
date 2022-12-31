// See https://aka.ms/new-console-template for more information

using System.Media;
using WhoWantsToBeAMillionaire.App;

var backgroundSongService = new BackgroundSongService(new SoundPlayer());

backgroundSongService.PlayOpening();

PrintMenu();
StartGame();

Console.ReadLine();

void PrintMenu()
{
    Console.WriteLine("Bem-vindo(a) ao Show do Milhão!");
    Console.WriteLine("");
}

string GetPlayerName()
{
    Console.Write("Digite o seu nome: ");

    var name = string.Empty;

    while(true)
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

void ShowProgressBar()
{
    for (int progress = 0; progress <= 100; progress++)
    {
        Console.Clear();
        Console.WriteLine($"Carregando... {progress}%");
        Thread.Sleep(25);
    }
}

void StartGame()
{
    var currentAward = 0m;
    var playerName = GetPlayerName();

    Console.WriteLine($"Vamos começar o jogo, {playerName}!");
    
    var awards = LoadAwards();

    ShowProgressBar();

    Console.Clear();

    var questions = LoadQuestions();
    var questionNumber = 0;

    foreach (var question in questions)
    {
        questionNumber++;

        backgroundSongService.PlayQuestionSelection();
        backgroundSongService.PlayThriller();

        var award = GetAward(questionNumber, awards);

        PrintHeader(playerName, currentAward, award.Correct, award.Wrong);
        PrintQuestion(question);

        var optionSelected = string.Empty;

        while (true)
        {
            optionSelected = Console.ReadLine();
            if (IsValidOption(optionSelected))
                break;
                
            Console.Clear();
            Console.WriteLine("Opção inválida! Tente novamente.");
            Console.WriteLine();

            PrintQuestion(question);
        }

        Console.Clear();

        if (!IsCorrect(question, optionSelected!))
        {
            Console.WriteLine($"Resposta errada! Você ganhou {award.Wrong:C}");
            return;
        }

        Console.Write("Certa resposta! Aguarde, carregando próxima pergunta...");
        Thread.Sleep(TimeSpan.FromSeconds(2));
        Console.Clear();
        
        currentAward = award.Correct;
    }
}

void PrintQuestion(Question question)
{
    Console.WriteLine(question.Description);
    Console.WriteLine();

    foreach (var option in question.Options)
    {
        Console.WriteLine($"{option.Alias}) {option.Description}");
    }

    Console.WriteLine();
    Console.Write("Digite a letra da opção desejada e tecle [ENTER]: ");
}

bool IsValidOption(string? optionAlias)
    => !string.IsNullOrEmpty(optionAlias)
    && optionAlias.Trim().ToUpper() is "1" or "2" or "3" or "4";

bool IsCorrect(Question question, string optionAlias)
    => question.Options.Any(w => w.IsCorrect && w.Alias == optionAlias);

List<Question> LoadQuestions()
{
    return new List<Question>()
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
}

List<Award> LoadAwards()
{
    return new List<Award>()
    {
        new Award(1, 1000m, 0m, 0m),
        new Award(2, 2000m, 1000m, 500m),
    };
}

Award GetAward(int awardNumber, List<Award> awards)
    => awards.First(f => f.Number == awardNumber);

void PrintHeader(string playerName, decimal currentAward, decimal correctAward, decimal wrongAward)
{
    Console.WriteLine($"Jogador: {playerName} - Prêmio atual: {currentAward:C}");
    Console.WriteLine($"Acertar: {correctAward:C} - Errar: {wrongAward:C}");
    Console.WriteLine();
}
    