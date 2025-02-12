CREATE PROCEDURE SP_ZPN_ExibeAlocacaoDocumento
    @IdPCIDocumento varchar(250)
AS
BEGIN
    SELECT 
         ALOCA.[Parcela],
         ALOCA.[Percentual],
		 INV1.LineTotal * ALOCA.[Parcela] / 100 [ValorParcelaBruto],
         OINV."DocTotal" * ALOCA.[Parcela] / 100 [ValorParcela],
         ALOCA.[CodigoAlocacao],
         ALOCA.[DescricaoAlocacao],
         ALOCA.[IdPCI]
    FROM 
        ZPN_ALOCACAOPARCELANF ALOCA
		INNER JOIN OINV ON OINV.U_IdPCI = ALOCA.[IdPCI]
		INNER JOIN INV1 ON OINV.DocEntry = OINV.DocEntry
    WHERE 
         ALOCA.IdPCIDocumento = @IdPCIDocumento
	ORDER BY
		Parcela;
END