create PROCEDURE [dbo].ZPN_PCI_CancelaNFContasReceber
(
    @nfeservicoid varchar(250),
    @contareceberid varchar(250),
	@datacancelamento datetime
)
AS
BEGIN

	update nfeservico set situacao = 5, gedataacao = GETDATE() where nfeservicoid = @nfeservicoid;

	update contareceber set cancelamento = @datacancelamento, gedataacao = GETDATE() where contareceberid = @contareceberid;

END;