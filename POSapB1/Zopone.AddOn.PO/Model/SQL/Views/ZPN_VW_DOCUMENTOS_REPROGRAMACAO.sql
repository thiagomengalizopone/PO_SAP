create VIEW ZPN_VW_DOCUMENTOS_REPROGRAMACAO AS

SELECT DISTINCT
		ROW_NUMBER() OVER (ORDER BY T1.DocEntry)                       as 'Sequencia',
		'N'                                                            as 'Selecionar',
		T0.CardCode			                                           as 'Cliente',
		T3.TaxId0			                                           as 'CNPJ',
		T0.CardName			                                           as 'RazaoSocial',
		T0.DocNum			                                           as 'Documento',
		T0.Serial			                                           as 'Fatura',
		T0.Project			                                           as 'Obra',
		T0.Project			                                           as 'Local',
		T0.U_MM_PCG		                                               as 'PO',
		CAST(T1.InstlmntID  as NVARCHAR(10))                           as 'Parcela',
		T0.DocDate			                                           as 'Emissao',
		T1.DueDate			                                           as 'Vencimento',
		T1.DueDate		                                               as 'Programacao',
		T1.U_InformacaoCobranca                                        as 'InfoPrg',	
		T1.Instotal - T1.PaidToDate                                    as 'ValorTitulo',
		T1.PaidToDate		                                           as 'ValorRecebido',

		ISNULL((SELECT SUM(T99.WTAmnt) From INV5 T99
		INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
		INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId 
		WHERE T97.WTType = 'PIS' AND T99.AbsEntry = T0.DocEntry),0)    as 'PIS',
		
		ISNULL((SELECT SUM(T99.WTAmnt) From INV5 T99
		INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
		INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId 
		WHERE T97.WTType = 'COFINS' AND T99.AbsEntry = T0.DocEntry),0) as 'COFINS',

		ISNULL((SELECT SUM(T99.WTAmnt) From INV5 T99
		INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
		INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId 
		WHERE T97.WTType = 'CSLL' AND T99.AbsEntry = T0.DocEntry),0)   as 'CSLL',

		0.00                                                           as 'INSS',

		ISNULL((SELECT SUM(T99.WTAmnt) From INV5 T99
		INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
		INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId 
		WHERE T97.WTType = 'IRRF' AND T99.AbsEntry = T0.DocEntry),0)   as 'IRRF',
			
		0.00                                                           as 'ISS',
		0.00                                                           as 'Desconto',
		0.00                                                           as 'Outros',

		T1.Instotal - T1.PaidToDate                                    as 'ValorLiquido'

	FROM OINV T0 
	INNER JOIN INV6 T1 on T0.DocEntry = T1.DocEntry
	INNER JOIN OBPL T2 on T2.BPLId = T0.BPLId
	INNER JOIN INV12 T3 on T0.DocEntry = T3.DocEntry

WHERE T0.Canceled = 'N' 
      AND (T1.Instotal - T1.PaidToDate) > 0
	  AND T0.Model = 39
