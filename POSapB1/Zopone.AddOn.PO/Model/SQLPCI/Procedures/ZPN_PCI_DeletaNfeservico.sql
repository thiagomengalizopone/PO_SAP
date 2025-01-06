CREATE PROCEDURE [dbo].ZPN_PCI_DeletaNfeservico
(
    @nfeservicoid varchar(250)
)
AS
BEGIN
		

		delete from [nfeservico] where nfeservicoid = @nfeservicoid;
        delete from [nfeservicoparcela] where [nfeservicoid] = @nfeservicoid;


END;
