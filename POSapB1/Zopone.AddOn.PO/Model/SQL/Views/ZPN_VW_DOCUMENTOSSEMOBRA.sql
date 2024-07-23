create VIEW ZPN_VW_DOCUMENTOSSEMOBRA
AS

SELECT DISTINCT
	ORDR.U_IdPO, 
	ORDR."DocEntry",
	ORDR."ObjType",
	'E' "Documento"
FROM 
	RDR1 
	INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
WHERE 
	ISNULL(RDR1.Project,'') = '' AND 
	ISNULL(ORDR.U_IdPO,0) <> 0

UNION ALL

SELECT DISTINCT
	ODRF.U_IdPO, 
	odrf."DocEntry",
	ODRF."ObjType",
	'E' "Documento"
FROM 
	DRF1 
	INNER JOIN ODRF ON ODRF."DocEntry" = DRF1."DocEntry"
WHERE 
	ISNULL(DRF1.Project,'') = '' AND 
	ISNULL(ODRF.U_IdPO,0) <> 0