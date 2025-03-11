Create PROCEDURE [dbo].[SP_ZPN_RetornaDadosLinhaPO]
(
	@DocEntry int 
)
AS 
BEGIN
	
	SELECT 
		VisOrder,
		LineNum,
		RDR1.Project,
		U_Candidato,
		ORDR.CardCode,
		ORDR.CardName,
		U_Item,
		U_ItemFat,
		U_DescItemFat,
		U_Parcela,
		LineTotal "U_Valor",
		RDR1.U_Tipo,
		U_DataFat,
		U_NroNF,
		U_DataSol,
		RDR1.Text "U_Obs",
		U_Bloqueado,
		U_itemDescription,
		U_manSiteInfo,
		OOAT.AbsId  "AgrNo" ,
		ooat.Descript U_DescCont,
		RDR1.OcrCode "PCG",
		RDR1.OcrCode2 "Obra",
		RDR1.OcrCode3 Regional




	FROM 
		RDR1 
		INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
		LEFT JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = RDR1.Project 
		LEFT JOIN OOAT on OOAT.AbsId = OBRA.U_CodContrato
	WHERE 
		RDR1.DocEntry = @DocEntry
	order by
		LineNum;
	
END;