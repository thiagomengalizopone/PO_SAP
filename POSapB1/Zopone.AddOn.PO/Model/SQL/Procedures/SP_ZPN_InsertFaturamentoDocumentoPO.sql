create PROCEDURE SP_ZPN_InsertFaturamentoDocumentoPO
    @DataHoraFaturamento datetime,
    @BaseEntry int,
    @BaseType int,
    @BaseLine int,
    @LineTotal decimal(16,4)
AS
BEGIN
	INSERT INTO ZPN_FATURADOCUMENTOPO 
    (
         DataHoraFaturamento,
         BaseEntry,
         BaseType,
         BaseLine,
         LineTotal
    )
    VALUES
    (
        @DataHoraFaturamento,
        @BaseEntry,
        @BaseType,
        @BaseLine,
        @LineTotal
    );

        
END;
