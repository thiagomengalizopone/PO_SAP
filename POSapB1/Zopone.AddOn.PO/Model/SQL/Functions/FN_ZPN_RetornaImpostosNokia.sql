create FUNCTION FN_ZPN_RetornaImpostosNokia (@DocEntry INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Impostos VARCHAR(MAX);

    SELECT 
        @Impostos = 
            ' IRRF: ' + 
            -- Formatação de percentual com 2 casas decimais
            FORMAT(ISNULL(IMP."IrrfRate", 0), 'N2') + '% ' + 
            FORMAT(ISNULL(IMP."IRRFWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +

            ' PIS: ' + 
            -- Formatação de percentual com 2 casas decimais
            FORMAT(ISNULL(IMP."PisRate", 0), 'N2') + '% ' + 
            FORMAT(ISNULL(IMP."PISWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +

            ' CSLL: ' + 
            -- Formatação de percentual com 2 casas decimais
            FORMAT(ISNULL(IMP."CSLLRate", 0), 'N2') + '% ' + 
            FORMAT(ISNULL(IMP."CSLLWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +

            ' COFINS: ' + 
            -- Formatação de percentual com 2 casas decimais
            FORMAT(ISNULL(IMP."COFINSRate", 0), 'N2') + '% ' + 
            FORMAT(ISNULL(IMP."COFINSWTAmnt", 0), 'C', 'pt-BR') + CHAR(13) + CHAR(10) +

            ' PIS/COFINS/CSLL - ' + 
            -- Formatação do somatório de percentuais com 2 casas decimais
            FORMAT(
                ISNULL(IMP."IrrfRate", 0) + ISNULL(IMP."PisRate", 0) + ISNULL(IMP."CSLLRate", 0) + ISNULL(IMP."COFINSRate", 0), 'N2') + '% ' +  
            ' ' + 
            -- Formatação do somatório de valores com a moeda local
            FORMAT(
                ISNULL(IMP."IRRFWTAmnt", 0) + ISNULL(IMP."PISWTAmnt", 0) + ISNULL(IMP."CSLLWTAmnt", 0) + ISNULL(IMP."COFINSWTAmnt", 0), 'C', 'pt-BR') +
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
