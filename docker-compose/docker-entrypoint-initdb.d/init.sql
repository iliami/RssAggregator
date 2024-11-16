CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241111064041_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241111064041_InitialCreate', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241111111512_Add-AppUser') THEN
    CREATE TABLE "AppUsers" (
        "Id" uuid NOT NULL,
        "Email" text NOT NULL,
        "Password" text NOT NULL,
        "Role" text NOT NULL,
        CONSTRAINT "PK_AppUsers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241111111512_Add-AppUser') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241111111512_Add-AppUser', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    ALTER TABLE "AppUsers" ALTER COLUMN "Role" TYPE character varying(16);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    ALTER TABLE "AppUsers" ALTER COLUMN "Password" TYPE character varying(128);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    ALTER TABLE "AppUsers" ALTER COLUMN "Email" TYPE character varying(32);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    CREATE TABLE "Feeds" (
        "Id" uuid NOT NULL,
        "Name" character varying(128) NOT NULL,
        "Url" character varying(256) NOT NULL,
        "LastFetchedAt" timestamp with time zone,
        CONSTRAINT "PK_Feeds" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    CREATE TABLE "Posts" (
        "Id" uuid NOT NULL,
        "Title" character varying(128) NOT NULL,
        "Content" character varying(8192) NOT NULL,
        "PublishDate" timestamp with time zone NOT NULL,
        "Url" character varying(256) NOT NULL,
        "FeedId" uuid NOT NULL,
        CONSTRAINT "PK_Posts" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Posts_Feeds_FeedId" FOREIGN KEY ("FeedId") REFERENCES "Feeds" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    CREATE INDEX "IX_Posts_FeedId" ON "Posts" ("FeedId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113062933_Add-Feed-Configure-AppUser') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241113062933_Add-Feed-Configure-AppUser', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113131405_Add-Subscriptions') THEN
    CREATE TABLE "Subscription" (
        "AppUserId" uuid NOT NULL,
        "FeedId" uuid NOT NULL,
        "SubscribedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Subscription" PRIMARY KEY ("AppUserId", "FeedId"),
        CONSTRAINT "FK_Subscription_AppUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES "AppUsers" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Subscription_Feeds_FeedId" FOREIGN KEY ("FeedId") REFERENCES "Feeds" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113131405_Add-Subscriptions') THEN
    CREATE INDEX "IX_Subscription_FeedId" ON "Subscription" ("FeedId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113131405_Add-Subscriptions') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241113131405_Add-Subscriptions', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscription" DROP CONSTRAINT "FK_Subscription_AppUsers_AppUserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscription" DROP CONSTRAINT "FK_Subscription_Feeds_FeedId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscription" DROP CONSTRAINT "PK_Subscription";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscription" RENAME TO "Subscriptions";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER INDEX "IX_Subscription_FeedId" RENAME TO "IX_Subscriptions_FeedId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscriptions" ADD CONSTRAINT "PK_Subscriptions" PRIMARY KEY ("AppUserId", "FeedId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscriptions" ADD CONSTRAINT "FK_Subscriptions_AppUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES "AppUsers" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    ALTER TABLE "Subscriptions" ADD CONSTRAINT "FK_Subscriptions_Feeds_FeedId" FOREIGN KEY ("FeedId") REFERENCES "Feeds" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241113142548_Configure-Subscriptions') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241113142548_Configure-Subscriptions', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241114135605_Configure-Posts') THEN
    ALTER TABLE "Posts" DROP COLUMN "Content";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241114135605_Configure-Posts') THEN
    ALTER TABLE "Posts" ALTER COLUMN "Title" TYPE character varying(1024);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241114135605_Configure-Posts') THEN
    ALTER TABLE "Posts" ADD "Description" character varying(32768) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241114135605_Configure-Posts') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241114135605_Configure-Posts', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241114161044_Configure-Feeds') THEN
    ALTER TABLE "Feeds" ADD "Description" character varying(2048) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241114161044_Configure-Feeds') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241114161044_Configure-Feeds', '8.0.10');
    END IF;
END $EF$;
COMMIT;

