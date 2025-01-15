USE [SBO_ZOPONE_ENGENHARIA]
GO
/****** Object:  StoredProcedure [dbo].[SP_ZPN_GeraCodigosObra]    Script Date: 15/01/2025 17:05:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_ZPN_GeraCodigosObra]
(
	@Prefixo VARCHAR(10),  
    @Quantidade INT

)
AS 
BEGIN


declare @CodigoInicial int, 
        @Sufixo VARCHAR(10);

/*
DECLARE @Prefixo VARCHAR(10), 
        
        @Quantidade INT;

SET @Prefixo = 'R5';


SET @Quantidade = 20;
*/

SET @Sufixo = substring(cast(YEAR(GETDATE()) as varchar(4)), 3, 2);

WITH CTE AS (
    SELECT 
        Code,
        SUBSTRING(Code, CHARINDEX('.', Code) + 1, 
                  CHARINDEX('/', Code) - CHARINDEX('.', Code) - 1) AS ExtractedValue,
        CAST(SUBSTRING(Code, CHARINDEX('/', Code) + 1, LEN(Code)) AS INT) AS YearValue
    FROM 
        "@ZPN_OPRJ"
    WHERE
        CAST(SUBSTRING(Code, CHARINDEX('/', Code) + 1, LEN(Code)) AS INT) = right(cast(YEAR(GETDATE()) as varchar(4)),2) and
		PATINDEX('[A-Za-z][0-9].____/__', "Code") > 0 and
		LEN(Code) = 10
)



SELECT 
     @CodigoInicial=  isnull(MAX(CAST(ExtractedValue AS INT)),0)+1
FROM 
    CTE
WHERE 
    YearValue = substring(cast(YEAR(GETDATE()) as varchar(4)), 3, 2)




DECLARE @CodigoBase INT;
SET @CodigoBase = CAST(@CodigoInicial AS INT);

WITH CTE_Codigos AS (
    SELECT 
        @CodigoBase AS Codigo,
        1 AS Contador
    UNION ALL
    SELECT 
        Codigo + 1,
        Contador + 1
    FROM 
        CTE_Codigos
    WHERE 
        Contador < @Quantidade
)

SELECT 
	'                                ' "Site",
	'                                ' "CodCli", 
	'                                                                                                                                                                                                ' "Cliente", 
	'                                                                ' "Contrato", 
    CONCAT(@Prefixo, '.', RIGHT('0000' + CAST(Codigo AS VARCHAR(10)), 4), '/', @Sufixo) AS "CodObra",
	GEtdatE() "Cadastro",
	'                                 ' Regional,
	'																											                                            																											                                            ' Validacao,
	'                                ' "IdContrato",
	'                                ' "IdRegional",
	'                                ' "IdCliente",
    '                                ' "Localizacao"
	
FROM 
    CTE_Codigos
OPTION (MAXRECURSION 0);



END;