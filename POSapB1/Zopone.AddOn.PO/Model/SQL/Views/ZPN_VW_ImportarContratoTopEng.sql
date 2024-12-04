

CREATE VIEW ZPN_VW_ImportarContratoTopEng
as



 SELECT 
	Contrato.CTR_002 Description,
	Contrato.CTR_002 Remarks,
	
	isnull(Contrato.CTR_003,'2024-01-01') "StartDate",
	ISNULL(Contrato.CTR_004,'2034-11-12') "EndDate",
	OLCT."Code" "U_Regional",
	'asApproved' "Status",
	'amMonetary' AgreementMethod,
	OCRD."CardCode" "BPCode",
	'atGeneral' "AgreementType" ,
	contrato.CTR_018 "U_CodigoRH"
	

 FROM 
	SQL_TOPENG.TOPENG.DBO.Contrato 
	inner join SQL_TOPENG.TOPENG.DBO.filial on filial.fil_001  = contrato.FIL_001
	INNER JOIN OLCT ON OLCT."Location" = Filial.FIL_002 collate SQL_Latin1_General_CP850_CI_AS
	INNER JOIN SQL_TOPENG.TOPENG.DBO.PESSOA on PESSOA.PES_001 = CONTRATO.PES_001
	INNER JOIN OCRD ON OCRD."CardCode" =   'C' + right('000000' + CAST(PESSOA.PES_057 AS VARCHAR(10)), 6)
WHERE 
	(Contrato.CTR_004 is null or Contrato.CTR_004>= '2024-01-01')  and 
	Contrato.EMP_001 = 74 
	AND 
		Contrato.CTR_002 NOT IN 
			(
				SELECT Descript collate SQL_Latin1_General_CP850_CI_AS FROM OOAT WHERE OOAT.Descript = Contrato.CTR_002 collate SQL_Latin1_General_CP850_CI_AS
			);	




