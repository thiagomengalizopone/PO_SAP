CREATE VIEW ZPN_VW_STATUSVALIDACAOPEDIDOSPO
AS


SELECT 
    ORDR.DocEntry,
    CASE 
        WHEN COUNT(CASE 
                WHEN ISNULL(RDR1.U_StatusImp, '') IN ('N', '') THEN 1 
            END) > 0 THEN 'N' 
        ELSE 'Y' 
    END AS "Validado"
FROM 
    ORDR
LEFT JOIN 
    RDR1 ON RDR1.DocEntry = ORDR.DocEntry
GROUP BY 
    ORDR.DocEntry;
