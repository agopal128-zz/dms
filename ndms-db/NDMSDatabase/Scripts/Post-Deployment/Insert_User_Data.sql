print 'Inserting seed data for Users'
SET IDENTITY_INSERT [ndms].[Users] ON

INSERT INTO [ndms].[Users]([ID],[AccountName],[FirstName],[LastName],[Email],[IsAdmin])
  SELECT 0, N'NDMS Admin', N'', N'', NULL, 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Users] WHERE AccountName = N'NDMS Admin')

SET IDENTITY_INSERT [ndms].[Users] OFF