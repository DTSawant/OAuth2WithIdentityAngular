USE [IdentityServerDb]
GO

INSERT INTO [dbo].[IdentityResources]
           ([Enabled]
           ,[Name]
           ,[DisplayName]
           ,[Description]
           ,[Required]
           ,[Emphasize]
           ,[ShowInDiscoveryDocument]
           ,[Created]
           ,[Updated]
           ,[NonEditable])
     VALUES
           (1
           ,'profile'
           ,null
           ,null
           ,0
           ,0
           ,1
           ,'2020-08-30'
           ,null
           ,0)
GO


