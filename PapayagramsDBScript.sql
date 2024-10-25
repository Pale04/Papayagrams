--Creating an Asymmetric Key (for encrypt data)
CREATE ASYMMETRIC KEY asy_TRIGGERDB
WITH ALGORITHM = RSA_2048
ENCRYPTION BY PASSWORD = 'RD_afAmGsRMYi29'

--Creating tables
CREATE TABLE [User] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [username] varchar(50) NOT NULL,
  [email] varchar(50) NOT NULL,
  [password] varbinary(MAX) NOT NULL
)
GO

CREATE TABLE [OriginalGameHistory] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [wonGames] int,
  [lostGames] int
)
GO

CREATE TABLE [SuddenDeathHistory] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [wonGames] int,
  [lostGames] int
)
GO

CREATE TABLE [TimeAtackHistory] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [wonGames] int,
  [lostGames] int
)
GO

CREATE TABLE [UserRelationship] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [senderId] int,
  [receiverId] int,
  [relationState] VARCHAR(20) CHECK (relationState IN ('request_pending', 'friend', 'blocked'))
)
GO

CREATE TABLE [Achievement] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [description] varchar(100)
)
GO

CREATE TABLE [UserAchieved] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [achievementId] int
)
GO

CREATE TABLE [UserStatus] (
  [userId] int PRIMARY KEY,
  [status] VARCHAR(20) CHECK (status IN ('online', 'offline','in_game')),
  [since] datetime
)
GO

CREATE TABLE [UserConfiguration] (
  [userId] int PRIMARY KEY,
  [selectedLanguage] VARCHAR(20) CHECK (selectedLanguage IN ('auto', 'spanish','english')),
  [typography] int,
  [pieceSize] VARCHAR(20) CHECK (pieceSize IN ('small', 'medium','large')),
  [pieceColor] int,
  [icon] int NOT NULL,
  [cursorDesign] int
)
GO

ALTER TABLE [OriginalGameHistory] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [SuddenDeathHistory] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [TimeAtackHistory] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [UserRelationship] ADD FOREIGN KEY ([senderId]) REFERENCES [User] ([id])
GO

ALTER TABLE [UserRelationship] ADD FOREIGN KEY ([receiverId]) REFERENCES [User] ([id])
GO

ALTER TABLE [UserAchieved] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [UserAchieved] ADD FOREIGN KEY ([achievementId]) REFERENCES [Achievement] ([id])
GO

ALTER TABLE [UserStatus] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [UserConfiguration] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO
