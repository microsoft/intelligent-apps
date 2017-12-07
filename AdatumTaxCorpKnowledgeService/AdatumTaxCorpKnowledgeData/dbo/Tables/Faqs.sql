CREATE TABLE [dbo].[Faqs] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Question] NVARCHAR (MAX) NULL,
    [Answer]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Faqs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

