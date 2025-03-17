

create procedure SP_ZPN_GeraDadosLCMDesconto
(
	@DocEntry int
)
as 
BEGIN



	SELECT	
		OINV.CreateDate,
		OINV.DocDate,
		OINV.DocDueDate,
		OINV.CardCode,
		OINV.CardName,
		OINV.Project,
		
			case 
				when 
					((inv1.Usage =25) AND isnull(oinv.U_ZP_ND,'') <> '') then oinv.U_ZP_ND
				else 
				CASE when 
					((oinv.Bplid = 12 or inv1.Usage =25) AND isnull(oinv.U_ZP_ND,'') =  '') then cast(oinv.serial as varchar(12))
				else 
					CAST(OINV.U_TX_NDfe AS VARCHAR(10)) 
				end
			END
			AS fatura,
		inv1.U_Candidato,
		'34001004500002' "ContaContabil",
		OINV."BplID" 

	FROM	
		OINV
		INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry
	WHERE
		OINV.DocEntry = @DocEntry;

END;

