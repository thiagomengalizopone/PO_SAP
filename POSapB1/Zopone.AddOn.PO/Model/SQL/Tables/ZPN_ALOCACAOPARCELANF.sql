create TABLE ZPN_ALOCACAOPARCELANF
(
	DocEntry int,
	ObjType int,
	TipoDocumento varchar(1),
	DraftKey int,
	Parcela int,
	Percentural decimal(16,4),
	ValorParcela decimal(16,4),
	CodigoAlocacao varchar(20),
	DescricaoAlocacao varchar(250),
	IdPCI varchar(250)
)