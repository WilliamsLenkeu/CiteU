
    -- --------------------------------------------------
    -- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
    -- --------------------------------------------------
    -- Date Created: 01/20/2024 12:44:35
    -- Generated from EDMX file: C:\c#\ClassLibrary1\ClassLibrary1\Model1.edmx
    -- --------------------------------------------------

    SET QUOTED_IDENTIFIER OFF;
    GO
    USE [CiteU];
    GO
    IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
    GO

    -- --------------------------------------------------
    -- Dropping existing FOREIGN KEY constraints
    -- --------------------------------------------------

    IF OBJECT_ID(N'[dbo].[FK_BatimentsChambre]', 'F') IS NOT NULL
        ALTER TABLE [dbo].[ChambreSet] DROP CONSTRAINT [FK_BatimentsChambre];
    GO
    IF OBJECT_ID(N'[dbo].[FK_PaimentEtudiants]', 'F') IS NOT NULL
        ALTER TABLE [dbo].[PaimentSet] DROP CONSTRAINT [FK_PaimentEtudiants];
    GO
    IF OBJECT_ID(N'[dbo].[FK_ReservationPaiment]', 'F') IS NOT NULL
        ALTER TABLE [dbo].[PaimentSet] DROP CONSTRAINT [FK_ReservationPaiment];
    GO
    IF OBJECT_ID(N'[dbo].[FK_ChambreReservation]', 'F') IS NOT NULL
        ALTER TABLE [dbo].[ReservationSet] DROP CONSTRAINT [FK_ChambreReservation];
    GO

    -- --------------------------------------------------
    -- Dropping existing tables
    -- --------------------------------------------------

    IF OBJECT_ID(N'[dbo].[BatimentsSet]', 'U') IS NOT NULL
        DROP TABLE [dbo].[BatimentsSet];
    GO
    IF OBJECT_ID(N'[dbo].[ChambreSet]', 'U') IS NOT NULL
        DROP TABLE [dbo].[ChambreSet];
    GO
    IF OBJECT_ID(N'[dbo].[EtudiantsSet]', 'U') IS NOT NULL
        DROP TABLE [dbo].[EtudiantsSet];
    GO
    IF OBJECT_ID(N'[dbo].[PaimentSet]', 'U') IS NOT NULL
        DROP TABLE [dbo].[PaimentSet];
    GO
    IF OBJECT_ID(N'[dbo].[ReservationSet]', 'U') IS NOT NULL
        DROP TABLE [dbo].[ReservationSet];
    GO

    -- --------------------------------------------------
    -- Creating all tables
    -- --------------------------------------------------

    -- Creating table 'BatimentsSet'
    CREATE TABLE [dbo].[BatimentsSet] (
        [Id_batiments] int IDENTITY(1,1) NOT NULL,
        [Nom_Batiment] nvarchar(max)  NOT NULL,
        [Nombre_etage] int  NOT NULL,
        [Nombre_Chambre_Par_Etage] int  NOT NULL,
        [Nombre_Lits_Par_Chambre] int  NOT NULL,
        [Prix_Chambre] int  NOT NULL
    );
    GO

    -- Creating table 'ChambreSet'
    CREATE TABLE [dbo].[ChambreSet] (
        [Id_Chambre] int IDENTITY(1,1) NOT NULL,
        [Niveau] nvarchar(max)  NOT NULL,
        [BatimentsId_batiments] int  NOT NULL
    );
    GO

    -- Creating table 'EtudiantsSet'
    CREATE TABLE [dbo].[EtudiantsSet] (
        [Matricule] nvarchar(50)  NOT NULL,
        [Nom] nvarchar(max)  NOT NULL,
        [Sexe] nvarchar(1)  NOT NULL,
        [Niveau] nvarchar(max)  NOT NULL,
        [Handicape] bit  NOT NULL,
        [Age] int  NOT NULL
    );
    GO

    -- Creating table 'PaimentSet'
    CREATE TABLE [dbo].[PaimentSet] (
        [Id_Paiement] int IDENTITY(1,1) NOT NULL,
        [Montant] int  NOT NULL,
        [Lieu_Paiement] nvarchar(max)  NOT NULL,
        [ChambreId_Chambre] int  NOT NULL,
        [ReservationId_Reservation] int  NOT NULL,
        [Etudiants_Matricule] nvarchar(50)  NOT NULL
    );
    GO

    -- Creating table 'ReservationSet'
    CREATE TABLE [dbo].[ReservationSet] (
        [Id_Reservation] int IDENTITY(1,1) NOT NULL,
        [Date_Debut] datetime  NOT NULL,
        [Date_Fin] datetime  NOT NULL,
        [ChambreId_Chambre] int  NOT NULL
    );
    GO

    -- --------------------------------------------------
    -- Creating all PRIMARY KEY constraints
    -- --------------------------------------------------

    -- Creating primary key on [Id_batiments] in table 'BatimentsSet'
    ALTER TABLE [dbo].[BatimentsSet]
    ADD CONSTRAINT [PK_BatimentsSet]
        PRIMARY KEY CLUSTERED ([Id_batiments] ASC);
    GO

    -- Creating primary key on [Id_Chambre] in table 'ChambreSet'
    ALTER TABLE [dbo].[ChambreSet]
    ADD CONSTRAINT [PK_ChambreSet]
        PRIMARY KEY CLUSTERED ([Id_Chambre] ASC);
    GO

    -- Creating primary key on [Matricule] in table 'EtudiantsSet'
    ALTER TABLE [dbo].[EtudiantsSet]
    ADD CONSTRAINT [PK_EtudiantsSet]
        PRIMARY KEY CLUSTERED ([Matricule] ASC);
    GO

    -- Creating primary key on [Id_Paiement] in table 'PaimentSet'
    ALTER TABLE [dbo].[PaimentSet]
    ADD CONSTRAINT [PK_PaimentSet]
        PRIMARY KEY CLUSTERED ([Id_Paiement] ASC);
    GO

    -- Creating primary key on [Id_Reservation] in table 'ReservationSet'
    ALTER TABLE [dbo].[ReservationSet]
    ADD CONSTRAINT [PK_ReservationSet]
        PRIMARY KEY CLUSTERED ([Id_Reservation] ASC);
    GO

    -- --------------------------------------------------
    -- Creating all FOREIGN KEY constraints
    -- --------------------------------------------------

    -- Creating foreign key on [BatimentsId_batiments] in table 'ChambreSet'
    ALTER TABLE [dbo].[ChambreSet]
    ADD CONSTRAINT [FK_BatimentsChambre]
        FOREIGN KEY ([BatimentsId_batiments])
        REFERENCES [dbo].[BatimentsSet]
            ([Id_batiments])
        ON DELETE NO ACTION ON UPDATE NO ACTION;
    GO

    -- Creating non-clustered index for FOREIGN KEY 'FK_BatimentsChambre'
    CREATE INDEX [IX_FK_BatimentsChambre]
    ON [dbo].[ChambreSet]
        ([BatimentsId_batiments]);
    GO

    -- Creating foreign key on [Etudiants_Matricule] in table 'PaimentSet'
    ALTER TABLE [dbo].[PaimentSet]
    ADD CONSTRAINT [FK_PaimentEtudiants]
        FOREIGN KEY ([Etudiants_Matricule])
        REFERENCES [dbo].[EtudiantsSet]
            ([Matricule])
        ON DELETE NO ACTION ON UPDATE NO ACTION;
    GO

    -- Creating non-clustered index for FOREIGN KEY 'FK_PaimentEtudiants'
    CREATE INDEX [IX_FK_PaimentEtudiants]
    ON [dbo].[PaimentSet]
        ([Etudiants_Matricule]);
    GO

    -- Creating foreign key on [ReservationId_Reservation] in table 'PaimentSet'
    ALTER TABLE [dbo].[PaimentSet]
    ADD CONSTRAINT [FK_ReservationPaiment]
        FOREIGN KEY ([ReservationId_Reservation])
        REFERENCES [dbo].[ReservationSet]
            ([Id_Reservation])
        ON DELETE NO ACTION ON UPDATE NO ACTION;
    GO

    -- Creating non-clustered index for FOREIGN KEY 'FK_ReservationPaiment'
    CREATE INDEX [IX_FK_ReservationPaiment]
    ON [dbo].[PaimentSet]
        ([ReservationId_Reservation]);
    GO

    -- Creating foreign key on [ChambreId_Chambre] in table 'ReservationSet'
    ALTER TABLE [dbo].[ReservationSet]
    ADD CONSTRAINT [FK_ChambreReservation]
        FOREIGN KEY ([ChambreId_Chambre])
        REFERENCES [dbo].[ChambreSet]
            ([Id_Chambre])
        ON DELETE NO ACTION ON UPDATE NO ACTION;
    GO

    -- Creating non-clustered index for FOREIGN KEY 'FK_ChambreReservation'
    CREATE INDEX [IX_FK_ChambreReservation]
    ON [dbo].[ReservationSet]
        ([ChambreId_Chambre]);
    GO

    -- --------------------------------------------------
    -- Script has ended
    -- --------------------------------------------------