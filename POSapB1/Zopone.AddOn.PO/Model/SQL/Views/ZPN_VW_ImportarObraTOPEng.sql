Create VIEW [dbo].[ZPN_VW_ImportarObraTOPEng]
as


select 
	OBR_002 "Code",
	isnull(OBR_029,OBR_002) "Name",
	OOAT.AbsID "U_CodContrato",
	OOAT.Descript "U_DescContrato",
	Obras.OBR_030 "U_IdSite",
	Obras.OBR_042 "U_NroRH",
	OCRd.CardCode "U_CardCode",
	OCRd.CardName "U_CardName",
	Obras.OBR_014 "U_Rua",
	Obras.OBR_015 "U_Numero",
	Obras.OBR_016 "U_Complemento",
	Obras.OBR_019 "U_CEP",
	'BR' "U_Pais",
	uf.UF_002 "U_Estado",
	ocnt.AbsId "U_Cidade",
	OCNT.Name "U_CidadeDesc",
	Obras.OBR_047 "U_Latitude",
	Obras.OBR_048 "U_Longitude",
	Obras.OBR_046 "U_Altitude",
	Obras.OBR_049 "U_Detent",
	Obras.OBR_050 "U_IdDetent",
	Obras.OBR_052 "U_Equip",
	Obras.OBR_011 "U_Situacao",
	case when Obras.OBR_003  is null then null else CONVERT(int, CONVERT(char(10), Obras.OBR_003, 112)) end    "U_PrevIni",
	case when Obras.OBR_004  is null then null else CONVERT(int, CONVERT(char(10), Obras.OBR_004, 112)) end    "U_PrevTerm",
	case when Obras.OBR_005  is null then null else CONVERT(int, CONVERT(char(10), Obras.OBR_005, 112)) end    "U_RelIni",
	case when Obras.OBR_006  is null then null else CONVERT(int, CONVERT(char(10), Obras.OBR_006, 112)) end    "U_RelTerm",
	Obras.OBR_051 "U_Tipo",
	CASE WHEN ISNULL(obras.OBR_023,0) = 0 THEN 'N' ELSE 'Y' END "U_VisPCI",
	oBRAS.OBR_074 "U_Medicao",
	oBRAS.OBR_027 "U_ValorPrev",
	OPRC.PrcCode "U_Regional",
	OPRC_COMPRA.PrcCode "U_RegionalCompra",
	CASE WHEN isnull("@ZPN_OPRJ"."Code",'') = '' then 'N' else 'Y' end "ObraAddOn",
	CASE WHEN isnull(OPRJ.PrjCode,'') = '' then 'N' else 'Y' end "ObraSAP",
	CASE WHEN isnull(OPRC_OBRA.PrcCode,'') = '' then 'N' else 'Y' end "CentroCustoObra",
	obpl.BPLId "U_BPLId",
	OBPL.BPLName "U_BPLName",
	TRIM(SUBSTRING(replace(replace(REPLACE(OBR_002, ' ', ''),'.',''), '/','') + '        ',0,8))   CenterCode

from 	
	SQL_TOPENG.TOPENG.DBO.Obras 
	INNER JOIN SQL_TOPENG.TOPENG.DBO.Contrato on Contrato.CTR_001 = Obras.CTR_001
	LEFT JOIN ooat ON OOAT.Descript = Contrato.CTR_002 COLLATE SQL_Latin1_General_CP850_CI_AS
	INNER JOIN SQL_TOPENG.TOPENG.DBO.Pessoa ON Pessoa.PES_001 = Obras.PES_001
	INNER JOIN OCRD ON OCRD."CardCode" =   'C' + right('000000' + CAST(PESSOA.PES_057 AS VARCHAR(10)), 6)
	LEFT JOIN SQL_TOPENG.[DBMULTSOFT].DBO.cidade ON CIDADE.CID_001 = obras.cid_001
	LEFT JOIN SQL_TOPENG.[DBMULTSOFT].DBO.uf     ON uf.uf_001      =   CIDADE.uf_001  
	LEFT JOIN OCNT ON OCNT."Name" = CIDADE.CID_002 collate  SQL_Latin1_General_CP850_CI_AS AND OCNT.State =  uf.uf_002 collate SQL_Latin1_General_CP850_CI_AS
	LEFT JOIN SQL_TOPENG.TOPENG.DBO.filial REGIONAL ON REGIONAL.FIL_001 = obras.fil_001
	LEFT JOIN OPRC ON OPRC.PrcName = REGIONAL.FIL_002  collate SQL_Latin1_General_CP850_CI_AS and OPRC.DimCode = 3 
	LEFT JOIN SQL_TOPENG.TOPENG.DBO.filial REGIONAL_COMPRA ON REGIONAL_COMPRA.FIL_001 = obras.OBR_045
	LEFT JOIN OPRC OPRC_COMPRA ON OPRC_COMPRA.PrcName = REGIONAL.FIL_002 collate SQL_Latin1_General_CP850_CI_AS and OPRC.DimCode = 3
	LEFT JOIN "@ZPN_OPRJ" ON "@ZPN_OPRJ".Code = obras.OBR_002 collate Latin1_General_CI_AI
	LEFT JOIN OPRJ ON OPRJ.PrjCode = obras.OBR_002 collate Latin1_General_CI_AI
	LEFT JOIN OPRC OPRC_OBRA ON OPRC_OBRA.PrcCode = TRIM(SUBSTRING(replace(replace(REPLACE(OBR_002, ' ', ''),'.',''), '/','') + '        ',0,8))  COLLATE SQL_Latin1_General_CP850_CI_AS
	inner join obpl on obpl.BPLId = 1
WHERE
	("@ZPN_OPRJ"."Code" is null  OR isnull("@ZPN_OPRJ".U_IdSite,'') = '' )and 
	

	(obras.OBR_006 is null or 
	obras.OBR_006 >= '2024-11-13')  and 
	Obras.OBR_002 in (
	(SELECT DISTINCT
		obras2.OBR_002
	FROM 
		SQL_TOPENG.TOPENG.DBO.PO 
		INNER JOIN SQL_TOPENG.TOPENG.DBO.PO_Itens ON PO_ITENS.PO_001 = PO.PO_001
		inner join SQL_TOPENG.TOPENG.DBO.Obras obras2 on obras2.OBR_001 = PO_ITENS.OBR_001
	WHERE 
		PO.PO_010 >= '2022-01-01')

		)

	
	

GO


