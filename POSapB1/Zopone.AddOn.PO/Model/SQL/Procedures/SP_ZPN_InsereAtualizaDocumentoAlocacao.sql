create PROCEDURE SP_ZPN_InsereAtualizaDocumentoAlocacao 
(
	@DocEntry int,
	@ObjType int,
	@TipoDocumento varchar(1),
	@DraftKey int,
	@Percentual decimal(16,4),
	@ValorParcela decimal(16,4),
	@CodigoAlocacao varchar(20),
	@DescricaoAlocacao varchar(250),
	@IdPCI varchar(250),
	@IdPCIDocumento varchar(250)
)
AS 
BEGIN
	DECLARE @Parcela int;

	SELECT @Parcela = ISNULL(MAX(Parcela), 0) + 1 FROM ZPN_ALOCACAOPARCELANF;

	if (isnull(@IdPCI,'') = '')
	begin
		set @IdPCI = newid();
	end;


	IF EXISTS (SELECT 1 FROM ZPN_ALOCACAOPARCELANF WHERE IdPCI = @IdPCI) 
	BEGIN
		UPDATE ZPN_ALOCACAOPARCELANF
		SET
			DocEntry = @DocEntry,
			ObjType = @ObjType,
			TipoDocumento = @TipoDocumento,
			DraftKey = @DraftKey,
			Percentual = @Percentual,
			ValorParcela = @ValorParcela,
			CodigoAlocacao = @CodigoAlocacao,
			DescricaoAlocacao = @DescricaoAlocacao,
			IdPCIDocumento = @IdPCIDocumento
		WHERE IdPCI = @IdPCI;
	END
	ELSE
	BEGIN
		
		INSERT INTO ZPN_ALOCACAOPARCELANF
			(
				DocEntry,
				ObjType,
				TipoDocumento,
				DraftKey,
				Parcela,
				Percentual,
				ValorParcela,
				CodigoAlocacao,
				DescricaoAlocacao,
				IdPCI,
				IdPCIDocumento
			)
		VALUES
			(
				@DocEntry,
				@ObjType,
				@TipoDocumento,
				@DraftKey,
				@Parcela,  
				@Percentual,
				@ValorParcela,
				@CodigoAlocacao,
				@DescricaoAlocacao,
				@IdPCI,
				@IdPCIDocumento
			);
	END;
END;
