CREATE TABLE TB_LOGMSG
(
	"LogId" NUMERIC IDENTITY(1,1) PRIMARY KEY,
	"DocEntry" INT ,
	"ObjType" varchar(20),
	"AddOn" varchar(50),
	"Modulo" varchar(50),
	"TipoLog" varchar(10),
	"Mensagem" varchar(max),
	"Erro" varchar(max),
	"Usuario" varchar(50),
	"DataLog" DATETIME 
)