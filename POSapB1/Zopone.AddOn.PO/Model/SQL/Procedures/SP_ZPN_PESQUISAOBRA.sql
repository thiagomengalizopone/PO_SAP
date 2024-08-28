CREATE PROCEDURE SP_ZPN_PESQUISAOBRA
(
	@CampoPesquisa varchar(250)
)
AS 

	set @campoPesquisa = TRIM(@campoPesquisa);

	SELECT TOP 100

		OPRJ.PrjCode	"Código Obra",
		OPRJ.PRjName	"Obra",
		ZPN_PRJ.U_IdSite	"Id Site",
		ooat.Descript	"Contrato",
		OBPL.BplName	"Filial",
		OCRD."CardCode" "Código Cliente",
		OCRD."CardName" "Cliente",
		CLASS.Name		"Classificação Obra",
		ZPN_PRJ.U_BplId	"ID Filial",
		OBPL.BplName	"Filial/Empresa",
		OOAT.[Number]   "Número Contrato",
		T0.[U_PCG] "PCG",
		OPRC."PrcCode" "Obra",
		T0.[U_Regional] "Regional"
	FROM 
		OPRJ 
		INNER JOIN "@ZPN_OPRJ" ZPN_PRJ	ON OPRJ.PrjCode     = ZPN_PRJ."Code"
		INNER JOIN OBPL					ON ZPN_PRJ.U_BPLId	= OBPL.BplId
		INNER JOIN OOAT					ON OOAT.Number		= ZPN_PRJ.U_CodContrato
		INNER JOIN OCRD					ON OCRD."CardCode"	=  ooat."BPCode"
		LEFT  JOIN "@ZPN_CLASSOB" CLASS	ON CLASS."Code"     = ZPN_PRJ.U_ClassOb
		LEFT JOIN  OPRC ON OPRC."U_Obra" = ZPN_PRJ."Code"
	WHERE
		OPRJ.PrjCode like '%' + @CampoPesquisa + '%' or 
		OPRJ.PRjName like '%' + @CampoPesquisa + '%' or 
		OBPL.BplName like '%' + @CampoPesquisa + '%' or 
		ooat.Descript like '%' + @CampoPesquisa + '%' or
		CLASS.Name like '%' + @CampoPesquisa + '%' 
	ORDER BY
		OPRJ.PRjName;


	