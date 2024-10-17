
CREATE PROCEDURE register_user
    @Parametro1 VARCHAR(50),
    @Parametro2 VARCHAR(50),
	@Parametro3 VARCHAR(100)
AS
BEGIN
    INSERT INTO [User]
	(username, email, password)
	VALUES 
	(@Parametro1, @Parametro2, ENCRYPTBYASYMKEY(ASYMKEY_ID('asy_TRIGGERDB'), @Parametro3));
END;
