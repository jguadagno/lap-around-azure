USE master
GO

-- ***NOTE***: Change `<secure_password>` to a secure password

CREATE Login contacts_user
WITH PASSWORD='<replace me>'

USE Contacts
GO

CREATE USER contacts_user 
FOR LOGIN contacts_user WITH DEFAULT_SCHEMA=dbo;
GO

ALTER ROLE db_datareader ADD MEMBER contacts_user;
ALTER ROLE db_datawriter ADD MEMBER contacts_user;
GO