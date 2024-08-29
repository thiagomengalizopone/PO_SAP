

CREATE VIEW ZPN_VW_TotalFaturadoPedido
as
SELECT 
    U_BaseEntry,
    U_BaseLine,
    SUM("LineTotal")		AS "LineTotal",
    SUM("TotalDevolucao")	AS "TotalDevolucao",
    SUM("SaldoFaturado")	AS "SaldoFaturado"
FROM 
(
    SELECT
        INV1.U_BaseEntry,
        INV1.U_BaseLine,
        SUM(INV1."LineTotal") AS "LineTotal",
        0 AS "TotalDevolucao",
        SUM(INV1."LineTotal") AS "SaldoFaturado"
    FROM
        INV1 
        INNER JOIN OINV ON OINV."DocEntry" = INV1."DocEntry"
    WHERE
        OINV."CANCELED" = 'N' and ISNULL(INV1.U_BaseEntry,0) <> 0
    GROUP BY
        INV1.U_BaseEntry,
        INV1.U_BaseLine

    UNION ALL

    SELECT
        DRF1.U_BaseEntry,
        DRF1.U_BaseLine,
        SUM(DRF1."LineTotal") AS "LineTotal",
        0 AS "TotalDevolucao",
        SUM(DRF1."LineTotal") AS "SaldoFaturado"  
    FROM
        DRF1 
        INNER JOIN ODRF ON ODRF."DocEntry" = DRF1."DocEntry"
    WHERE
        ODRF."CANCELED" = 'N' AND ODRF."DocStatus" = 'O' and ISNULL(DRF1.U_BaseEntry,0) <> 0
    GROUP BY
        DRF1.U_BaseEntry,
        DRF1.U_BaseLine
) AS CombinedData
GROUP BY 
    U_BaseEntry,
    U_BaseLine;


