CREATE TABLE [dbo].[Prices] (
    [ID]     INT             IDENTITY (1, 1) NOT NULL,
    [Symbol] VARCHAR (50)    NULL,
    [Open]   DECIMAL (18, 2) NULL,
    [High]   DECIMAL (18, 2) NULL,
    [Low]    DECIMAL (18, 2) NULL,
    [Close]  DECIMAL (18, 2) NULL,
    [Volume] INT             NULL,
    [Date]   DATETIME        NULL
);

