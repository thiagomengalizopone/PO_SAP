create procedure ZPN_SP_LOGIMPORTACAOPO
(
	@po_id int, 
	@MensagemLog varchar(5000)
)
as
begin
	
	INSERT INTO 
		ZPN_LOGIMPORTACAOPO
			(
				po_id, 
				DataLog, 
				MensagemLog
			)
	VALUES
		(
			@po_id, 
			GETDATE(),
			@MensagemLog
		);
	
end;