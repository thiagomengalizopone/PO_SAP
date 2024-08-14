alter PROCEDURE ZPN_SP_PCI_ATUALIZAOBRA
(
	@CodeObra varchar(30)
)
AS
BEGIN



INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].[obra]
           ([obraid]
           ,[gestatus]
           ,[gedataacao]
           ,[gecontaidacao]
           ,[obraclassificacaoid]
           ,[referencia]
           ,[longitude]
           ,[bairro]
           ,[realizadotermino]
           ,[realizadoinicio]
           ,[altitude]
           ,[cep]
           ,[complemento]
           ,[contratoid]
           ,[datacadastro]
           ,[endereco]
           ,[filialid]
           ,[latitude]
           ,[localizacao]
           ,[numero]
           ,[previsaoinicio]
           ,[previsaotermino]
           ,[visualizarpci]
           ,[detentora]
           ,[equipamento]
           ,[historicoavaliacoes]
           ,[iddetentora]
           ,[idsite]
           ,[tipo]
           ,[situacao]
           ,[cidade]
           ,[estado]
           ,[dataatualizacao]
           ,[situacaopci]
           ,[observacaomontagemform]
           ,[gedarquivoid]
           ,[gedpastaid])

SELECT
	NEWID(),
	1,
	GETDATE(),
	NULL,
	CLASS.U_IdPCI,
	OBRA.Code,
	OBRA.U_Longitude,
	OBRA.U_Bairro,
	OBRA.U_RelTerm,
	OBRA.U_RelIni,
	OBRA.U_Altitude, 
	OBRA.U_CEP,
	OBRA.U_Complemento,
	OOAT.U_IdPCI,
	GETDATE(),
	ISNULL(OBRA.U_TipoLog,'') + ' ' + ISNULL(OBRA.U_Rua,''),
	OPRC.U_IdPCI,
	OBRA.U_Latitude,
	OBRA.U_IdSite,
	OBRA.U_Numero,
	OBRA.U_PrevIni,
	OBRA.U_PrevTerm,
	CASE WHEN ISNULL(OBRA.U_VisPCI,'') = 'Y' THEN 1 ELSE 0 END,
	OBRA.U_Detent,
	obra.U_Equip,
	obra.U_Obs,
	obra.U_IdDetent,
	obra.U_IdSite,
	obra.U_Tipo,
	OBRA.U_Situacao,
	OCNT.Name,	
	OCNT.State,
	OBRA.UpdateDate,
	0, --verificar comn alessandro
	null, 
	null,
	NULL
FROM	 
	"@ZPN_OPRJ" OBRA
	INNER JOIN OOAT ON OOAT.AbsID = OBRA.U_CodContrato
	INNER JOIN "@ZPN_CLASSOB" CLASS ON CLASS.Code = OBRA.U_ClassOb
	INNER JOIN OPRC ON OPRC.PrcCode = OBRA.U_Regional 
	LEFT  JOIN OCNT ON OCNT.[Code]  = OBRA.U_Cidade
WHERE
	(OBRA.Code = @CodeObra OR ISNULL(@CodeObra,'') = '' ) AND
	OBRA.Code 
		NOT IN 
			(
				SELECT 
					OBRAPCI.referencia COLLATE SQL_Latin1_General_CP1_CI_AS
				FROM 
					[LINKZCLOUD].[zsistema_aceite].[dbo].[OBRA] OBRAPCI
				WHERE
					OBRAPCI.referencia COLLATE SQL_Latin1_General_CP1_CI_AS = OBRA.Code 
			);

	
UPDATE
	"@ZPN_OPRJ"
	SET 
		U_IdPci = OBRAPCI.obraid
FROM	 
	"@ZPN_OPRJ" OBRA
	INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[OBRA] OBRAPCI ON 
		OBRAPCI.referencia COLLATE SQL_Latin1_General_CP1_CI_AS = OBRA.Code 
where 
	(OBRA.Code = @CodeObra OR ISNULL(@CodeObra,'') = '' ) AND
	isnull(OBRA.U_IdPci,'') = '';





UPDATE [LINKZCLOUD].[zsistema_aceite].[dbo].[obra] 
SET
    [gedataacao] = GETDATE(),
    [gecontaidacao] = NULL,
    [obraclassificacaoid] = CLASS.U_IdPCI,
    [referencia] = OBRA.Code,
    [longitude] = OBRA.U_Longitude,
    [bairro] = OBRA.U_Bairro,
    [realizadotermino] = OBRA.U_RelTerm,
    [realizadoinicio] = OBRA.U_RelIni,
    [altitude] = OBRA.U_Altitude,
    [cep] = OBRA.U_CEP,
    [complemento] = OBRA.U_Complemento,
    [contratoid] = OOAT.U_IdPCI,
    [endereco] = ISNULL(OBRA.U_TipoLog,'') + ' ' + ISNULL(OBRA.U_Rua,''),
    [filialid] = OPRC.U_IdPCI,
    [latitude] = OBRA.U_Latitude,
    [localizacao] = OBRA.U_IdSite,
    [numero] = OBRA.U_Numero,
    [previsaoinicio] = OBRA.U_PrevIni,
    [previsaotermino] = OBRA.U_PrevTerm,
    [visualizarpci] = CASE WHEN ISNULL(OBRA.U_VisPCI,'') = 'Y' THEN 1 ELSE 0 END,
    [detentora] = OBRA.U_Detent,
    [equipamento] = OBRA.U_Equip,
    [historicoavaliacoes] = OBRA.U_Obs,
    [iddetentora] = OBRA.U_IdDetent,
    [idsite] = OBRA.U_IdSite,
    [tipo] = OBRA.U_Tipo,
    [situacao] = OBRA.U_Situacao,
    [cidade] = OCNT.Name,
    [estado] = OCNT.State,
    [dataatualizacao] = OBRA.UpdateDate,
    [situacaopci] = 0, --verificar com Alessandro
    [observacaomontagemform] = NULL,
    [gedarquivoid] = NULL,
    [gedpastaid] = NULL
FROM
    "@ZPN_OPRJ" OBRA
    INNER JOIN OOAT ON OOAT.AbsID = OBRA.U_CodContrato
    INNER JOIN "@ZPN_CLASSOB" CLASS ON CLASS.Code = OBRA.U_ClassOb
    INNER JOIN OPRC ON OPRC.PrcCode = OBRA.U_Regional 
    LEFT JOIN OCNT ON OCNT.[Code] = OBRA.U_Cidade
	INNER JOIN  [LINKZCLOUD].[zsistema_aceite].[dbo].[obra] OBRAPCI ON OBRAPCI.[obraid] = OBRA.U_IdPci
WHERE
    (OBRA.Code = @CodeObra) ;
    



END;


