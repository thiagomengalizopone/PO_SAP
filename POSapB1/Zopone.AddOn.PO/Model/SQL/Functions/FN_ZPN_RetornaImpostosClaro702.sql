CREATE FUNCTION [dbo].[FN_ZPN_RetornaImpostosClaro702] (@DocEntry INT)
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
            case 
                when ISNULL(IMP.ISSWTAmnt, 0) = 0 then '' 
                else 
                    ' ISS: ' + 
                    -- Formatação de percentual com 2 casas decimais
                    FORMAT(ISNULL(IMP."ISSRate", 0), 'N2') + '% ' + 
                    FORMAT(ISNULL(IMP.ISSWTAmnt, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			        + CHAR(13) + CHAR(10)
			        + CHAR(13) + CHAR(10) 
                end + 
			' LIQUIDO A RECEBER: ' + FORMAT(ODRF.DocTotal-ISNULL(IMPRET."WTAmnt",0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			+ CHAR(13) + CHAR(10)
            + ' VENCIMENTO: ' + FORMAT(ODRF.DocDueDate, 'dd/MM/yyyy') + CHAR(13) + CHAR(10)
    FROM 
        DRF1 
        INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
        LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.TipoDocumento = 'DRF' AND IMP.AbsEntry = ODRF.DocEntry
		LEFT JOIN VW_ZPN_TotalImpostoRetidoPagamento IMPRET ON IMPRET.AbsEntry = ODRF.DocEntry
    WHERE
        DRF1.DocEntry = @DocEntry;

    RETURN @Impostos;
END;



