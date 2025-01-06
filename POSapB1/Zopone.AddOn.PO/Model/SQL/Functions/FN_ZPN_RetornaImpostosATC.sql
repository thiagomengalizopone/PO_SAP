CREATE FUNCTION [dbo].[FN_ZPN_RetornaImpostosATC] (@DocEntry INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Impostos VARCHAR(MAX);

    SELECT 
        @Impostos = 

		' ISS: ' + ISNULL(OBRA.U_CidadeDesc, '') + ' /ISS: ' +
						-- Formatação de percentual com 2 casas decimais
						FORMAT(ISNULL(IMP.ISSRate, 0), 'N2') + '%  - ' + 
						FORMAT(ISNULL(IMP.ISSWtAmnt, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10)  +
				
                ' BASE DE CÁLCULO P/ RETENÇÃO...: ' + 
                FORMAT(ISNULL(IMP.INSSTaxbleAmnt, 0), 'C', 'pt-BR') 
				+ CHAR(13) + CHAR(10) 
                +
                ' RETENÇÃO P/ PREVIDENCIA SOCIAL: ' + 
                FORMAT(ISNULL(IMP.INSSWTAmnt, 0), 'C', 'pt-BR') 
				+ CHAR(13) + CHAR(10) +
                case WHEN  IMP.INSSTAXBLEAMNT = DRF1.LineTotal then ''
				else
				' 50% MATERIAIS..........................: ' + FORMAT(ISNULL(DRF1.LineTotal * 0.5, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
			    ' 50% SERVIÇO............................: ' + FORMAT(ISNULL(DRF1.LineTotal * 0.5, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
				end +



                +

			+CHAR(13) + CHAR(10) +
            ' VENCIMENTO: ' + FORMAT(ODRF.DocDueDate, 'dd/MM/yyyy') + CHAR(13) + CHAR(10)
    FROM 
        DRF1 
        INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
        LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.TipoDocumento = 'DRF' AND IMP.AbsEntry = ODRF.DocEntry
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
    WHERE
        DRF1.DocEntry = @DocEntry;

    RETURN @Impostos;
END;



