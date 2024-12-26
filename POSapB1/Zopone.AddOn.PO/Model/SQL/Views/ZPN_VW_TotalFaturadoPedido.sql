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
        DRF21.RefDocEntr "U_BaseEntry",
	    RDR1.LineNum  U_BaseLine,
        SUM(DRF1."LineTotal" * (RDR1.LineTotal / ORDR.DocTotal)) AS "LineTotal",
        0 AS "TotalDevolucao",
        SUM(DRF1."LineTotal"* (RDR1.LineTotal / ORDR.DocTotal)) AS "SaldoFaturado"
    FROM
        DRF1
        INNER JOIN ODRF ON ODRF."DocEntry" = DRF1."DocEntry"
        INNER JOIN DRF21 ON DRF21.DocEntry = ODRF.DocEntry
        CROSS APPLY STRING_SPLIT(DRF21.Remark, ' ') AS SplitRemark
        INNER JOIN RDR1 ON RDR1.DocEntry = DRF21.RefDocEntr
            AND RDR1.LineNum = CAST(SplitRemark.value AS INT) AND DRF1.Project = RDR1.Project
	    INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
    WHERE
        ODRF.ObjType = '13'
        AND ODRF."CANCELED" = 'N'
        AND ODRF."DocStatus" = 'O'
    GROUP BY
        DRF21.RefDocEntr,
        DRF1.Project,
	    RDR1.LineNum


    UNION ALL

   SELECT
        INV21.RefDocEntr "U_BaseEntry",
	    RDR1.LineNum  U_BaseLine,
        SUM(INV1."LineTotal" * (RDR1.LineTotal / ORDR.DocTotal)) AS "LineTotal",
        0 AS "TotalDevolucao",
        SUM(INV1."LineTotal"* (RDR1.LineTotal / ORDR.DocTotal)) AS "SaldoFaturado"
    FROM
        INV1
        INNER JOIN OINV ON OINV."DocEntry" = INV1."DocEntry"
        INNER JOIN INV21 ON INV21.DocEntry = OINV.DocEntry
        CROSS APPLY STRING_SPLIT(INV21.Remark, ' ') AS SplitRemark
        INNER JOIN RDR1 ON RDR1.DocEntry = INV21.RefDocEntr
            AND RDR1.LineNum = CAST(SplitRemark.value AS INT) AND INV1.Project = RDR1.Project
	    INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
    WHERE
        OINV.ObjType = '13'
        AND OINV."CANCELED" = 'N'
        AND OINV."DocStatus" = 'O'
    GROUP BY
        INV21.RefDocEntr,
        INV1.Project,
	    RDR1.LineNum
) AS CombinedData
GROUP BY 
    U_BaseEntry,
    U_BaseLine;


