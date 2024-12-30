
CREATE view [vw_tmp_obrasimportar] as 
select	distinct 
	obra.referencia,
	ooat.AbsID,
	ooat.Descript
from 
	[LINKZCLOUD].[zsistema_producao].contrato 
	inner join [LINKZCLOUD].[zsistema_producao].dbo.obra on obra.contratoid = contrato.contratoid 
	inner join [LINKZCLOUD].[zsistema_producao].dbo.poitem on poitem.obraid = obra.obraid
	inner join [LINKZCLOUD].[zsistema_producao].dbo.cliente on cliente.clienteid = contrato.clienteid
	inner join CRD7 ON CRD7.TaxId0 = CLIENTE.DOCUMENTOPRINCIPAL  COLLATE SQL_Latin1_General_CP1_CI_AS
	INNER JOIN OOAT ON OOAT.Descript = CONTRATO.referencia collate SQL_Latin1_General_CP1_CI_AS
where
	(
		cliente.razaosocial like '%HUAWEI%' OR
		cliente.razaosocial like '%ERICSSON%' OR
		cliente.razaosocial like '%CLARO%' 

	)AND 
	poitem.datalancamento >= '2024-01-01'  