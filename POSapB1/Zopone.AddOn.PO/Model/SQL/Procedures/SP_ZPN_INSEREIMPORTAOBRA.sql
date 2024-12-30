create procedure SP_ZPN_INSEREIMPORTAOBRA(@PRJCODE VARCHAR(100))
AS 
	BEGIN



	INSERT INTO "@ZPN_OPRJ" 
	(
		[Code],
		[Name],
		[DocEntry],
		[Canceled],
		[Object],
		[LogInst],
		[UserSign],
		[Transfered],
		[CreateDate],
		[CreateTime],
		[UpdateDate],
		[UpdateTime],
		[DataSource],
		[U_ClassOb],
		[U_IdPci],
		[U_Longitude],
		[U_Bairro],
		[U_RelTerm],
		[U_RelIni],
		[U_Altitude],
		[U_CEP],
		[U_Complemento],
		[U_CodContrato],
		[U_Regional],
		[U_Latitude],
		[U_IdSite],
		[U_Numero],
		[U_PrevIni],
		[U_PrevTerm],
		[U_VisPCI],
		[U_Detent],
		[U_Equip],
		[U_Obs],
		[U_IdDetent],
		[U_Tipo],
		[U_Situacao],
		[U_Cidade],
		[U_Estado]
	)
	SELECT 
		OPRJ."PrjCode",
		OPRJ."PrjName", 
		(SELECT ISNULL(MAX("DocEntry"), 1) FROM "@ZPN_OPRJ") + ROW_NUMBER() OVER (ORDER BY OPRJ."PrjCode"),
		'N',
		'ZPN_OPRJ',
		0,
		1,
		'N',
		GETDATE(),
		FORMAT(GETDATE(), 'HHmm'),
		GETDATE(),
		FORMAT(GETDATE(), 'HHmm'),
		'O',
		CLASS."Code",
		OBRAPCI.obraid,
		OBRAPCI.longitude,
		OBRAPCI.bairro,
		OBRAPCI.realizadotermino,
		OBRAPCI.realizadoinicio,
		OBRAPCI.altitude, 
		OBRAPCI.cep,
		OBRAPCI.complemento,
		OOAT.AbsID,
		OPRC.PrcCode,
		OBRAPCI.latitude,
		OBRAPCI.idsite,
		OBRAPCI.numero,
		OBRAPCI.previsaoinicio,
		OBRAPCI.previsaotermino,
		CASE WHEN OBRAPCI.visualizarpci = 1 THEN 'Y' ELSE '' END,
		OBRAPCI.detentora,
		OBRAPCI.equipamento,
		OBRAPCI.historicoavaliacoes,
		OBRAPCI.iddetentora,
		OBRAPCI.tipo,
		OBRAPCI.situacao,
		OCNT.Code,
		OCNT.State
	FROM 
		[LINKZCLOUD].[zsistema_producao].[dbo].[obra] OBRAPCI
		INNER JOIN OPRJ ON OPRJ.PrjCode = OBRAPCI.referencia COLLATE SQL_Latin1_General_CP1_CI_AS
		INNER JOIN "@ZPN_CLASSOBF" CLASS ON OBRAPCI.obraclassificacaoid = CLASS.U_IdPCI
		inner join vw_tmp_obrasimportar obraimp on obraimp.referencia =  OBRAPCI.referencia
		INNER JOIN OOAT ON ooat.absid = obraimp.absid
		INNER JOIN OPRC ON OPRC.U_IdPCI = OBRAPCI.filialid AND OPRC.DimCOde = 3
		LEFT JOIN OCNT ON OCNT.[Name] = OBRAPCI.cidade COLLATE SQL_Latin1_General_CP1_CI_AS
	WHERE
		OPRJ.PRJcODE = @PRJCODE;
END;
