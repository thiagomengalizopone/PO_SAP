







	SELECT  
		DISTINCT 
			PREV.U_CodEve
	FROM
		"@ZPN_LOTEPAG" PREV
		INNER JOIN "@ZPN_LOTEPAGL" PREVL	ON PREVL."Code"		= PREV."Code"
		INNER JOIN OCRD						ON OCRD.CardCode	= PREVL.U_CardCode
		INNER JOIN "@ZPN_EVENTO" EVE		ON EVE."Code"		= PREVL.U_Evento
		inner JOIN OPRC						ON OPRC.PrcCode		= PREVL.U_PCG
		inner JOIN OPRC OBRA				ON OBRA.U_Obra      = OCRD.ProjectCod
		inner JOIN "@ZPN_OPRJ" OPRJ			ON OPRJ."Code"		= OCRD.ProjectCod




















