
CREATE PROCEDURE SP_ZPN_ExibeAlocacaoDocumento
    @IdPCIDocumento varchar(250)
AS
BEGIN
    SELECT 
        [Parcela],
        [Percentual],
        [ValorParcela],
        [CodigoAlocacao],
        [DescricaoAlocacao],
        [IdPCI]
    FROM 
        ZPN_ALOCACAOPARCELANF
    WHERE 
        IdPCIDocumento = @IdPCIDocumento
	ORDER BY
		Parcela;
END
