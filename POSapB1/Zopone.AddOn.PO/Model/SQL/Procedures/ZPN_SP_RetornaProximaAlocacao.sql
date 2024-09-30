


CREATE PROCEDURE ZPN_SP_RetornaProximaAlocacao
(
	@AbsId int, 
	@CodObra varchar(50)

)
as 
BEGIN

--DECLARE @AbsId int;
--DECLARE @CodObra varchar(50);

--set @AbsId = 375;
--SET @CODoBRA = 'C1.0636/24'

SELECT top 1 
	ALOCA.Code, ALOCA.U_Desc
FROM 
	[@ZPN_ALOCONI] CONTALOCA
	INNER JOIN "@ZPN_ALOCA" ALOCA ON ALOCA."Code" = CONTALOCA."U_CodAloc" AND CONTALOCA.U_PC = 'Y' and ALOCA."U_ItensPed" = 'Y'	
WHERE
	isnull(CONTALOCA.U_CodAloc,'') NOT IN 
	(
		SELECT 
			RDR1.U_Item
		FROM 
			RDR1 
			INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
		WHERE 
			ORDR."Canceled" <> 'Y' AND
			RDR1.Project = @CodObra and
			RDR1.U_Item = isnull(CONTALOCA.U_CodAloc,'')

	) 
	AND	CONTALOCA."Code" = @AbsId
ORDER BY
	ALOCA."U_Perc"
	

end;