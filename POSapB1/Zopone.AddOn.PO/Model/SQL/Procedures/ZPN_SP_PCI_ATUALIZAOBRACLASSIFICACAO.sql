CREATE PROCEDURE ZPN_SP_PCI_ATUALIZAOBRACLASSIFICACAO
(
	@CodeClassificacao varchar(30)
)
AS
BEGIN




INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].[obraclassificacao]
	(
		    [obraclassificacaoid]
           ,[gestatus]
           ,[gedataacao]
           ,[gecontaidacao]
           ,[empresaid]
           ,[codigo]
		   ,[nome]
	)
    
	SELECT 
		NEWID(),
		1,
		GETDATE(),
		NULL,
		obpl.U_IdPCI,
		CLASSOB.Code,
		CLASSOB.Name

	FROM 
		"@ZPN_CLASSOB" CLASSOB
		CROSS JOIN OBPL 
	WHERE
		(CLASSOB.Code = @CodeClassificacao OR ISNULL(@CodeClassificacao,'') = '') AND 
		OBPL.U_EnviaPCI = 'Y' AND 
		CLASSOB.Code NOT IN (
								SELECT 
									OC.[codigo] collate SQL_Latin1_General_CP1_CI_AS
								FROM
									[LINKZCLOUD].[zsistema_aceite].[dbo].[obraclassificacao] OC
								where 
									OC.[codigo]	collate SQL_Latin1_General_CP1_CI_AS	= CLASSOB.Code and
									cast(oc.[empresaid] as varchar(50)) collate SQL_Latin1_General_CP1_CI_AS 	= obpl.U_IdPCI
							);
		

	UPDATE "@ZPN_CLASSOB" 
	SET
		U_IdPCI = OC.[obraclassificacaoid]
	FROM 
		"@ZPN_CLASSOB" CLASSOB
		CROSS JOIN OBPL 
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[obraclassificacao] OC ON 
								OC.[codigo]	collate SQL_Latin1_General_CP1_CI_AS	= CLASSOB.Code and
									cast(oc.[empresaid] as varchar(50)) collate SQL_Latin1_General_CP1_CI_AS 	= obpl.U_IdPCI
	WHERE 
		(CLASSOB.Code = @CodeClassificacao OR ISNULL(@CodeClassificacao,'') = '') AND 
		OBPL.U_EnviaPCI = 'Y';


END;