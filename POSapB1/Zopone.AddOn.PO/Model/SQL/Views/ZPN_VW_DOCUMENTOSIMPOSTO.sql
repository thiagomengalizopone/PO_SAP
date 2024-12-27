create VIEW [ZPN_VW_DOCUMENTOSIMPOSTO] AS

SELECT 
    TipoDocumento,
    AbsEntry,
    isnull(MAX(CASE WHEN WTType = 'IRRF' THEN Rate ELSE NULL END),0) AS "IrrfRate",
    isnull(MAX(CASE WHEN WTType = 'PIS' THEN Rate ELSE NULL END),0) AS "PisRate",
    isnull(MAX(CASE WHEN WTType = 'CSLL' THEN Rate ELSE NULL END),0) AS "CSLLRate",
    isnull(MAX(CASE WHEN WTType = 'COFINS' THEN Rate ELSE NULL END),0) AS "COFINSRate",
	isnull(MAX(CASE WHEN WTType = 'ISSF' THEN Rate ELSE NULL END),0) AS "ISSRate",
	isnull(MAX(CASE WHEN WTType = 'INSS' THEN Rate ELSE NULL END),0) AS "INSSRate",

    isnull(SUM(CASE WHEN WTType = 'IRRF' THEN TaxbleAmnt ELSE 0 END),0) AS "IRRFTaxbleAmnt",
    isnull(SUM(CASE WHEN WTType = 'PIS' THEN TaxbleAmnt ELSE 0 END),0) AS "PISTaxbleAmnt",
    isnull(SUM(CASE WHEN WTType = 'CSLL' THEN TaxbleAmnt ELSE 0 END),0) AS "CSLLTaxbleAmnt",
    isnull(SUM(CASE WHEN WTType = 'COFINS' THEN TaxbleAmnt ELSE 0 END),0) AS "COFINSTaxbleAmnt",
	isnull(SUM(CASE WHEN WTType = 'ISSF' THEN TaxbleAmnt ELSE 0 END),0) AS "ISSTaxbleAmnt",
	isnull(SUM(CASE WHEN WTType = 'INSS' THEN TaxbleAmnt ELSE 0 END),0) AS "INSSTaxbleAmnt",

    isnull(SUM(CASE WHEN WTType = 'IRRF' THEN WTAmnt ELSE 0 END),0) AS "IRRFWTAmnt",
    isnull(SUM(CASE WHEN WTType = 'PIS' THEN WTAmnt ELSE 0 END),0) AS "PISWTAmnt",
    isnull(SUM(CASE WHEN WTType = 'CSLL' THEN WTAmnt ELSE 0 END),0) AS "CSLLWTAmnt",
    isnull(SUM(CASE WHEN WTType = 'COFINS' THEN WTAmnt ELSE 0 END),0) AS "COFINSWTAmnt",
	isnull(SUM(CASE WHEN WTType = 'ISSF' THEN WTAmnt ELSE 0 END),0) AS "ISSWTAmnt",
	isnull(SUM(CASE WHEN WTType = 'INSS' THEN WTAmnt ELSE 0 END),0) AS "INSSWTAmnt"

FROM 

    (SELECT 
        'INV' AS TipoDocumento,
        T99."AbsEntry",
        T98.Rate,
        T97.WTType,
        T99.TaxbleAmnt,
        T99.WTAmnt
    FROM 
        INV5 T99
    INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
    INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId
    

    UNION ALL 

    SELECT 
        'DRF' AS TipoDocumento,
        T99."AbsEntry",
        T98.Rate,
        T97.WTType,
        T99.TaxbleAmnt,
        T99.WTAmnt
    FROM 
        DRF5 T99
    INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
    INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId
	 ) AS Subquery

GROUP BY 
    TipoDocumento,
    aBSeNTRY;
