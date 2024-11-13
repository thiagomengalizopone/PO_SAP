CREATE VIEW ZPN_VW_ImportacaoEtapaSAPTopEng
as


 SELECT 
	Etapa.ETA_001 "Code", 
	Etapa.ETA_002 U_Desc,
	case when Etapa.eta_004 = 3 then 'C' else 'O' end as "U_Classif",
	Etapa.ETA_006 "U_Tipo",
	CASE WHEN Etapa.ETA_007 = 1 THEN 'Y' ELSE 'N' END "U_ItensFat",
	Etapa.ETA_008 U_Perc,
	CASE WHEN Etapa.ETA_009 = 1 THEN 'Y' ELSE 'N' END "U_ItensRec",
	Etapa.ETA_010 "U_EtapaRec",
	etapa2.ETA_002 "U_EtapaRecD",
	CASE WHEN Etapa.ETA_011 = 1 THEN 'Y' ELSE 'N' END "U_ItensPed"
 FROM 
	SQL_TOPENG.TOPENG.DBO.Etapa 
	LEFT join SQL_TOPENG.TOPENG.DBO.Etapa etapa2 on etapa2.ETA_001 = Etapa.ETA_010
WHERE 
	Etapa.EMP_001 = 74  and 
	Etapa.ETA_001 not in (SELECT "Code" FROM "@ZPN_ALOCA" WHERE "Code" = Etapa.ETA_001);





