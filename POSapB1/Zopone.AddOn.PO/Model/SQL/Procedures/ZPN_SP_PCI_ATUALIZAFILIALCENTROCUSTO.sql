

CREATE PROCEDURE ZPN_SP_PCI_ATUALIZAFILIALCENTROCUSTO
(
	@PrcCode varchar(50)
)
AS 
BEGIN




	
	INSERT INTO 
		[LINKZCLOUD].[zsistema_aceite].[dbo].[filial] 
		(
			[filialid]
			,[gestatus]
			,[gedataacao]
			,[gecontaidacao]
			,[empresaid]
			,[nome]
			,[sigla]
			,[codigo]
		)
		SELECT 
			NEWID(),
			1,
			GETDATE(),
			null,
			t2.U_IdPCI,
			T0.PrcName,
			NULL,
			T0.U_IdZSistemas
		FROM 
			OPRC T0  
			INNER JOIN ODIM T1 ON T0.[DimCode] = T1.[DimCode]
			INNER JOIN OBPL T2 ON T2.BPLId = CAST(T0.U_MM_Filial AS VARCHAR(10))
		WHERE
			(T0.PrcCode = @PrcCode OR ISNULL(@PrcCode,'') = '') AND
			T0.PrcName not in 
				(
					select 
						filial.[nome] collate SQL_Latin1_General_CP1_CI_AS
					from 
						[LINKZCLOUD].[zsistema_aceite].[dbo].[filial] filial 
					where 
						filial.nome collate SQL_Latin1_General_CP1_CI_AS = T0.PrcName and 
						cast(filial.empresaid as varchar(50)) collate SQL_Latin1_General_CP1_CI_AS = t2.U_IdPCI
				)  and
			T1.DimDesc = 'REGIONAL' AND
			ISNULL(T0.[U_IdPCI],'') = '' AND
			ISNULL(T0.[U_IdZSistemas] ,'') = '' ;



	update
		oprc
	set
		[U_IdPCI] = FILIAL.filialid, 
		[U_IdZSistemas] = FILIAL.codigo
	FROM 
		OPRC T0  
		INNER JOIN ODIM T1 ON T0.[DimCode] = T1.[DimCode]
		INNER JOIN OBPL T2 ON T2.BPLId = T0.U_MM_Filial
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[filial] FILIAL ON FILIAL.nome collate SQL_Latin1_General_CP1_CI_AS = T0.[PrcName] AND FILIAL.empresaid = t2.U_IdPCI	
	WHERE	
		T0.PrcCode = @PrcCode OR ISNULL(@PrcCode,'') = '' AND
		T1.DimDesc = 'REGIONAL' AND
		ISNULL(T0.[U_IdPCI],'') = '' AND
		ISNULL(T0.[U_IdZSistemas] ,'') = '' ;

END;
