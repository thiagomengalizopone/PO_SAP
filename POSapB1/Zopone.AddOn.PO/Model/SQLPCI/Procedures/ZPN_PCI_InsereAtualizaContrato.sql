create procedure [dbo].[ZPN_PCI_InsereAtualizaContrato]
(
	 @contratoid varchar(250),
	 @gestatus INT,
	 @gedataacao DATETIME,
	 @referencia VARCHAR(200),
	 @descricao TEXT,
	 @filialid UNIQUEIDENTIFIER,
	 @clienteid UNIQUEIDENTIFIER,
	 @iniciocontrato DATETIME,
	 @terminocontrato DATETIME,
	 @datacadastro DATETIME,
	 @codigo INT
)
AS 
BEGIN

DECLARE @COUNT INT;



SET @COUNT = (SELECT COUNT(1) FROM [contrato] WHERE contratoid = @contratoid);

IF (@COUNT = 0) 
BEGIN

	INSERT INTO [contrato]
			   ([contratoid]
			   ,[gestatus]
			   ,[gedataacao]
			   ,[referencia]
			   ,[descricao]
			   ,[filialid]
			   ,[clienteid]
			   ,[iniciocontrato]
			   ,[terminocontrato]
			   ,[datacadastro]
			   ,[codigo])
		 VALUES
			   (
					 @contratoid,
					 @gestatus,
					 @gedataacao,
					 @referencia,
					 @descricao,
					 @filialid,
					 @clienteid,
					 @iniciocontrato,
					 @terminocontrato,
					 @datacadastro,
					 @codigo

			   );

END;
ELSE
BEGIN
	UPDATE [contrato]
		SET 
		 [gestatus] = @gestatus
		,[gedataacao] = @gedataacao
		,[referencia] =@referencia
		,[descricao] = @descricao
		,[filialid] = @filialid
		,[clienteid] = @clienteid
		,[terminocontrato] = @terminocontrato
		,[datacadastro] = @datacadastro
		,[codigo] = @codigo
	WHERE 
		[contratoid] = @contratoid;

END;

end;