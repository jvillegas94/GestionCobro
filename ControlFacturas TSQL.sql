use master
go
declare @IDSesion int=0;

DECLARE Sesiones CURSOR FOR  

SELECT a.session_id

FROM sys.dm_exec_sessions a

inner JOIN sys.dm_exec_connections b on a.session_id=b.session_id

join sys.databases c on a.database_id=c.database_id

where name='GestionCobroBD' and a.session_id<>@@SPID;

OPEN Sesiones

FETCH NEXT FROM Sesiones   

INTO @IDSesion

WHILE @@FETCH_STATUS = 0  

BEGIN 

exec('KILL '+@IDSesion); 

        FETCH NEXT FROM Sesiones INTO @IDSesion  

        END  

CLOSE Sesiones  

    DEALLOCATE Sesiones 
go
drop database if exists GestionCobroBD
go
create database GestionCobroBD
go
use GestionCobroBD
go
set nocount on
go
Create table ControlFacturas(
NoSeq int identity(1,1) primary key,
Empresa nvarchar(50),
Tipo nvarchar(50),
NoDocumento int,
Condiciones nvarchar(100),
Fecha date,
CardCode nvarchar(50),
CardName nvarchar(250),
Total decimal(18,2),
Usuario nvarchar(250),
Despacho date,
Facturacion Datetime,
CuentasPorCobrar Datetime
);
go
Select * from ControlFacturas where CuentasPorCobrar is null
go
 SELECT isnull(Usuario,'') FROM [GestionCobroBD].[dbo].[ControlFacturas] WHERE [Empresa] = 'Empagro' AND [Tipo] = 'Tipo' AND [NoDocumento] = 1031927