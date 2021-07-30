
CREATE PROCEDURE [dbo].[sp_AddPrices] 
	@Symbol VARCHAR(255) = null,
	@Open DECIMAL(18,2) = null,
	@High DECIMAL(18,2) = null,
	@Low DECIMAL(18,2) = null,
	@Close DECIMAL(18,2) = null,
	@Volume INT = 0,
	@Date DATETIME = '1900-01-01'
AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @cnt INT

	SELECT @cnt = COUNT(*) FROM Prices WHERE Symbol = @Symbol AND Date = @Date

	IF(@cnt < 1)
	INSERT INTO Prices VALUES (@Symbol,@Open,@High,@Low,@Close,@Volume,@Date)

END