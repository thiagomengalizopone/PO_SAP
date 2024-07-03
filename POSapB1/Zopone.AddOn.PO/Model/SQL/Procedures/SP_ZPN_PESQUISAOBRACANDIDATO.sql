
create PROCEDURE SP_ZPN_PESQUISAOBRACANDIDATO
(
	 @CampoPesquisa varchar(250),
	 @CodigoObra varchar(30)
)
AS 

	set @campoPesquisa = TRIM(@campoPesquisa);

	SELECT TOP 100
		CAND."U_Identif"	"Candidato",
		OPRJ.PrjCode		"Código Obra",
		OPRJ.PRjName		"Obra",
		ZPN_PRJ.U_IdSite	"Id Site",
		ooat.Descript		"Contrato",
		OBPL.BplName		"Filial",
		OCRD."CardCode"		"Código Cliente",
		OCRD."CardName"		"Cliente",
		CLASS.Name			"Classificação Obra"
	FROM 
		OPRJ 
		INNER JOIN "@ZPN_OPRJ" ZPN_PRJ	ON OPRJ.PrjCode     = ZPN_PRJ."Code"
		INNER JOIN OBPL					 ON ZPN_PRJ.U_BPLId		= OBPL.BplId
		INNER JOIN OOAT					 ON OOAT.Number		= ZPN_PRJ.U_CodContrato
		INNER JOIN OCRD					 ON OCRD."CardCode"	=  ooat."BPCode"
		LEFT  JOIN "@ZPN_CLASSOB" CLASS	 ON CLASS."Code"     = ZPN_PRJ.U_ClassOb
		INNER JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code = ZPN_PRJ.Code
	WHERE
		CAND."U_Identif" like '%' + @CampoPesquisa + '%' or 
		OPRJ.PrjCode = @CodigoObra and
		OBPL.BplName like '%' + @CampoPesquisa + '%' or 
		ooat.Descript like '%' + @CampoPesquisa + '%' or
		CLASS.Name like '%' + @CampoPesquisa + '%' 
	ORDER BY
		OPRJ.PRjName;

	