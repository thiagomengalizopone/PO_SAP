USE [msdb]
GO

DECLARE @DATABASENAME VARCHAR(100) = 'SBO_ZOPONE_6Z_HOMOLOG2';

DECLARE @job_name_CREATE VARCHAR(500) =  N'ZPN_SP_PCI_ATUALIZAPO ' + @DATABASENAME;
DECLARE @schedule_name_CREATE varchar(500) = N'Agendamento a cada 60 segundos' + @job_name_CREATE;
DECLARE @command_CREATE  VARCHAR(500) = N'EXEC ZPN_SP_PCI_ATUALIZAPO 0;'
DECLARE @step_name_CREATE VARCHAR(500) =   N'Executar a Procedure - ' + @command_CREATE + ' - ' + @DATABASENAME; 

-- 1. Criar o Job
EXEC sp_add_job 
    @job_name = @job_name_CREATE -- Nome do Job
GO

-- 2. Adicionar uma etapa ao Job para executar a procedure
EXEC sp_add_jobstep
    @job_name =@job_name_CREATE,
    @step_name = @step_name_CREATE,
    @subsystem = N'TSQL',          -- Tipo de tarefa (Transact-SQL)
    @command = @command_CREATE, -- Comando para executar a procedure
    @database_name = @DATABASENAME,    -- Banco de dados no qual a procedure será executada
    @on_success_action = 1,        -- Ação em caso de sucesso (1 = Ir para o próximo passo, 0 = Sair do Job)
    @on_fail_action = 2;           -- Ação em caso de falha (2 = Sair do Job com falha)
GO

-- 3. Criar um agendamento para o Job com intervalo de 30 segundos
EXEC sp_add_schedule 
    @schedule_name = @schedule_name_CREATE, -- Nome do agendamento
    @freq_type = 4,              -- Frequência do agendamento (4 = Diário)
    @freq_interval = 1,          -- Intervalo de frequência (1 = Todo dia)
    @freq_subday_type = 2,       -- Tipo de frequência intradiária (2 = Segundos)
    @freq_subday_interval = 60,  -- Intervalo de execução em segundos
    @active_start_time = 000000; -- Hora de início (HHMMSS)
GO

-- 4. Associar o agendamento ao Job
EXEC sp_attach_schedule 
    @job_name = @job_name_CREATE,
    @schedule_name = @schedule_name_CREATE;
GO

-- 5. Adicionar o Job ao SQL Server Agent
EXEC sp_add_jobserver 
    @job_name = @job_name_CREATE
GO

EXEC sp_start_job 
    @job_name = @job_name_CREATE
GO