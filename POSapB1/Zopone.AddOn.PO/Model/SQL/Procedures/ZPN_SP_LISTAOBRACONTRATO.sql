CREATE PROCEDURE ZPN_SP_LISTAOBRACONTRATO
(
	@CodContrato varchar(50)
)
AS

SELECT
	ZPN_OPRJ."Code" "Código Obra",
	ZPN_OPRJ.Name "Descrição Obra",
	ZPN_OPRJ.U_BPLId "Id Filial",
	OBPL."BplName" "Descrição Filial" 
FROM
	"@ZPN_OPRJ" ZPN_OPRJ
	INNER JOIN OBPL ON OBPL."BplId" = ZPN_OPRJ.U_BPLId
WHERE 
	U_CodContrato = @CodContrato