CREATE TABLE Awards
(
    Id INTEGER PRIMARY KEY,
    Correct INTEGER,
    Stop INTEGER,
    Wrong INTEGER
);

INSERT INTO Awards (Id, Correct, Stop, Wrong) VALUES
(1, 1000, 0, 0),
(2, 2000, 1000, 500),
(3, 3000, 2000, 1000),
(4, 4000, 3000, 1500),
(5, 5000, 4000, 2000),
(6, 10000, 5000, 2500),
(7, 20000, 10000, 5000),
(8, 30000, 20000, 10000),
(9, 40000, 30000, 15000),
(10, 50000, 40000, 20000),
(11, 100000, 50000, 25000),
(12, 200000, 100000, 50000),
(13, 300000, 200000, 100000),
(14, 400000, 300000, 150000),
(15, 500000, 400000, 200000),
(16, 1000000, 500000, 0);