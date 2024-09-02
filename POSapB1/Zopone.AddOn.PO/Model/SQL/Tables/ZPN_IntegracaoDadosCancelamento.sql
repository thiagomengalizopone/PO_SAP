create TABLE ZPN_IntegracaoDadosCancelamento (
    ID INT IDENTITY(1,1), -- Coluna de identidade para controlar o loop
    U_IdPci varchar(100),
    DocDate DATE
);