CREATE TABLE Question
(
    Id INTEGER PRIMARY KEY,
    Description VARCHAR(100)
);

CREATE TABLE Options
(
    Id INTEGER PRIMARY KEY,
    Description VARCHAR(100),
    Correct BOOLEAN,
    Question_Id INTEGER,
    FOREIGN KEY(Question_Id) REFERENCES QUESTION(ID)
);

INSERT INTO Question (Id, Description) values (1, 'Qual a capital da China?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(1, 'Kyoto', 0, 1),
(2, 'Pequim', 1, 1),
(3, 'Taiwan', 0, 1),
(4, 'Brasilia', 0, 1);

INSERT INTO Question (Id, Description) values (2, 'Qual o nome do terceiro planeta do sistema solar?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(5, 'Marte', 0, 2),
(6, 'Plutão', 0, 2),
(7, 'Terra', 1, 2),
(8, 'Mercúrio', 0, 2);

INSERT INTO Question (Id, Description) values (3, 'Qual o ponto mais alto do Brasil?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(9, 'Pico da Bandeira', 0, 3),
(10, 'Pico do Calçado', 0, 3),
(11, 'Pico 31 de Março', 0, 3),
(12, 'Pico da Neblina', 1, 3);

INSERT INTO Question (Id, Description) values (4, 'Qual o dia da independência nos EUA?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(13, '23 de Março', 0, 4),
(14, '4 de Julho', 1, 4),
(15, '7 de Setembro', 0, 4),
(16, '15 de Novembro', 0, 4);

INSERT INTO Question (Id, Description) values (5, 'Quais são as cores presentes na bandeira da China?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(17, 'Vermelho e branco', 0, 5),
(18, 'Laranja e amarelo', 0, 5),
(19, 'Vermelho e amarelo', 1, 5),
(20, 'Laranja e vermelho', 0, 5);

INSERT INTO Question (Id, Description) values (6, 'Qual desses animais não é classificado como aracnídeo?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(21, 'Aranha', 0, 6),
(22, 'Escorpião', 0, 6),
(23, 'Carrapato', 0, 6),
(24, 'Centopeia', 1, 6);

INSERT INTO Question (Id, Description) values (7, 'Qual é o maior continente em extensão?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(25, 'Europa', 0, 7),
(26, 'América', 0, 7),
(27, 'África', 0, 7),
(28, 'Ásia', 1, 7);

INSERT INTO Question (Id, Description) values (8, 'Qual é o idioma oficial do Egito?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(29, 'árabe', 1, 8),
(30, 'francês', 0, 8),
(31, 'egípcio', 0, 8),
(32, 'inglês', 0, 8);

INSERT INTO Question (Id, Description) values (9, 'Quantas patas uma formiga possui?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(33, '8', 0, 9),
(34, '6', 1, 9),
(35, '4', 0, 9),
(36, '10', 0, 9);

INSERT INTO Question (Id, Description) values (10, 'Quantos segundos há em duas horas?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(37, '2800', 0, 10),
(38, '3600', 0, 10),
(39, '6900', 0, 10),
(40, '7200', 1, 10);

INSERT INTO Question (Id, Description) values (11, 'Na mitologia grega, Zeus era filho de quem?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(41, 'Apolo', 0, 11),
(42, 'Urano', 0, 11),
(43, 'Cronos', 1, 11),
(44, 'Poseidon', 0, 11);

INSERT INTO Question (Id, Description) values (12, 'Como é o nome da energia gerada pelo vento?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(45, 'hídrica', 0, 12),
(46, 'térmica', 0, 12),
(47, 'eólica', 1, 12),
(48, 'química', 0, 12);

INSERT INTO Question (Id, Description) values (13, 'Você tem 36 laranjas e joga um terço fora, com quantas você fica?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(49, '23', 0, 13),
(50, '24', 1, 13),
(51, '12', 0, 13),
(52, '18', 0, 13);

INSERT INTO Question (Id, Description) values (14, 'Qual é o substantivo usado para se referir a um grupo de lobos?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(53, 'matilha', 0, 14),
(54, 'bando', 0, 14),
(55, 'alcateia', 1, 14),
(56, 'lobos', 0, 14);

INSERT INTO Question (Id, Description) values (15, 'Em qual país fica Machu Picchu?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(57, 'Chile', 0, 15),
(58, 'Venezuela', 0, 15),
(59, 'Peru', 1, 15),
(60, 'Argentina', 0, 15);

INSERT INTO Question (Id, Description) values (16, 'O azeite de oliva é feito de quê?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(61, 'azeitona', 1, 16),
(62, 'milho', 0, 16),
(63, 'canola', 0, 16),
(64, 'oliveira', 0, 16);

INSERT INTO Question (Id, Description) values (17, 'O tomate é...');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(65, 'Uma gruta', 0, 17),
(66, 'Uma fruta', 1, 17),
(67, 'Uma verdura', 0, 17),
(68, 'Um legume', 0, 17);

INSERT INTO Question (Id, Description) values (18, 'Na animação de Toy Story, qual o nome do personagem que é dono dos brinquedos?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(69, 'Sid', 0, 18),
(70, 'Tandy', 0, 18),
(71, 'Andy', 1, 18),
(72, 'Anderson', 0, 18);

INSERT INTO Question (Id, Description) values (19, 'Qual é o nome do criador do Facebook?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(73, 'Bill Gates', 0, 19),
(74, 'Mark Zuckerberg', 1, 19),
(75, 'Jeff Bezos', 0, 19),
(76, 'Steve Jobs', 0, 19);

INSERT INTO Question (Id, Description) values (20, 'Qual animal nasce do cruzamento do burro com a égua?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(77, 'mula', 1, 20),
(78, 'bezerro', 0, 20),
(79, 'cabrito', 0, 20),
(80, 'cabra', 0, 20);

INSERT INTO Question (Id, Description) values (21, ' Qual é o nome do metal que prejudica o poder do superman?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(81, 'esmeralda', 0, 21),
(82, 'kryptonita', 1, 21),
(83, 'diamante', 0, 21),
(84, 'ouro', 0, 21);

INSERT INTO Question (Id, Description) values (22, 'De quem é a famosa frase “Penso, logo existo”?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(85, 'Platão', 0, 22),
(86, 'Galileu Galilei', 0, 22),
(87, 'Descartes', 1, 22),
(88, 'Sócrates', 0, 22);

INSERT INTO Question (Id, Description) values (23, 'Qual o menor país do mundo?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(89, 'Vaticano', 1, 23),
(90, 'Naura', 0, 23),
(91, 'Mônaco', 0, 23),
(92, 'Malta', 0, 23);

INSERT INTO Question (Id, Description) values (24, 'Qual o maior país do mundo?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(93, 'China', 0, 24),
(94, 'Estados Unidos', 0, 24),
(95, 'Índia', 0, 24),
(96, 'Rússia', 1, 24);

INSERT INTO Question (Id, Description) values (25, 'Qual o número mínimo de jogadores em cada time numa partida de futebol?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(97, '8', 0, 25),
(98, '10', 0, 25),
(99, '7', 1, 25),
(100, '5', 0, 25);

INSERT INTO Question (Id, Description) values (26, 'Quanto tempo a luz do Sol demora para chegar à Terra?');
INSERT INTO Options (Id, Description, Correct, Question_Id) values
(101, '12 minutos', 0, 26),
(102, '1 dia', 0, 26),
(103, '12 horas', 0, 26),
(104, '8 minutos', 1, 26);