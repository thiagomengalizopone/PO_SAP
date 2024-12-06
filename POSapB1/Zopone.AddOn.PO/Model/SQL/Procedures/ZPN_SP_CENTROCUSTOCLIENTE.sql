
create PROCEDURE ZPN_SP_CENTROCUSTOCLIENTE
(
	@CardCode varchar(100)
)
AS 



SELECT 
	OCRD.CardCode, 
	trim(substring(OCRD.CardName + '                             ', 0, 30))"PrcName" ,
	isnull(OPRC.PrcCode,'')								"PrcCode"
FROM 
	OCRD 
	LEFT JOIN SQL_TOPENG.TOPENG.DBO.PESSOA ON PESSOA.pes_057 =  cast(REPLACE(OCRD.CardCode, 'C', '') as int)
	LEFT JOIN SQL_TOPENG.TOPENG.DBO.PCG    ON PCG.PCG_001    = PESSOA.PCG_001
	LEFT JOIN OPRC ON OPRC.PrcCode = cast(PCG.PCG_001 as  varchar(100)) and OPRC.U_CardCode = OCRD.CardCode
WHERE 
	(isnull(@CardCode,'') = '' Or OCRD.CardCode = @CardCode) and 
	OCRD.CardType = 'C' AND 
	OCRD."CardCode" not IN
		(SELECT 
			U_CardCode 
		FROM 
			OPRC 
		WHERE 
			U_CardCode = OCRD."CardCode")