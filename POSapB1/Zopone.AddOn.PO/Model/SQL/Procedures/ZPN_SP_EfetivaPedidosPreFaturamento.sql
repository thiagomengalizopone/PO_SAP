CREATE PROCEDURE ZPN_SP_EfetivaPedidosPreFaturamento
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @DataInicialInclusao datetime,
	 @DataFinalInclusao datetime,
	 @NumAtCard varchar(100),
	 @CardName varchar(250),
	 @Usuario int
)
AS
BEGIN


--ZPN_SP_ListaPedidosGerarPreFaturamento '2024-08-28', '2024-08-28', ''

--DECLARE @StatusFaturamento varchar(1);
--DECLARE @NumAtCard varchar(100);
--DECLARE @DataInicial datetime;
--DECLARE @DataFinal datetime;

--set @DataInicial = '2024-01-01';
--set @DataFinal = '2025-01-01';

--set @StatusFaturamento = 'A';

set @Usuario = isnull(@Usuario,-1);

SELECT 
	'N' "Selecionar",
	ORDR."DocEntry" "Pedido",
	ORDR."NumAtCard" "PO",
	ODRF.DocDate "DataT",
	ODRF.CreateDate "DataI",
	ODRF."CardCode",
	ODRF."CardName",
	OOAT.Remarks "Contrato",
	obra.Code "Obra",
	odrf.DocDueDate "Vencimento",
	DRF1."LineNum" "Linha",
	DRF1."U_Item" "Item",
	DRF1."U_Atividade" "Atividade",
	DRF1."U_itemDescription" "Descricao",
	DRF1."LineTotal" "Valor",
	DRF1.DocEntry "Esboco",
	0 "NF",
	DRF1."ItemCode",
	DRF1."Dscription", 
	
	ODRF.DocTotal "SaldoFaturado",
	0 "SaldoAberto",
	0 "TotalFaturar",
	DRF1.U_StatusFat as "Status",
	0 as "TotalDocumento",
	isnull(IMP.CofinsWTAmnt,0) COFINS,
    isnull(IMP.CSLLWTAmnt,0) CSLL,
    isnull(IMP.IRRFWTAmnt,0) IRRF,
	isnull(IMP.PisWTAmnt,0) PIS,
	isnull(IMP.InssWTAmnt,0) INSS,
	isnull(IMP.ISSWTAmnt,0) ISS,
	isnull(OBRA.U_Estado,'') + ' - ' + isnull(OBRA.U_CidadeDesc,'') "CidadeObra"
	
FROM
	ODRF 
	INNER JOIN DRF1 ON ODRF."DocEntry" = DRF1."DocEntry"
	INNER JOIN DRF21 ON DRF21.DocEntry = ODRF.DocEntry  
	LEFT JOIN  ORDR ON ORDR.DocEntry = DRF21.RefDocEntr AND DRF21.RefObjType = ORDR.ObjType
	LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.AbsEntry = ODRF.DocEntry AND IMP."TipoDocumento" = 'DRF'
	LEFT JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
	LEFT JOIN OOAT ON OOAT.AbsID = OBRA.U_CodContrato
	 
WHERE
	ODRF."DocStatus" = 'O' AND 
	(
		(isnull(ODRF."NumAtCard",'') = @NumAtCard and isnull(@NumAtCard,'') <> '')
		or 
		(isnull(@NumAtCard,'') = '')
	) 
	AND odrf.CreateDate between @DataInicialInclusao and @DataFinalInclusao
	AND DRF1."DocDate" between @DataInicial and @DataFinal 
	AND 
	(
		(ORDR."CardName" like '%' + @CardName + '%' or isnull(@CardName,'') = '')  
		OR 
		(ORDR.CardCode like '%' + @CardName + '%' or isnull(@CardName,'') = '')  
	)
	and 
	(
		ODRF.[UserSign] = @Usuario or @Usuario = -1
	)
ORDER BY
	DRF1."DocDate", ODRF."DocNum", DRF1."LineNum";


END ;


