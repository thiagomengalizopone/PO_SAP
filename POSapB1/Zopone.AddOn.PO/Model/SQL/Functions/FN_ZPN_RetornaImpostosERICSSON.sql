
create FUNCTION [dbo].[FN_ZPN_RetornaImpostosERICSSON] (@DocEntry INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Impostos VARCHAR(MAX);

    SELECT 
        @Impostos = 

                ' IRRF: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP."IrrfRate", 0), 'N2') + '% ' + 
                FORMAT(ISNULL(IMP."IRRFWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 


				+
                ' PIS: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP."PisRate", 0), 'N2') + '% ' + 
                FORMAT(ISNULL(IMP."PISWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 

            +

                ' ALIQUOTA INSS: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP.INSSRate, 0), 'N2') + '% ' 
                + 

                ' BASE DE CÁLCULO P/ RETENÇÃO...: ' + 
                FORMAT(ISNULL(IMP.INSSTaxbleAmnt, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
                +
                ' RETENÇÃO P/ PREVIDENCIA SOCIAL: ' + 
                FORMAT(ISNULL(IMP.INSSWTAmnt, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
                +

                ' ISS: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP.ISSRate, 0), 'N2') + '%  - ' + 
                FORMAT(ISNULL(IMP.ISSWtAmnt, 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 

                +
                ' DADOS BANCÁRIOS: BCO BRADESCO (BAURU) / AG.: 3384-7 - C/C: 2887-8 ' 
                +
			+CHAR(13) + CHAR(10) +
            ' VENCIMENTO: ' + FORMAT(ODRF.DocDueDate, 'dd/MM/yyyy') + CHAR(13) + CHAR(10)
    FROM 
        DRF1 
        INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
        LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.TipoDocumento = 'DRF' AND IMP.AbsEntry = ODRF.DocEntry
    WHERE
        DRF1.DocEntry = @DocEntry;

    RETURN @Impostos;
END;



