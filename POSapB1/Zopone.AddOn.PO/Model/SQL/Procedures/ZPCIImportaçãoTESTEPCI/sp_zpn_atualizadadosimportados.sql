-----------------------------------------------------------------------------------
-------------- PASSO 1 ------------------------------------------------------------
-----------------------------------------------------------------------------------
--ATUALIZA CLASSIFICAÇÃO DE OBRA


UPDATE "@ZPN_CLASSOB" 
SET
	U_IdPCI = OC.[obraclassificacaoid]
FROM 
	"@ZPN_CLASSOB" CLASSOB
	CROSS JOIN OBPL 
	INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[obraclassificacao] OC ON 
							OC.[codigo]	collate SQL_Latin1_General_CP1_CI_AS	= CLASSOB.U_Sigla and
								cast(oc.[empresaid] as varchar(50)) collate SQL_Latin1_General_CP1_CI_AS 	= obpl.U_IdPCI
WHERE 
	isnull(CLASSOB.U_IdPCI,'') = '' and 
	OBPL.U_EnviaPCI = 'Y';


-----------------------------------------------------------------------------------
-------------- PASSO 2 ------------------------------------------------------------
-----------------------------------------------------------------------------------

--corrige id da regional

UPDATE OOAT SET u_regional = oprc.prccode
	from
		ooat 
		inner join olct on ooat.u_regional  = olct.code 
		inner join oprc on oprc.U_Localiz = olct.code 


--atualiza id do contrato


UPDATE OOAT 
	SET U_IDPCI = CONTRATO.CONTRATOID
FROM
	OOAT
	INNER  JOIN OPRC ON OPRC.PrcCode = OOAT.U_Regional 
	INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].CONTRATO ON CONTRATO.referencia = OOAT.Descript  collate SQL_Latin1_General_CP1_CI_AS AND CONTRATO.FILIALID = OPRC.U_IdPCI
WHERE
	ISNULL(OOAT.U_IdPCI,'') = '';



-----------------------------------------------------------------------------------
-------------- PASSO 2 ------------------------------------------------------------
-----------------------------------------------------------------------------------

update "@ZPN_ALOCA"
set U_idpci = etapa.etapaid

FROM
	"@ZPN_ALOCA" ALOCA
	inner join obpl on aloca.U_bplid = obpl.bplid
	INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].etapa on etapa.nome = aloca.u_desc collate SQL_Latin1_General_CP1_CI_AS
															and etapa.empresaid = obpl.u_idpci collate SQL_Latin1_General_CP1_CI_AS
where 
	isnull(aloca.u_idpci,'') = ''

	


	



	









