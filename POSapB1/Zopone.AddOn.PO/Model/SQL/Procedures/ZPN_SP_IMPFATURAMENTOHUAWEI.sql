CREATE PROCEDURE ZPN_SP_IMPFATURAMENTOHUAWEI
(
	@NomeArquivo varchar(500),
	@NumeroPO varchar(500),
	@NumeroLinha varchar(10),
	@QtdeFaturada numeric,
	@CodigoServico varchar(100),
	@Item varchar(100),
	@ValorUnitario  decimal(16,4),
	@ValorTotal  decimal(16,4)
)
as 
BEGIN

	INSERT INTO 
		ZPN_IMPFATURAMENTOHUAWEI
			(
				NomeArquivo,
				NumeroPO,
				NumeroLinha,
				QtdeFaturada,
				CodigoServico,
				Item,
				ValorUnitario,
				ValorTotal	
			)
	VALUES
		(
			@NomeArquivo,
			@NumeroPO,
			@NumeroLinha,
			@QtdeFaturada,
			@CodigoServico,
			@Item,
			@ValorUnitario,
			@ValorTotal		
		);


END;

