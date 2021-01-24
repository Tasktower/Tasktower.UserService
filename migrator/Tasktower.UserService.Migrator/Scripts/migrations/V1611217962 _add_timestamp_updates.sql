CREATE TRIGGER [dbo].[updateDatetime]
ON [dbo].[users]
AFTER INSERT, UPDATE 
AS UPDATE [dbo].[users] SET updated_at = GETDATE()
	FROM [users] t
		INNER JOIN inserted i
			ON t.ID = i.ID
GO