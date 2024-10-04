Create procedure ZPN_SP_LOGIMPORTACAOPO
(
	@po_id numeric, 
	@MensagemLog varchar(5000),
	@Empresa varchar(100)
)
as
begin
	
	INSERT INTO 
		ZPN_LOGIMPORTACAOPO
			(
				po_id, 
				DataLog, 
				Empresa,
				MensagemLog
			)
	VALUES
		(
			@po_id, 
			GETDATE(),
			@Empresa,
			@MensagemLog
		);
	
end;
