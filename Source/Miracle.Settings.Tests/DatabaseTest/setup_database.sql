USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'SettingsUnitTest')
BEGIN
	DROP DATABASE [SettingsUnitTest]
END
GO

CREATE DATABASE [SettingsUnitTest];
GO

USE [SettingsUnitTest]
GO

CREATE TABLE [dbo].[Setting]
(
    [Key] NVARCHAR(50) NOT NULL PRIMARY KEY,
    [Value] NVARCHAR(MAX) NULL
)
GO

INSERT INTO  [dbo].[Setting] ([Key],[Value])
VALUES
(N'Foo', N'Foo from database'),
(N'Bar', N'14'),
(N'Baz', N'hello world')
