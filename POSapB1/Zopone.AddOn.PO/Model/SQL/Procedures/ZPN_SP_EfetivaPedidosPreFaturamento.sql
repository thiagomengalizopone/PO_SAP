CREATE PROCEDURE ZPN_SP_EfetivaPedidosPreFaturamento
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @DataInicialInclusao datetime,
	 @DataFinalInclusao datetime,
	 @NumAtCard varchar(100),
	 @CardName varchar(250),
	 @Usuario int,
	 @Item varchar(250) = '',
	 @Obra varchar(250) = ''
)
AS
BEGIN



set @Usuario = isnull(@Usuario,-1);

SELECT 
	ROW_NUMBER() OVER 
		(
			 ORDER BY
				DRF1."DocDate", 
				ODRF."DocNum", 
				DRF1."LineNum"
			) AS "LineId",
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
	case 
		when isnull(oitm.ItemName,'') <> '' then oitm.ItemName
		else oitm.FrgnName
	end "Dscription", 
	
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
	isnull(OBRA.U_Estado,'') + ' - ' + isnull(OBRA.U_CidadeDesc,'') "CidadeObra",
	OUSR.USER_CODE
FROM
	ODRF 
	INNER JOIN DRF1 ON ODRF."DocEntry" = DRF1."DocEntry"
	LEFT JOIN DRF21 ON DRF21.DocEntry = ODRF.DocEntry  
	LEFT JOIN  ORDR ON ORDR.DocEntry = DRF21.RefDocEntr AND DRF21.RefObjType = ORDR.ObjType
	LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.AbsEntry = ODRF.DocEntry AND IMP."TipoDocumento" = 'DRF'
	LEFT JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
	LEFT JOIN OOAT ON OOAT.AbsID = OBRA.U_CodContrato
	INNER JOIN OUSR ON ODRF.[UserSign] = OUSR.[USERID] 
	INNER JOIN OITM ON OITM.ItemCode = DRF1."ItemCode"
WHERE
	ODRF."DocStatus" = 'O' AND 
	 (
        (ISNULL(@NumAtCard, '') <> '' 
            AND EXISTS (
                SELECT 1
                FROM STRING_SPLIT(@NumAtCard, ',') AS nums
                WHERE trim(ODRF.NumAtCard) like '%' + trim(nums.value) + '%'
            )
        )
        OR (ISNULL(@NumAtCard, '') = '')
    )
	AND odrf.CreateDate between @DataInicialInclusao and @DataFinalInclusao
	AND DRF1."DocDate" between @DataInicial and @DataFinal 
	AND 
	(
		isnull(@CardName,'') = '' or 

		(
			ODRF."CardName" like '%' + @CardName + '%'
			OR 
			ODRF.CardCode like '%' + @CardName + '%' 
		)
	)
	and
	(
		DRF1."ItemCode" = @Item or isnull(@Item,'') = '' 
	)
	and
	(
		DRF1.Project = @Obra or isnull(@Obra,'') = '' 
	)
	and 
	(
		ODRF.[UserSign] = @Usuario or @Usuario = -1
	)
ORDER BY
	DRF1."DocDate", ODRF."DocNum", DRF1."LineNum";


END ;


