﻿create VIEW ZPN_VW_DOCUMENTOSIMPOSTO AS

SELECT 
    T99."AbsEntry",
    ISNULL(SUM(CASE WHEN T97.WTType = 'COFINS' THEN T99.WTAmnt ELSE 0 END),0) AS COFINS,
    ISNULL(SUM(CASE WHEN T97.WTType = 'CSLL' THEN T99.WTAmnt ELSE 0 END),0) AS CSLL,
    ISNULL(SUM(CASE WHEN T97.WTType = 'IRRF' THEN T99.WTAmnt ELSE 0 END),0) AS IRRF,
	ISNULL(SUM(CASE WHEN T97.WTType = 'PIS' THEN T99.WTAmnt ELSE 0 END),0) AS PIS,
	ISNULL(SUM(CASE WHEN T97.WTType = 'INSS' THEN T99.WTAmnt ELSE 0 END),0) AS INSS,
	ISNULL(SUM(CASE WHEN T97.WTType = 'ISS' THEN T99.WTAmnt ELSE 0 END),0) AS ISS
FROM 
	INV5 T99
	INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
	INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId
GROUP BY 
	T99."AbsEntry";


	