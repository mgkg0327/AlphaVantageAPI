

CREATE PROCEDURE [dbo].[sp_ChangePercent] 
	@Symbol VARCHAR(255) = null,
	@StartDate DATETIME,
	@EndDate DATETIME,
	@StartField VARCHAR(255),
	@EndField VARCHAR(255)
AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @Original DECIMAL(18,2)
	DECLARE @New DECIMAL(18,2)
	DECLARE @PercentDecrease INT
	DECLARE @ChangeStatus VARCHAR(255)
	DECLARE @tmp TABLE (Symbol VARCHAR(10), Price DECIMAL(18,2), Date DATETIME)

	IF (@StartField = 'Open')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, [Open], Date FROM Prices WHERE Symbol = @Symbol AND Date = @StartDate		
	END

	ELSE IF (@StartField = 'High')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, High, Date FROM Prices WHERE Symbol = @Symbol AND Date = @StartDate		
	END

	ELSE IF (@StartField = 'Low')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, Low, Date FROM Prices WHERE Symbol = @Symbol AND Date = @StartDate		
	END

	ELSE IF (@StartField = 'Close')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, [Close], Date FROM Prices WHERE Symbol = @Symbol AND Date = @StartDate		
	END




	IF (@EndField = 'Open')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, [Open], Date FROM Prices WHERE Symbol = @Symbol AND Date = @EndDate		
	END

	ELSE IF (@EndField = 'High')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, High, Date FROM Prices WHERE Symbol = @Symbol AND Date = @EndDate		
	END

	ELSE IF (@EndField = 'Low')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, Low, Date FROM Prices WHERE Symbol = @Symbol AND Date = @EndDate		
	END

	ELSE IF (@EndField = 'Close')
	BEGIN
		INSERT INTO @tmp (Symbol, Price, Date) SELECT @Symbol, [Close], Date FROM Prices WHERE Symbol = @Symbol AND Date = @EndDate		
	END
	
	

	SELECT TOP 1
		(Price - LAG(Price, 1) over (ORDER BY date)) / LAG(Price, 1) over (ORDER BY date)*100 as PercentChange
	FROM
		@tmp
	WHERE
		Symbol = @Symbol
	AND
		Date >= @StartDate
	AND
		Date <= @EndDate			
	ORDER BY 
		Date DESC

END