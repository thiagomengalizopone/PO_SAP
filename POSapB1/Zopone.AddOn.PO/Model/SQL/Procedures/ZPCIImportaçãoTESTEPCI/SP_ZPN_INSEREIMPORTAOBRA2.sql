


create procedure [SP_ZPN_INSEREIMPORTAOBRA2](@PRJCODE VARCHAR(100))
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
		[U_CodContrato],
		[U_BplID],
		[U_Regional],
		U_IdPCI
	)
	SELECT top 50
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

		
		OOAT.AbsID,
		1,
		oprc.prccode,
		obra.obraid
	FROM 
		OPRJ 
		inner join vw_tmp_obrasimportar obraimp on obraimp.referencia =  oprj."PrjCode" collate SQL_Latin1_General_CP850_CI_AS
		INNER JOIN OOAT ON ooat.absid = obraimp.absid
		inner join [LINKZCLOUD].[zsistema_producao].dbo.obra obra on obra.referencia = oprj.prjcode COLLATE SQL_Latin1_General_CP850_CI_AS
		inner join [LINKZCLOUD].[zsistema_producao].dbo.filial filial on filial.filialid = obra.filialid
		inner join oprc on oprc.prcname = filial.sigla COLLATE SQL_Latin1_General_CP1_CI_AS
		
		
	WHERE
		OPRJ.PrjCode NOT IN (SELECT "Code" FROM "@ZPN_OPRJ" WHERE "Code" = OPRJ.PrjCode) AND 
		(OPRJ.PRJcODE = @PRJCODE or isnull(@PRJCODE,'') = '' );
END;
