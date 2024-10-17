
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

CREATE PROCEDURE login
	@username VARCHAR(50),
	@password VARCHAR(50)
AS
BEGIN
	IF EXISTS (SELECT username FROM [User] WHERE username = @username)
	BEGIN
		DECLARE @correctPassword VARCHAR(100);
		SELECT @correctPassword = CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) FROM [User];
		IF @correctPassword = @password
		BEGIN
			SELECT id, username, email, CAST(DecryptByAsymKey (AsymKey_ID('asy_triggerdb'),password,N'RD_afAmGsRMYi29') AS VARCHAR(100)) password FROM [User] WHERE username = @username;
		END
		ELSE
		BEGIN
			THROW 50001,'Incorrect password',1;
		END
	END
	ELSE
	BEGIN
		THROW 50002,'The username does not exist',2;
	END
END;
GO
