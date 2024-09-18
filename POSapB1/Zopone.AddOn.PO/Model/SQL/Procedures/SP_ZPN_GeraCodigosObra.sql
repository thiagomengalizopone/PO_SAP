CREATE PROCEDURE SP_ZPN_GeraCodigosObra
(
	@Prefixo VARCHAR(10), 
    @CodigoInicial VARCHAR(10), 
    @Sufixo VARCHAR(10), 
    @Quantidade INT

)
AS 
BEGIN



/*
DECLARE @Prefixo VARCHAR(10), 
        @CodigoInicial VARCHAR(10), 
        @Sufixo VARCHAR(10), 
        @Quantidade INT;

SET @Prefixo = 'R5';
SET @CodigoInicial = '0009';
SET @Sufixo = '25';
SET @Quantidade = 20;
*/

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
    CONCAT(@Prefixo, '.', RIGHT('0000' + CAST(Codigo AS VARCHAR(10)), LEN(@CodigoInicial)), '/', @Sufixo) AS "Código Gerado",
	CASE 
		WHEN ISNULL(OPRJ."PrjCode",'') <> '' THEN 'Obra gerada!'
		Else ''
	end 'Status'
FROM 
    CTE_Codigos
	LEFT JOIN OPRJ ON OPRJ."PrjCode" = CONCAT(@Prefixo, '.', RIGHT('0000' + CAST(Codigo AS VARCHAR(10)), LEN(@CodigoInicial)), '/', @Sufixo) 
OPTION (MAXRECURSION 0);



END;