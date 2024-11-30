


-- Trigger for "[11] 1 game won in original game mode"
CREATE TRIGGER VerifyAchievement11
ON OriginalGameHistory
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	DECLARE @playerId int
	SELECT @playerID = userId FROM inserted;

	IF NOT EXISTS (SELECT 1 FROM UserAchieved WHERE userId = @playerId AND achievementId = 11) 
	BEGIN
		IF (SELECT wonGames FROM OriginalGameHistory WHERE userId = @playerId) = 1
        BEGIN
            INSERT INTO UserAchieved (userId, achievementId) VALUES (@playerId, 11);
        END
	END
END;
GO