CREATE PROCEDURE [dbo].ZPN_PCI_InsereAtualizaPO
    @poid VARCHAR(100),
    @gestatus INT,
    @gedataacao DATETIME,
    @empresaid VARCHAR(100),
    @descricao NVARCHAR(MAX),
    @pedido NVARCHAR(50),
    @valor DECIMAL(18, 2),
    @data DATETIME,
    @contratocliente NVARCHAR(MAX),
	@Codigo int
AS
BEGIN

	declare @count int;

	set @count = (select count(1) from  [po] where [poid] = @poid);

	delete from poitem where poid = @poid;

	IF (@count = 0) 
	BEGIN

		-- Inserir dados na tabela final
		INSERT INTO [po]
		   ([poid]
		   ,[gestatus]
		   ,[gedataacao]
		   ,[empresaid]
		   ,[descricao]
		   ,[pedido]
		   ,[valor]
		   ,[data]
		   ,[contratocliente]
		   ,[codigo])
		VALUES
		   (@poid,
			@gestatus,
			@gedataacao,
			@empresaid,
			@descricao,
			@pedido,
			@valor,
			@data,
			@contratocliente,
			@codigo
			);
	END
	ELSE
	BEGIN
		 UPDATE PO
        SET 
            gestatus = @gestatus,
            gedataacao = @gedataacao,
            empresaid = @empresaid,
            descricao = @descricao,
            pedido = @pedido,
            valor = @valor,
            data = @data,
            contratocliente = @contratocliente,
			codigo = @Codigo
        WHERE 
            [poid] = @poid;

	END
END