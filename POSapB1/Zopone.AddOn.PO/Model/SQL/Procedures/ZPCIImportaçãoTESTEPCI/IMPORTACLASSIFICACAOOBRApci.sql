CREATE PROCEDURE SP_GRAVARLOG 
(
	@DocEntry NVARCHAR(50),
	@ObjType NVARCHAR(30),
	@Entidade NVARCHAR(50),
	@LogType NVARCHAR(20), 
	@Erro NVARCHAR(max), 
	@ExceptionStr NVARCHAR(max),
	@UserId NVARCHAR(30)
)
as 
BEGIN
	INSERT INTO TB_LOGMSG
		( DocEntry, ObjType, AddOn, Modulo, TipoLog, Mensagem, Erro, Usuario, DataLog)
    VALUES
		( @DocEntry, @ObjType, @Entidade, null, @LogType, @Erro, @ExceptionStr, @UserId, getdate())
	
END

