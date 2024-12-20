ALTER PROCEDURE SP_ZPN_CriaObservacoesFaturamentoEsboco
(
    @DocEntry INT
/*	, 
    @PO VARCHAR(50), 
    @Obra VARCHAR(50), 
    @CodPrestServicos VARCHAR(50), 
    @Site VARCHAR(150), 
    @Irrf DECIMAL(16, 2), 
    @Pis DECIMAL(16, 2), 
    @CSLL DECIMAL(16, 2), 
    @PisCofinsCSLL DECIMAL(16, 2), 
    @Vencto DATE, 
    @BaseCalculoRetencao DECIMAL(16, 2), 
    @RetencaoPrevSocial DECIMAL(16, 2), 
    @LiquidoReceber DECIMAL(16, 2)
*/)
AS 
BEGIN
    
	UPDATE DRF6
	SET DRF6.U_Project = DRF1.Project
	FROM
		DRF6
		INNER JOIN DRF1 ON DRF1.DocEntry = DRF6.DocEntry
	WHERE
		DRF1.DocEntry = @DocEntry;


	DECLARE @MENSAGEM VARCHAR(MAX);

	SET @MENSAGEM = '';

	SELECT @MENSAGEM = 
	'OBRA: ' + isnull(DRF1.Project,'') + CHAR(13)+CHAR(10) +
	'PEDIDO DE COMPRA: ' + ISNULL(ODRF.NumAtCard,'') + CHAR(13)+CHAR(10) +
	+ CHAR(13)+CHAR(10) +
	'CÓD. DA PREST. DE SERVS. ' + isnull(DRF1.ItemCode, '') + CHAR(13)+CHAR(10) +
	+ CHAR(13)+CHAR(10) +
	'SITE ' + isnull(OBRA.U_IdSite,'') + ' ' + isnull(OBRA.U_CidadeDesc,'') + ' - ' + isnull(Obra.U_Estado,'') + CHAR(13)+CHAR(10) +
	+ CHAR(13)+CHAR(10) +
	+ CHAR(13)+CHAR(10) +
	' IRRF ' + FORMAT(isnull(IMP.IRRF,0), 'C', 'pt-BR') + CHAR(13)+CHAR(10) +
	' PIS ' + FORMAT(isnull(IMP.PIS,0), 'C', 'pt-BR') + CHAR(13)+CHAR(10) +
	' CSLL ' + FORMAT(isnull(IMP.CSLL,0), 'C', 'pt-BR') + CHAR(13)+CHAR(10) +
	' COFINS ' + FORMAT(isnull(IMP.COFINS,0), 'C', 'pt-BR') + CHAR(13)+CHAR(10) +
	' ISS ' + FORMAT(isnull(IMP.ISS,0), 'C', 'pt-BR') + CHAR(13)+CHAR(10) +
	+ CHAR(13)+CHAR(10) +
	'VENCIMENTO: ' + FORMAT(ODRF.DocDueDate, 'dd/MM/yyyy') + CHAR(13)+CHAR(10) 

		
	FROM 
		DRF1 
		INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
		left join ZPN_VW_DOCUMENTOSIMPOSTO IMP on imp.TipoDocumento = 'DRF' AND IMP.AbsEntry = ODRF.DocEntry
	WHERE
		DRF1.DocEntry = @DocEntry;

	UPDATE ODRF SET Header = @MENSAGEM WHERE DocEntry = @DocEntry and trim(cast(isnull(header,'') as varchar(max))) = '';

	

	

END;

