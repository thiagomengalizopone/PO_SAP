INSERT INTO [@ZPN_ALOCA]
           ([Code]
           ,[Name]
           ,[DocEntry]
           ,[Canceled]
           ,[Object]
           ,[LogInst]
           ,[UserSign]
           ,[Transfered]
           ,[CreateDate]
           ,[CreateTime]
           ,[UpdateDate]
           ,[UpdateTime]
           ,[DataSource]
           ,[U_Classif]
           ,[U_Desc]
           ,[U_Tipo]
           ,[U_ItensRec]
           ,[U_ItensPed]
           ,[U_ItensFat]
           ,[U_ExporEtap]
           ,[U_Perc]
           ,[U_BplID]
           ,[U_IdPCI])


SELECT 
    ETP.[codigo] AS Code,
	ETP.[nome] AS U_Desc,
    (SELECT ISNULL(MAX(DocEntry),0)+1 FROM [@ZPN_ALOCA]) + ROW_NUMBER() OVER(order by ETP.[codigo]), 
	'N',
	'ZPN_ALOCA',
	0,
	1,
	'N',
	GETDATE(),
	FORMAT(GETDATE(), 'HHmm'),
	GETDATE(),
	FORMAT(GETDATE(), 'HHmm'),
	'O',
	'O',
	ETP.[nome] AS U_Desc,
	'A',
    CASE WHEN ETP.[itemrecebimento] = 1 THEN 'Y' ELSE 'N' END AS U_ItensRec,
	    CASE WHEN ETP.[itempedido] = 1 THEN 'Y' ELSE 'N' END AS U_ItensPed,
	CASE WHEN ETP.[itemfaturamento] = 1 THEN 'Y' ELSE 'N' END AS U_ItensFat,
    'Y',
	ETP.[percentualfaturamento] AS U_Perc,
    OBPL.BPLId AS U_BplId,
	etp.etapaid
FROM 
    [LINKZCLOUD].[zsistema_producao].[dbo].[etapa] ETP
	INNER JOIN OBPL ON OBPL.U_IdPci = CAST(ETP.[empresaid] AS VARCHAR(100)) COLLATE SQL_Latin1_General_CP1_CI_AS
WHERE 
    (ETP.[nome] COLLATE SQL_Latin1_General_CP1_CI_AS NOT IN
    (
        SELECT 
            [U_Desc] COLLATE SQL_Latin1_General_CP1_CI_AS
        FROM 
            "@ZPN_ALOCA" ALOCA
        WHERE 
            ALOCA.[Code] = CAST(ETP.[codigo] AS VARCHAR(100)) COLLATE SQL_Latin1_General_CP1_CI_AS
            AND CAST(ALOCA.[U_IdPCI] AS VARCHAR(50)) COLLATE SQL_Latin1_General_CP1_CI_AS = OBPL.U_IdPci COLLATE SQL_Latin1_General_CP1_CI_AS
    )) 
    AND OBPL.U_EnviaPCI = 'Y'
    AND ISNULL(OBPL.U_IdPci, '') <> ''
ORDER BY  ETP.[codigo];
