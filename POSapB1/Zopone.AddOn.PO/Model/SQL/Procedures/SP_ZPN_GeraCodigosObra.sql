CREATE PROCEDURE SP_ZPN_GeraCodigosObra
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
        -- Extrai a parte entre o '.' e o '/'
        SUBSTRING(Code, CHARINDEX('.', Code) + 1, 
                  CHARINDEX('/', Code) - CHARINDEX('.', Code) - 1) AS ExtractedValue,
        -- Extrai o ano após o '/'
        CAST(SUBSTRING(Code, CHARINDEX('/', Code) + 1, LEN(Code)) AS INT) AS YearValue
    FROM 
        "@ZPN_OPRJ"
    WHERE
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
    CONCAT(@Prefixo, '.', RIGHT('0000' + CAST(Codigo AS VARCHAR(10)), LEN(@CodigoInicial)), '/', @Sufixo) AS "CodObra",
	GEtdatE() "Cadastro",
	'                                 ' Regional,
	'																											                                            																											                                            ' Validacao,
	'                                ' "IdContrato",
	'                                ' "IdRegional",
	'                                ' "IdCliente"
	
FROM 
    CTE_Codigos
OPTION (MAXRECURSION 0);



END;