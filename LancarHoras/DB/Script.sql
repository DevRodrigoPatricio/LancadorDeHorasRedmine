CREATE TABLE HorasTrabalhadas (
    Id INT PRIMARY KEY,          
    Data DATE NOT NULL,
    Tarefa NVARCHAR(100) NOT NULL,
    HorarioInicial TIME NOT NULL,
    HorarioFinal TIME NOT NULL,
    Duracao TIME NOT NULL,
    Comentario NVARCHAR(500),
    Atividade NVARCHAR(50),
    Situacao NVARCHAR(50)
);

CREATE TABLE Config
(
    id INT NOT NULL PRIMARY KEY, 
    url NVARCHAR(MAX) NULL, 
    chaveKey NVARCHAR(MAX) NULL
);