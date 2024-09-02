CREATE TABLE ZPN_LogImportacaoPCI (
    LogID INT IDENTITY(1,1) PRIMARY KEY,  -- Identificador único do log
    ErrorNumber INT,                      -- Número do erro (código de erro SQL)
    ErrorSeverity INT,                    -- Severidade do erro
    ErrorState INT,                       -- Estado do erro
    ErrorProcedure NVARCHAR(128),         -- Nome da procedure onde o erro ocorreu
    ErrorLine INT,                        -- Linha de código onde o erro ocorreu
    ErrorMessage NVARCHAR(4000),          -- Mensagem de erro detalhada
    ErrorTime DATETIME DEFAULT GETDATE(), -- Data e hora do erro
    HostName NVARCHAR(128),               -- Nome do servidor ou máquina onde ocorreu o erro
    ApplicationName NVARCHAR(128),        -- Nome da aplicação ou módulo chamador
    UserName NVARCHAR(128),               -- Nome do usuário que executou a operação
    AdditionalInfo NVARCHAR(MAX) NULL     -- Campo para informações adicionais, como parâmetros de entrada
);