

CREATE PROCEDURE SP_ZPN_COPIAALOCACAOCONTRATO
(
	@AbsId int,
	@AbsIdOrigem int
)
AS 
BEGIN

	--SP_ZPN_COPIAALOCACAOCONTRATO 1, 1

	SELECT 
		 ALOCI.U_CodAloc,
		 ALOCI.U_DescAloc,
		 ALOCI.U_PC		
	FROM	
		OOAT
		cross JOIN "@ZPN_ALOCONI" ALOCI
	WHERE	
		isnull(ALOCI.U_DescAloc,'') <> '' and 
		 ALOCI."Code" = @AbsIdOrigem and 
		(@AbsId = -1 OR OOAT.AbsId = @AbsId) and
		OOAT.AbsId not in (SELECT ALOCI2."Code" FROM "@ZPN_ALOCONI" ALOCI2 WHERE ALOCI2."Code" = OOAT.AbsId and ALOCI.U_CodAloc = ALOCI2.U_CodAloc)


	order by ALOCI.U_DescAloc, OOAT.AbsId;

end;