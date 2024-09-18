


	update
		oprc
	set
		[U_IdPCI] = FILIAL.filialid, 
		[U_IdZSistemas] = FILIAL.codigo
	FROM 
		OPRC T0  
		INNER JOIN ODIM T1 ON T0.[DimCode] = T1.[DimCode]
		INNER JOIN OBPL T2 ON T2.BPLId = T0.U_MM_Filial
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[filial] FILIAL ON FILIAL.SIGLA collate SQL_Latin1_General_CP1_CI_AS = T0.[PrcName] AND FILIAL.empresaid = t2.U_IdPCI	
	WHERE	
		T0.DimCode = 3 and
		ISNULL(T0.[U_IdPCI],'') = '' AND
		ISNULL(T0.[U_IdZSistemas] ,'') = '' ;