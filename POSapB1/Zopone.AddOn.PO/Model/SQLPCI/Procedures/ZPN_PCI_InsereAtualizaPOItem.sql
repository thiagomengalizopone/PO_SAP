CREATE PROCEDURE ZPN_PCI_InsereAtualizaPOItem
(
    @poitemid NVARCHAR(MAX),
    @gestatus INT,
    @gedataacao DATETIME,
    @item NVARCHAR(MAX),
    @poid NVARCHAR(MAX),
    @obraid NVARCHAR(MAX),
    @percentualparcela DECIMAL(18,2),
    @datalancamento DATETIME,
    @datafaturamento DATETIME,
    @numeronotafiscal NVARCHAR(MAX),
    @valor DECIMAL(18,2),
    @tipo CHAR(1),
    @clienteid NVARCHAR(MAX),
    @etapaid NVARCHAR(MAX),
    @observacao NVARCHAR(MAX),
    @codigo INT,
    @descricao NVARCHAR(MAX),
    @obracandidatoid NVARCHAR(MAX)
)
AS
BEGIN
	declare @count int;




	set @count = (select count(1) from  POitem where  poitemid = @poitemid);

	IF (@count = 0) 
	BEGIN
         INSERT INTO [POitem]
           ([poitemid]
           ,[gestatus]
           ,[gedataacao]
           ,[item]
           ,[poid]
           ,[obraid]
           ,[percentualparcela]
           ,[datalancamento]
           ,[datafaturamento]
           ,[numeronotafiscal]
           ,[valor]
           ,[tipo]
           ,[clienteid]
           ,[etapaid]
           ,[observacao]
           ,[codigo]
           ,[descricao]
           ,[obracandidatoid])
        VALUES
           (@poitemid,
            @gestatus,
            @gedataacao,
            @item,
            @poid,
            @obraid,
            @percentualparcela,
            @datalancamento,
            @datafaturamento,
            @numeronotafiscal,
            @valor,
            @tipo,
            @clienteid,
            @etapaid,
            @observacao,
            @codigo,
            @descricao,
            @obracandidatoid);
    END
    ELSE
    BEGIN
        -- Executa o update com base nos parâmetros fornecidos
        UPDATE POitem
        SET 
            gestatus = @gestatus,
            gedataacao = @gedataacao,
            item = @item,
            poid = @poid,
            obraid = @obraid,
            percentualparcela = @percentualparcela,
            datalancamento = @datalancamento,
            datafaturamento = @datafaturamento,
            numeronotafiscal = @numeronotafiscal,
            valor = @valor,
            tipo = @tipo,
            clienteid = @clienteid,
            etapaid = @etapaid,
            observacao = @observacao,
            descricao = @descricao,
            obracandidatoid = @obracandidatoid,
			[codigo] = @CODIGO
        WHERE poitemid = @poitemid;
    END;
END;



