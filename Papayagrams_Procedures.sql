
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

CREATE PROCEDURE get_player
	@username VARCHAR(50)
AS
BEGIN
	SELECT id, username, email, CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) password 
	FROM [User] 
	WHERE username = @username;
END;
GO
