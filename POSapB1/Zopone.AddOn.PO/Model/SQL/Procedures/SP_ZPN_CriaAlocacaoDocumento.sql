create PROCEDURE SP_ZPN_CriaAlocacaoDocumento
(
    @DocEntry INT,
    @ObjType INT,
    @DraftKey INT,
    @TipoDocumento VARCHAR(1),
    @CodAlocacao1 VARCHAR(20),
    @PercAlocacao1 DECIMAL(16,4),
    @CodAlocacao2 VARCHAR(20),
    @PercAlocacao2 DECIMAL(16,4),
    @CodAlocacao3 VARCHAR(20),
    @PercAlocacao3 DECIMAL(16,4),
    @CodAlocacao4 VARCHAR(20),
    @PercAlocacao4 DECIMAL(16,4),
    @CodAlocacao5 VARCHAR(20),
    @PercAlocacao5 DECIMAL(16,4)
)
AS
BEGIN
    DECLARE @Parcela INT = 0;
    DECLARE @Alocacoes TABLE (CodigoAlocacao VARCHAR(20), Percentual DECIMAL(16,4));
	declare @DocTotal decimal (16,4);

    IF (ISNULL(@CodAlocacao1, '') <> '') 
        INSERT INTO @Alocacoes (CodigoAlocacao, Percentual) VALUES (@CodAlocacao1, @PercAlocacao1);

    IF (ISNULL(@CodAlocacao2, '') <> '') 
        INSERT INTO @Alocacoes (CodigoAlocacao, Percentual) VALUES (@CodAlocacao2, @PercAlocacao2);

    IF (ISNULL(@CodAlocacao3, '') <> '') 
        INSERT INTO @Alocacoes (CodigoAlocacao, Percentual) VALUES (@CodAlocacao3, @PercAlocacao3);

    IF (ISNULL(@CodAlocacao4, '') <> '') 
        INSERT INTO @Alocacoes (CodigoAlocacao, Percentual) VALUES (@CodAlocacao4, @PercAlocacao4);

    IF (ISNULL(@CodAlocacao5, '') <> '') 
        INSERT INTO @Alocacoes (CodigoAlocacao, Percentual) VALUES (@CodAlocacao5, @PercAlocacao5);
    
    DELETE FROM ZPN_ALOCACAOPARCELANF 
    WHERE ObjType = @ObjType AND DocEntry = @DocEntry AND TipoDocumento = @TipoDocumento;

    SELECT 
		@DocTotal = SUM(DRF1.LineTotal)
    FROM 
		ODRF
    INNER JOIN DRF1 ON DRF1.DocEntry = ODRF.DocEntry
    WHERE 
		ODRF.ObjType = @ObjType 
		AND ODRF.DocEntry = @DocEntry 
		AND @TipoDocumento = 'E';

    SELECT 
		@DocTotal = SUM(INV1.LineTotal)
    FROM 
		OINV
    INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry
    WHERE 
		OINV.ObjType = @ObjType 
		AND OINV.DocEntry = @DocEntry 
		AND @TipoDocumento = 'N';


    -- Insere as novas alocações
    INSERT INTO ZPN_ALOCACAOPARCELANF
    (
        DocEntry, ObjType, DraftKey, Parcela, Percentural, ValorParcela, CodigoAlocacao, DescricaoAlocacao, TipoDocumento, IdPCI
    )
    SELECT 
        @DocEntry,
        @ObjType,
        @DraftKey,
        @Parcela,
        A.Percentual,
        @DocTotal * A.Percentual / 100,
        Z.Code,
        Z.U_Desc,
		@TipoDocumento,
        NEWID()
    FROM @Alocacoes A
    INNER JOIN "@ZPN_ALOCA" Z ON Z.Code = A.CodigoAlocacao;

END;
