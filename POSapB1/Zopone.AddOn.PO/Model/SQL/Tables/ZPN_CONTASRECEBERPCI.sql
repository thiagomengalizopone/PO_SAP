create TABLE ZPN_CONTASRECEBERPCI
(
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Coluna de ID com autoincremento
    DocEntry INT,
    IntId INT,
    LineNum INT,
    Status INT,
    Total DECIMAL(18, 4),
    PercItem DECIMAL(18, 4),
	U_IdPCINotaServico varchar(150), 
	U_IdPCIContasReceber varchar(150)
);