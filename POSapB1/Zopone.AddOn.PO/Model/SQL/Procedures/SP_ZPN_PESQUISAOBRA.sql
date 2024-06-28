create PROCEDURE SP_ZPN_PESQUISAOBRA
(
	@CampoPesquisa varchar(250)
)
AS 

	set @campoPesquisa = TRIM(@campoPesquisa);

	SELECT TOP 100

		OPRJ.PrjCode	"Código Obra",
		OPRJ.PRjName	"Obra",
		OPRJ.U_IdSite	"Id Site",
		ooat.Descript	"Contrato",
		OBPL.BplName	"Filial",
		OCRD."CardCode" "Código Cliente",
		OCRD."CardName" "Cliente",
		CLASS.Name		"Classificação Obra"
	FROM 
		OPRJ 
		INNER JOIN OBPL					ON OPRJ.U_BPLId		= OBPL.BplId
		INNER JOIN OOAT					ON OOAT.Number		= OPRJ.U_CodContrato
		INNER JOIN OCRD					ON OCRD."CardCode"	=  ooat."BPCode"
		LEFT  JOIN "@ZPN_CLASSOB" CLASS	ON CLASS."Code"     = OPRJ.U_ClassOb
	WHERE
		OPRJ.PrjCode like '%' + @CampoPesquisa + '%' or 
		OPRJ.PRjName like '%' + @CampoPesquisa + '%' or 
		OBPL.BplName like '%' + @CampoPesquisa + '%' or 
		ooat.Descript like '%' + @CampoPesquisa + '%' or
		CLASS.Name like '%' + @CampoPesquisa + '%' 
	ORDER BY
		OPRJ.PRjName;

	