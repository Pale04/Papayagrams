CREATE PROCEDURE register_user
    @username VARCHAR(50),
    @email VARCHAR(50),
	@password VARCHAR(100)
AS
BEGIN
    INSERT INTO [User] (username, email, password, accountStatus) VALUES (@username, @email, ENCRYPTBYASYMKEY(ASYMKEY_ID('asy_TRIGGERDB'), @password), 'pending');
	DECLARE @id int
	SELECT @id = SCOPE_IDENTITY();
	INSERT INTO [UserStatus] (userId) VALUES (@id);
	INSERT INTO [UserConfiguration] (userId, selectedLanguage, typography, pieceSize, pieceColor, icon, cursorDesign) VALUES (@id, 'auto', 1, 'small', 1, 1, 1);
	INSERT INTO [OriginalGameHistory] (userId, wonGames, lostGames) VALUES (@id, 0, 0);
	INSERT INTO [SuddenDeathHistory] (userId, wonGames, lostGames) VALUES (@id, 0, 0);
	INSERT INTO [TimeAtackHistory] (userId, wonGames, lostGames) VALUES (@id, 0, 0);
END;
GO

CREATE PROCEDURE log_in
	@username VARCHAR(50),
	@password VARCHAR(100)
AS
BEGIN
	IF EXISTS (SELECT * FROM [User] WHERE username = @username)
	BEGIN
		DECLARE @correctPassword VARCHAR(100)
		SELECT @correctPassword = CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) FROM [User] WHERE username = @username;
		IF @password = @correctPassword
		BEGIN
			DECLARE @accountStatus VARCHAR(MAX)
			SELECT @accountStatus = accountStatus FROM [User] WHERE username = @username
			IF @accountStatus = 'pending'
			BEGIN
				RETURN 1
			END
			ELSE
			BEGIN
				DECLARE @id int
				SELECT @id = id FROM [User] WHERE username = @username;
				UPDATE [UserStatus] SET status='online', since=GETDATE() WHERE userId = @id;
				RETURN 0
			END
		END
		ELSE
		BEGIN
			RETURN -2
		END
	END
	ELSE
	BEGIN
		RETURN -1
	END
END;
GO

CREATE PROCEDURE log_out
	@username VARCHAR(50)
AS
BEGIN
	UPDATE [UserStatus] SET status = 'offline', since = GETDATE() WHERE userId = (SELECT id FROM [User] WHERE username = @username);
END;
GO

CREATE PROCEDURE get_player_by_username
	@username VARCHAR(50)
AS
BEGIN
	SELECT id, username, email, CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) password 
	FROM [User] 
	WHERE username = @username;
END;
GO

CREATE PROCEDURE get_player_by_email
	@email VARCHAR(50)
AS
BEGIN
	SELECT id, username, email, CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) password 
	FROM [User] 
	WHERE email = @email;
END;
GO

CREATE PROCEDURE search_no_friend_player
	@searcherUsername VARCHAR(50),
	@searchedUsername VARCHAR(50)
AS
BEGIN
	DECLARE @searcherId int
	DECLARE @searchedId int
	SELECT @searcherId = id FROM [User] WHERE username = @searcherUsername;
	SELECT @searchedId = id FROM [User] WHERE username = @searchedUsername;
	SELECT * FROM [User] 
	WHERE username = @searchedUsername 
	AND NOT username = @searcherUsername
	AND NOT EXISTS (SELECT * FROM [UserRelationship] 
					WHERE ((senderId = @searcherId AND receiverId = @searchedId) OR (senderId = @searchedId AND receiverId = @searcherId))
					AND relationState = 'friend');
END;
GO

--The case when the relationState is blocked was omitted because the blocked players cannot be searched for send a friend request (in both directions)
CREATE PROCEDURE send_friend_request
	@senderUsername VARCHAR(50),
	@receiverUsername VARCHAR(50)
AS
BEGIN
	DECLARE @senderId int
	DECLARE @receiverId int
	SELECT @senderId = id FROM [User] WHERE username = @senderUsername;
	SELECT @receiverId = id FROM [User] WHERE username = @receiverUsername;
	IF EXISTS (SELECT * FROM [UserRelationship] WHERE senderId = @senderId AND receiverId = @receiverId AND relationState = 'request_pending')
	BEGIN
		 RETURN -1
	END
	ELSE IF EXISTS (SELECT * FROM [UserRelationship] WHERE senderId = @receiverId AND receiverId = @senderId  AND relationState = 'request_pending')
	BEGIN
		RETURN -2
	END
	ELSE IF EXISTS (SELECT * FROM [UserRelationship] WHERE (senderId = @senderId AND receiverId = @receiverId) OR (senderId = @receiverId AND receiverId = @senderId) AND relationState = 'friend')
	BEGIN
		RETURN -3
	END
	ELSE
	BEGIN
		INSERT INTO [UserRelationship] (senderId, receiverId, relationState) VALUES (@senderId, @receiverId, 'request_pending');
		RETURN 0
	END
END;
GO

