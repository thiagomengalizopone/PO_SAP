create PROCEDURE SP_ZPN_PESQUISAOBRA
(
    @CampoPesquisa VARCHAR(250)
)
AS 
BEGIN
    -- Trim whitespace from the input parameter
    SET @CampoPesquisa = TRIM(@CampoPesquisa);

    SELECT TOP 100
        OPRJ.PrjCode AS "Código Obra",
        OPRJ.PrjName AS "Obra",
        ZPN_PRJ.U_IdSite AS "Id Site",
        OOAT.Descript AS "Contrato",
        OBPL.BplName AS "Filial",
        OCRD.CardCode AS "Código Cliente",
        OCRD.CardName AS "Cliente",
        CLASS.Name AS "Classificação Obra",
        ZPN_PRJ.U_BplId AS "ID Filial",
        OBPL.BplName AS "Filial/Empresa",
        OOAT.Number AS "Número Contrato",
        OPRC_PCG.PrcCode AS "PCG",
        OPRC.PrcCode AS "Obra",
        ZPN_PRJ.U_Regional AS "Regional"
    FROM 
        OPRJ
        INNER JOIN "@ZPN_OPRJ" ZPN_PRJ ON OPRJ.PrjCode = ZPN_PRJ.Code
        INNER JOIN OBPL ON ZPN_PRJ.U_BPLId = OBPL.BplId
        INNER JOIN OOAT ON OOAT.Number = ZPN_PRJ.U_CodContrato
        INNER JOIN OCRD ON OCRD.CardCode = OOAT.BPCode
        LEFT JOIN "@ZPN_CLASSOB" CLASS ON CLASS.Code = ZPN_PRJ.U_ClassOb
        LEFT JOIN OPRC ON OPRC.U_Obra = ZPN_PRJ.Code
        LEFT JOIN OPRC OPRC_PCG ON OPRC_PCG.U_CardCode = OCRD.CardCode
    WHERE
        OPRJ.PrjCode LIKE '%' + @CampoPesquisa + '%' OR 
        ZPN_PRJ.U_IdSite LIKE '%' + @CampoPesquisa + '%' OR 
        OPRJ.PrjName LIKE '%' + @CampoPesquisa + '%' OR 
        OBPL.BplName LIKE '%' + @CampoPesquisa + '%' OR 
        OOAT.Descript LIKE '%' + @CampoPesquisa + '%' OR
        CLASS.Name LIKE '%' + @CampoPesquisa + '%' OR 
		OCRD.CardCode LIKE '%' + @CampoPesquisa + '%' OR 
        OCRD.CardName LIKE '%' + @CampoPesquisa + '%' 
    ORDER BY
        OPRJ.PrjName;
END
