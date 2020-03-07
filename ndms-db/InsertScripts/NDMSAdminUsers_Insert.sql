print 'Inserting seed data for Admin Users'
SET IDENTITY_INSERT [ndms].[Users] ON

--Add five admin users
INSERT INTO [ndms].[Users]([ID],[AccountName],[FirstName],[LastName],[Email],[IsAdmin])
  SELECT 1, N'madanV', N'Vishal', N'Madan', N'MadanV@ndms.com', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Users] WHERE AccountName = N'MadanV')

INSERT INTO [ndms].[Users]([ID],[AccountName],[FirstName],[LastName],[Email],[IsAdmin])
  SELECT 2, N'arumugamR', N'Ranga', N'Arumugam', N'ArumugamR@ndms.com', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Users] WHERE AccountName = N'ArumugamR')

INSERT INTO [ndms].[Users]([ID],[AccountName],[FirstName],[LastName],[Email],[IsAdmin])
  SELECT 3, N'freundB', N'Brian', N'Freund', N'FreundB@ndms.com', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Users] WHERE AccountName = N'FreundB')

INSERT INTO [ndms].[Users]([ID],[AccountName],[FirstName],[LastName],[Email],[IsAdmin])
  SELECT 4, N'shahB', N'Bhavik', N'Shah', N'shahB@ndms.com', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Users] WHERE AccountName = N'shahB')

INSERT INTO [ndms].[Users]([ID],[AccountName],[FirstName],[LastName],[Email],[IsAdmin])
  SELECT 5, N'olaleyeA', N'Afo', N'Olaleye', N'olaleyeA@ndms.com', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Users] WHERE AccountName = N'olaleyeA')

SET IDENTITY_INSERT [ndms].[Users] OFF