


CREATE FUNCTION [dbo].[FN_ZPN_RetornaImpostosWinity] (@DocEntry INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Impostos VARCHAR(MAX);
	
    SELECT 
        @Impostos = 
            ' BASE DE CÁLCULO P/RETENÇÃO...: ' + 
            FORMAT(ISNULL(IMP."INSSTaxbleAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			' RETENÇÃO P/ PREVIDÊNCIA SOCIAL: ' + 
            FORMAT(ISNULL(IMP."INSSWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			' 50% MATERIAIS..........................: ' + FORMAT(ISNULL(ODRF.GrosProfit * 0.5, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			' 50% SERVIÇO............................: ' + FORMAT(ISNULL(ODRF.GrosProfit * 0.5, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
            ' ISS: ' + 
            -- Formatação de percentual com 2 casas decimais
            FORMAT(ISNULL(IMP."ISSRate", 0), 'N2') + '% ' + 
            FORMAT(ISNULL(IMP.ISSWTAmnt, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			+ CHAR(13) + CHAR(10)
			
			+ CHAR(13) + CHAR(10) +
			' DADOS BANCÁRIOS: BCO BRADESCO (BAURU) / AG.: 3387-7 - C/C: 2887-8 '
			+ CHAR(13) + CHAR(10)

            + ' VENCIMENTO: ' + FORMAT(ODRF.DocDueDate, 'dd/MM/yyyy') + CHAR(13) + CHAR(10)
    FROM 
        DRF1 
        INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
        LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.TipoDocumento = 'DRF' AND IMP.AbsEntry = ODRF.DocEntry
    WHERE
        DRF1.DocEntry = @DocEntry;

    RETURN @Impostos;
END;
GO


