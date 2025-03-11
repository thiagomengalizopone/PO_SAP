create PROCEDURE SP_ZPN_PCI_ListaNFsPCI
as 
BEGIN

	declare @QtdeMinutos int = -10;


	SELECT DISTINCT 
		OINV.DocEntry
	FROM 
		OINV
	INNER JOIN OBPL ON OBPL.BPLId = OINV.BPLId
	INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry
	LEFT  JOIN "@ZPN_OPRJ" ZPN_OPRJ ON ZPN_OPRJ.Code = INV1.Project
	LEFT  JOIN sbo_taxOne.[dbo].doc ON DOC.DocType = oinv.ObjType AND DOC.DocEntry = OINV.DocEntry
	LEFT  JOIN sbo_taxOne.[dbo].Entidade ET ON ET.id = doc.EntityId
	LEFT JOIN sbo_taxOne.[dbo].[DocHist] HIST ON HIST.BatchId = DOC.BatchId
	WHERE 
		(
			(
				oinv.CreateDAte = cast(getdate() AS date)
				OR oinv.UpdateDate = cast(getdate() AS date)
			)
		)
	  AND 
		(
			(
				hist.StatusId = 4
				AND ISNULL(DOC.SerialNFSe, 0) <> 0
				AND DOC.StatusId = 4
				AND ET.CompanyDb= 'SBO_ZOPONE_ENGENHARIA'
				AND hist.DateAdd >= DATEADD(MINUTE, @QtdeMinutos, GETDATE())
			)
		OR 
		(
			(
				oinv.Bplid = 12
				OR INV1.Usage = 25
			)
			AND 
			(
				oinv.CreateTS >= FORMAT(DATEADD(MINUTE, @QtdeMinutos, GETDATE()), 'HHmm')
				OR oinv.UpdateTs >= FORMAT(DATEADD(MINUTE, @QtdeMinutos, GETDATE()), 'HHmm')
			)
		)
		)
	  AND OINV.CANCELED <> 'Y'
	ORDER BY 
		DocEntry ;
end;