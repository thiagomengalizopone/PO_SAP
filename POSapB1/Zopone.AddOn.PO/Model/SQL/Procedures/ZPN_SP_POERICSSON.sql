create PROCEDURE ZPN_SP_POERICSSON
(
	@NomeArquivo varchar(200),
	@PO numeric,
	@Obra varchar(20),
	@ITEM VARCHAR(5),
	@Codigo varchar(20),
	@Descricao varchar(150),
	@Qtde  decimal(16,4),
	@Site varchar(20),
	@Municipio varchar(100),
	@Piece decimal(16,4),
	@NBM varchar(20),
	@Importado varchar(10)

)
AS
BEGIN

	INSERT INTO 
		ZPN_POERICSSON
			(
				NomeArquivo,
				PO,
				Obra,
				ITEM,
				Codigo,
				Descricao,
				Qtde,
				Site,
				Municipio,
				Piece,
				NBM,
				Importado	
			)
		VALUES
			(
				@NomeArquivo,
				@PO,
				@Obra,
				@ITEM,
				@Codigo,
				@Descricao,
				@Qtde,
				@Site,
				@Municipio,
				@Piece,
				@NBM,
				@Importado			
			);
END;