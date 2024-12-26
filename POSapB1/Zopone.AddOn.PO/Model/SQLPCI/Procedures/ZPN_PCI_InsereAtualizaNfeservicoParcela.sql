
CREATE PROCEDURE ZPN_PCI_InsereAtualizaNfeservicoParcela
    @nfeservicopardelaid varchar(250),
    @gestatus INT,
    @gedataacao DATETIME,
    @gecontaidacao varchar(250),
    @nfeservicoid varchar(250),
    @sequencia INT,
    @vencimento DATETIME,
    @valor DECIMAL(18,2),
    @fatura VARCHAR(200),
    @percentual FLOAT,
    @etapaid varchar(250)
AS
BEGIN

    INSERT INTO [nfeservicoparcela]
           ([nfeservicopardelaid]
           ,[gestatus]
           ,[gedataacao]
           ,[gecontaidacao]
           ,[nfeservicoid]
           ,[sequencia]
           ,[vencimento]
           ,[valor]
           ,[fatura]
           ,[percentual]
           ,[etapaid])
     VALUES
           (@nfeservicopardelaid
           ,@gestatus
           ,@gedataacao
           ,@gecontaidacao
           ,@nfeservicoid
           ,@sequencia
           ,@vencimento
           ,@valor
           ,@fatura
           ,@percentual
           ,@etapaid);
END
GO
