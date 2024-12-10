
CREATE PROCEDURE SP_ZPN_CriaObservacoesFaturamento
(
    @DocEntry INT, 
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
)
AS 
BEGIN
    SELECT 1;
END;
