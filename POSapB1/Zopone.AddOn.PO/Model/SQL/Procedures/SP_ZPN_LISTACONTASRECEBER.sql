ALTER PROCEDURE SP_ZPN_LISTACONTASRECEBER
(
	@DataInicial date,
	@DataFinal date,
	@TipoData varchar(1) = 'V', 
	@CardCode varchar(100) = '',
	@CardName varchar(250) = '',
	@CodContrato varchar(10) = '',
	@CodObra varchar(20) = '', 
	@PO varchar(50) = '',
	@Fatura varchar(50) = ''
)

AS 

BEGIN

	set @DataInicial = isnull(@DataInicial, '2020-01-01');
	set @DataFinal = isnull(@DataFinal, '2050-01-01');
	set @TipoData = isnull(@TipoData, 'V');
	set @CardCode = isnull(@CardCode,'');
	set @CardName = isnull(@CardName,'') ;
	set @CodContrato = isnull(@CardName,'') ;
	set @CodObra = isnull(@CardName,'') ;
	set @PO = isnull(@CardName,'') ;
	set @Fatura = isnull(@CardName,'') ;


SELECT TOP 300
	'N' Seleciona,
	cast(OINV.U_TX_NDfe as varchar(20)) + '-' + cast(INV6.InstlmntId as varchar(10)) "Fatura",
	OINV."Project" "Obra",
	null "Candidato",
	OBRA.Name "Local",
	INV6.InstlmntId "Parcela",
	OINV."DocDate" Emissao,
	INV6.DueDate Vencimento,
	inv6.U_DataProgramacao Programado,
	'SERVIÇO' Tipo,
	null "Recebimento",
	OINV."DocEntry" "OF",
	case 
		when INV6."Status" = 'C' then 'RECEBIDO'
		ELSE 'A RECEBER'
	end "Situacao",
	OINV."CardCode" "Cliente",
	OINV."CardName" "RazaoSocial",
	OOAT.Descript "Contrato",
	TOTDOC.LineTotal * INV6.InstPrcnt/ 100 "ValorTitulo",
	INV6."PaidToDate" "ValorRecebido",
	0.00 "Desconto",
	0.00 "Outros",
	IMPOSTO.PISWTAmnt * INV6.InstPrcnt / 100 PIS,
	IMPOSTO.COFINSWTAmnt * INV6.InstPrcnt/ 100 COFINS,
	IMPOSTO.CSLLWTAmnt * INV6.InstPrcnt/ 100 CSLL,
	IMPOSTO.INSSWTAmnt * INV6.InstPrcnt/ 100 INSS,
	IMPOSTO.IRRFWTAmnt * INV6.InstPrcnt/ 100 IRRF,
	IMPOSTO.ISSWTAmnt * INV6.InstPrcnt / 100 ISS,
	TOTDOC.LineTotal - 
					(
						IMPOSTO.PISWTAmnt * INV6.InstPrcnt / 100 +
						IMPOSTO.COFINSWTAmnt * INV6.InstPrcnt/ 100 +
						IMPOSTO.CSLLWTAmnt * INV6.InstPrcnt/ 100 +
						IMPOSTO.INSSWTAmnt * INV6.InstPrcnt/ 100 +
						IMPOSTO.IRRFWTAmnt * INV6.InstPrcnt/ 100 +
						IMPOSTO.ISSWTAmnt * INV6.InstPrcnt / 100 
					) "ValorLiquido",
	ALOCA.U_Desc "EtapaRecebimento"
FROM
	OINV
	INNER JOIN INV6 ON INV6."DocEntry" = OINV."DocEntry"
	INNER JOIN VW_ZPN_TOTALDOCUMENTOFATURAMENTO TOTDOC ON TOTDOC."DocEntry" = OINV."DocEntry"
	INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA."Code" = OINV."Project"
	INNER JOIN OOAT ON OOAT.AbsId = OBRA.U_CodContrato
	INNER JOIN [ZPN_VW_DOCUMENTOSIMPOSTO] IMPOSTO ON IMPOSTO.AbsEntry = OINV."DocEntry"
	INNER JOIN "@ZPN_ALOCA" ALOCA ON ALOCA."Code" = INV6."U_ItemFat"
WHERE
	(OINV."CANCELED" <> 'Y' and OINV."CANCELED" <> 'C' and INV6.Status = 'O')   and
	(
		(@TipoData = 'E' and OINV.DocDate BETWEEN @DataInicial and @DataFinal) or 
		(@TipoData = 'V' and INV6.DueDate BETWEEN @DataInicial and @DataFinal) or 
		(@TipoData = 'P' and INV6.U_DataProgramacao BETWEEN @DataInicial and @DataFinal)  

	) and
	(@CardCode  = '' or OINV.CardCode = @CardCode) AND 
	(@CardName ='' or  OINV.CardName Like '%' + @CardName + '%') and
	(@CodContrato = '' or  OOAT.Descript Like '%' + @CodContrato + '%') and
	(@CodObra = '' or  OINV."Project" = @CodObra) AND 
	( @PO = '' or OINV.NumAtCard = @PO ) AND 
	(@Fatura = '' or cast(OINV.U_TX_NDfe as varchar(20)) + '-' + cast(INV6.InstlmntId as varchar(10))	 = @Fatura );
	
	

	
END;