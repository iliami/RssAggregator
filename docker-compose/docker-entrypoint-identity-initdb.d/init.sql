CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250120150845_Initial') THEN
    CREATE TABLE "IdentityEvents" (
        "Id" uuid NOT NULL,
        "EmittedAt" timestamp with time zone NOT NULL,
        "ContentBlob" bytea NOT NULL,
        CONSTRAINT "PK_IdentityEvents" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250120150845_Initial') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Email" character varying(32) NOT NULL,
        "Password" character varying(128) NOT NULL,
        "Role" character varying(16) NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250120150845_Initial') THEN
    CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250120150845_Initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250120150845_Initial', '8.0.11');
    END IF;
END $EF$;
COMMIT;

