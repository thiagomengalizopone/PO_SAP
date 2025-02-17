create PROCEDURE [dbo].[SP_ZPN_ExibeAlocacaoDocumento]
    @DocEntry int
AS
BEGIN
    SELECT 
         ALOCA.[Parcela],
         ALOCA.[Percentual],
		 INV1.LineTotal * ALOCA.[Percentual] / 100 [ValorParcelaBruto],
         OINV."DocTotal" * ALOCA.[Percentual] / 100 [ValorParcela],
         ALOCA.[CodigoAlocacao],
         ALOCA.[DescricaoAlocacao],
         ALOCA.[IdPCI]
    FROM 
        ZPN_ALOCACAOPARCELANF ALOCA
	    INNER JOIN OINV ON OINV.U_IdPCI = ALOCA.IdPCIDocumento
		INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry
    WHERE 
         OINV.DocEntry =  @DocEntry
	ORDER BY
		Parcela;
END
