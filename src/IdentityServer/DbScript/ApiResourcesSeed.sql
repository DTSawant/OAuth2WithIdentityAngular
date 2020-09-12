USE [IdentityServerDb]
GO

INSERT INTO [dbo].[ApiResources]
           ([Enabled]
           ,[Name]
           ,[DisplayName]
           ,[Description]
           ,[AllowedAccessTokenSigningAlgorithms]
           ,[ShowInDiscoveryDocument]
           ,[Created]
           ,[Updated]
           ,[LastAccessed]
           ,[NonEditable])
     VALUES
           (1
           ,'api2'
           ,'My Api 2'
           ,''
           ,null
           ,1
           ,'2020-08-30'
           ,null
           ,null
           ,0)
GO


