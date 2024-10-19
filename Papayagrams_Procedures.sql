
CREATE PROCEDURE register_user
    @username VARCHAR(50),
    @email VARCHAR(50),
	@password VARCHAR(100)
AS
BEGIN
    INSERT INTO [User]
	(username, email, password)
	VALUES 
	(@username, @email, ENCRYPTBYASYMKEY(ASYMKEY_ID('asy_TRIGGERDB'), @password));
END;
GO

CREATE PROCEDURE log_in
	@username VARCHAR(50),
	@password VARCHAR(100)
AS
BEGIN
	DECLARE @correctPassword VARCHAR(100)
	SELECT @correctPassword = CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) 
	FROM [User] 
	WHERE username = @username;
	IF @password = @correctPassword
	BEGIN 
		RETURN 0
	END
	ELSE
	BEGIN
		RETURN -1
	END
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
