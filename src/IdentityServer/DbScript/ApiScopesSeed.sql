USE [IdentityServerDb]
GO

INSERT INTO [dbo].[ApiScopes]
           ([Enabled]
           ,[Name]
           ,[DisplayName]
           ,[Description]
           ,[Required]
           ,[Emphasize]
           ,[ShowInDiscoveryDocument])
     VALUES
           (1
           ,'api2'
           ,'My Api 2'
           ,''
           ,0
           ,0
           ,1)
GO


