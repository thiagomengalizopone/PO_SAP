create FUNCTION [dbo].[FN_ZPN_RetornaImpostosClaro] (@DocEntry INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Impostos VARCHAR(MAX);

    SELECT 
        @Impostos = 
            CASE WHEN IMP."IRRFWTAmnt" = 0 THEN '' ELSE 
                ' IRRF: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP."IrrfRate", 0), 'N2') + '% ' + 
                FORMAT(ISNULL(IMP."IRRFWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
            END
                +
            CASE WHEN IMP."PISWTAmnt" = 0 THEN '' ELSE 
                ' PIS: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP."PisRate", 0), 'N2') + '% ' + 
                FORMAT(ISNULL(IMP."PISWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
            END    
            +
            CASE WHEN IMP."CSLLWTAmnt" = 0 THEN '' ELSE 
                ' CSLL: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP."CSLLRate", 0), 'N2') + '% ' + 
                FORMAT(ISNULL(IMP."CSLLWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
            END
            +
            CASE WHEN IMP."COFINSWTAmnt" = 0 THEN '' ELSE 
                ' COFINS: ' + 
                -- Formatação de percentual com 2 casas decimais
                FORMAT(ISNULL(IMP."COFINSRate", 0), 'N2') + '% ' + 
                FORMAT(ISNULL(IMP."COFINSWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) 
            END
            +
            CASE WHEN (ISNULL(IMP."IrrfRate", 0) + ISNULL(IMP."PisRate", 0) + ISNULL(IMP."CSLLRate", 0) + ISNULL(IMP."COFINSRate", 0) = 0) THEN '' ELSE 
                ' PIS/COFINS/CSLL - ' + 
                -- Formatação do somatório de percentuais com 2 casas decimais
                FORMAT(
                    ISNULL(IMP."IrrfRate", 0) + ISNULL(IMP."PisRate", 0) + ISNULL(IMP."CSLLRate", 0) + ISNULL(IMP."COFINSRate", 0), 'N2') + '% ' +  
                ' ' + 
                -- Formatação do somatório de valores com a moeda local
                FORMAT(
                    ISNULL(IMP."IRRFWTAmnt", 0) + ISNULL(IMP."PISWTAmnt", 0) + ISNULL(IMP."CSLLWTAmnt", 0) + ISNULL(IMP."COFINSWTAmnt", 0), 'C', 'pt-BR') 
            END +
                CHAR(13) + CHAR(10) +
			CHAR(13) + CHAR(10) +
            -- Detalhes do valor a ser recebido e vencimento
            ' LIQUIDO A RECEBER: ' + FORMAT(ODRF.DocTotal, 'C', 'pt-BR') + CHAR(13) + CHAR(10) +
            CHAR(13) + CHAR(10) +
			CHAR(13) + CHAR(10) +
            ' VENCIMENTO: ' + FORMAT(ODRF.DocDueDate, 'dd/MM/yyyy') + CHAR(13) + CHAR(10)
    FROM 
        DRF1 
        INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
        LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.TipoDocumento = 'DRF' AND IMP.AbsEntry = ODRF.DocEntry
    WHERE
        DRF1.DocEntry = @DocEntry;

    RETURN @Impostos;
END;
GO


